#region File Description
//-----------------------------------------------------------------------------
// DuckHuntGameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Audio;
#endregion

namespace PongaThemes
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class DuckHuntGameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        
        /* * * * * * * * * * * TEXTURES * * * * * * * * * * * */

        Texture2D tree;
        Texture2D ground;
        Texture2D bush;
        Texture2D ScreenTexture; // Debug texture
        Texture2D debug;
        Texture2D walkingdog;
        Texture2D flyawaysign;
        Texture2D flash;

        Texture2D[] clouds = new Texture2D[3];

        /* * * * * * * * * * * * SOUNDS * * * * * * * * * * * */

        SoundEffect dogbark;
        SoundEffect startround;
        SoundEffect m_shot;
        SoundEffect endround;
        SoundEffect pause;

        /* * * * * * * * * * * ANIMATIONS * * * * * * * * * */
        
        AnimatedSprite wdog = new AnimatedSprite();     // Walking Dog Animation (with sound)
        AnimatedSprite sdog = new AnimatedSprite();     // Sniff Dog Animation
        AnimatedSprite sprdog = new AnimatedSprite();   // Suprised Dog Animation
        AnimatedSprite jdog1 = new AnimatedSprite();
        AnimatedSprite jdog2 = new AnimatedSprite();
        AnimatedSprite jdog3 = new AnimatedSprite();

        /* * * * * * * * * ANIMATION SCENE * * * * * * * * */

        AnimationScene m_intro = new AnimationScene();

        // Needed
        Rectangle boxrec = new Rectangle(HelperUtils.SafeBoundary.X + 96, HelperUtils.SafeBoundary.Y, 832, 512);
        //Rectangle m_background_color_rect = new Rectangle(0, 0, 1024, 612);

        Sprite m_fly_sign = new Sprite();

        /**************************/
        bool m_paused = false;
        bool m_played_intro = false;
        bool m_built_ending = false;

        CountTimer m_endround = new CountTimer();

        DuckPopulation m_ducks;

        Rectangle[] newbound;
        CollisionData[] bounddata;

        DuckHuntPlayer m_player1;
        DuckHuntPlayer m_player2;

        Texture2D[] m_player_textures = new Texture2D[5];

        Clouds m_clouds_p1;
        Clouds m_clouds_p2;
        Cloud[] m_clouds; 
        
        Drawable m_flash;
        Drawable m_duck1_flash = new Image();
        Drawable m_duck2_flash = new Image();

        private SpriteFont m_num;
        private SpriteFont m_score_fnt;

        /****************************/

        float pauseAlpha;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public DuckHuntGameplayScreen()
        {
            //TransitionOnTime = TimeSpan.FromSeconds(1.5);
            //TransitionOffTime = TimeSpan.FromSeconds(0.5);       
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            /*  LOAD TEXTURES, SOUNDS AND FONTS */

            tree = content.Load<Texture2D>("DuckHunt\\Textures\\tree");
            ground = content.Load<Texture2D>("DuckHunt\\Textures\\ground");
            bush = content.Load<Texture2D>("DuckHunt\\Textures\\bush");
            m_player_textures[0] = content.Load<Texture2D>("DuckHunt\\Textures\\panel");
            walkingdog = content.Load<Texture2D>("DuckHunt\\Textures\\walkdog");
            flyawaysign = content.Load<Texture2D>("DuckHunt\\Textures\\flyawaysign");
            m_player_textures[1] = content.Load<Texture2D>("DuckHunt\\Textures\\score");
            m_player_textures[2] = content.Load<Texture2D>("DuckHunt\\Textures\\clouds");
            m_player_textures[3] = content.Load<Texture2D>("DuckHunt\\Textures\\duckcall");
            m_player_textures[4] = content.Load<Texture2D>("DuckHunt\\Textures\\shot");
            clouds[0] = content.Load<Texture2D>("DuckHunt\\Textures\\smallcloud");
            clouds[1] = content.Load<Texture2D>("DuckHunt\\Textures\\medcloud");
            clouds[2] = content.Load<Texture2D>("DuckHunt\\Textures\\bigcloud");

            dogbark = content.Load<SoundEffect>("DuckHunt\\Sounds\\dogbark");
            m_shot = content.Load<SoundEffect>("DuckHunt\\Sounds\\shot");
            startround = content.Load<SoundEffect>("DuckHunt\\Sounds\\startround");
            endround = content.Load<SoundEffect>("DuckHunt\\Sounds\\roundend");
            pause = content.Load<SoundEffect>("DuckHunt\\Sounds\\pause");

            m_num = content.Load<SpriteFont>("Fonts\\bignesfont");
            m_score_fnt = content.Load<SpriteFont>("Fonts\\hugenesfont");

            debug = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            debug.SetData(new Color[] { new Color(255, 255, 255) });

            m_clouds_p1 = new Clouds(0, clouds);//, debug);
            m_clouds_p2 = new Clouds(1, clouds);//, debug);
            m_clouds = new Cloud[m_clouds_p1.CloudsA.Length*2]; 

            m_clouds[0] = m_clouds_p1.CloudsA[0];
            m_clouds[1] = m_clouds_p1.CloudsA[1];
            m_clouds[2] = m_clouds_p1.CloudsA[2];
            m_clouds[3] = m_clouds_p1.CloudsA[0];
            m_clouds[4] = m_clouds_p1.CloudsA[1];
            m_clouds[5] = m_clouds_p1.CloudsA[2];
            m_clouds[6] = m_clouds_p2.CloudsA[0];
            m_clouds[7] = m_clouds_p2.CloudsA[1];
            m_clouds[8] = m_clouds_p2.CloudsA[2];
            m_clouds[9] = m_clouds_p2.CloudsA[0];
            m_clouds[10] = m_clouds_p2.CloudsA[1];
            m_clouds[11] = m_clouds_p2.CloudsA[2];
            
            /* SETUP ALL ANIMATIONSPRITES */
            wdog.BuildAnimation(walkingdog, 1, 8, true, new int[4] { 1, 2, 3, 4 });
            wdog.SetFrame(0, 8, null);
            wdog.SetFrame(1, 8, null);
            wdog.SetFrame(2, 8, null);
            wdog.SetFrame(3, 8, null);
            wdog.X_Pos = HelperUtils.SafeBoundary.X + 22;
            wdog.Y_Pos = HelperUtils.SafeBoundary.Y + 471;

            sdog.BuildAnimation(walkingdog, 1, 8, true, new int[6] { 1, 0, 1, 0, 1, 0 });
            sdog.SetFrame(0, 8, null);
            sdog.SetFrame(1, 8, null);
            sdog.SetFrame(2, 8, null);
            sdog.SetFrame(3, 8, null);
            sdog.SetFrame(4, 8, null);
            sdog.SetFrame(5, 8, null);

            sprdog.BuildAnimation(walkingdog, 1, 8, true, new int[1] { 5 });
            sprdog.SetFrame(0, 100, null);

            jdog1.BuildAnimation(walkingdog, 1, 8, true, new int[1] { 6 });
            jdog1.SetFrame(0, 1000, dogbark);

            jdog2.BuildAnimation(walkingdog, 1, 8, true, new int[1] { 6 });
            jdog2.SetFrame(0, 50, dogbark);

            jdog3.BuildAnimation(walkingdog, 1, 8, true, new int[1] { 7 });
            jdog3.SetFrame(0, 50, dogbark);

            /*   MAKE INTRO ANIMATION   */
            m_intro.AddAnimation(new DirXYMover(wdog, 180, 0, 1.7f));
            m_intro.AddAnimation(new TimeOutDrawable(sdog, 49, true));
            m_intro.AddAnimation(new DirXYMover(wdog, 180, 0, 1.7f));
            m_intro.AddAnimation(new TimeOutDrawable(sdog, 49, true));
            m_intro.AddAnimation(new DirXYMover(wdog, 10, 0, 1.7f));
            m_intro.AddAnimation(new TimeOutDrawable(sprdog, 49, true));
            m_intro.AddAnimation(new DirXYMover(jdog1, 5, -50, 3.5f));
            m_intro.AddAnimation(new DirXYMover(jdog1, 15, -25, 3.0f));
            m_intro.AddAnimation(new DirXYMover(jdog2, 15, -25, 3.0f));
            m_intro.AddAnimation(new DirXYMover(jdog3, 40, 40, 3.0f));
            m_intro.BuildScene(new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            m_intro.Scene_State = DrawableState.Active;

            /* Make a texture for bg. NOT gonna be needed when I change the dimension of screen */
            ScreenTexture = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            ScreenTexture.SetData(new Color[] { Color.White });

            /* Build the SHOT animations */
            flash = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            flash.SetData(new Color[] { new Color(255, 255, 255, 128) });

            m_flash = new TimeOutDrawable(new Image(HelperUtils.SafeBoundary.X, HelperUtils.SafeBoundary.Y, 1024, 612, flash), 3, false);
            m_flash.Draw_State = DrawableState.Finished;
            
            /* Create Players */
            m_player1 = new DuckHuntPlayer(0); 
            m_player1.LoadContent(m_player_textures, m_num, m_score_fnt, boxrec);
            m_player2 = new DuckHuntPlayer(1); 
            m_player2.LoadContent(m_player_textures, m_num, m_score_fnt, boxrec);

            newbound = Boundary.CreateBoundRects(boxrec);
            bounddata = new CollisionData[newbound.Length];

            bounddata[0] = new CollisionData(newbound[0]);
            bounddata[1] = new CollisionData(newbound[1]);
            bounddata[2] = new CollisionData(newbound[2]);
            bounddata[3] = new CollisionData(newbound[3]);
            bounddata[4] = new CollisionData(newbound[4]);
            bounddata[5] = new CollisionData(newbound[5]);
            bounddata[6] = new CollisionData(newbound[6]);
            bounddata[7] = new CollisionData(newbound[7]);
            bounddata[8] = new CollisionData(newbound[8]);
            bounddata[9] = new CollisionData(newbound[9]);

            m_ducks = new DuckPopulation(content, bounddata);

            // FLY AWAY SIGN
            m_fly_sign.Texture = flyawaysign;
            m_fly_sign.X_Pos = ScreenManager.GraphicsDevice.Viewport.Width / 2 - m_fly_sign.Texture.Width / 2;
            m_fly_sign.Y_Pos = HelperUtils.SafeBoundary.Y + 256;
            
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (!otherScreenHasFocus)
            {
                m_paused = false;
            }

            if (!m_paused)
            {
                if(m_intro.Scene_State != DrawableState.Finished) {
                    if (!m_played_intro)
                    {
                        startround.Play();
                        m_played_intro = true;
                    }
                    m_intro.Update();  
                }
                if (m_intro.Scene_State == DrawableState.Finished && 
                    m_ducks.BallsAlive()) 
                {
                    m_ducks.UpdateBalls(m_player1.CData, m_player2.CData, m_clouds);
                }

                if (m_intro.Scene_State == DrawableState.Finished &&  
                    m_ducks.BallsDead())
                {
                    if (m_ducks.DuckOneState == BallState.DeadBall && m_ducks.DuckOneDir == Direction.Right)
                    {
                        m_player1.ScoreNum += 1;
                    }
                    else if (m_ducks.DuckOneState == BallState.DeadBall && m_ducks.DuckOneDir == Direction.Left)
                    {
                        m_player2.ScoreNum += 1;
                    }

                    if (m_ducks.DuckTwoState == BallState.DeadBall && m_ducks.DuckTwoDir == Direction.Right)
                    {
                        m_player1.ScoreNum += 1;
                    }
                    else if (m_ducks.DuckTwoState == BallState.DeadBall && m_ducks.DuckTwoDir == Direction.Left)
                    {
                        m_player2.ScoreNum += 1;
                    }


                    m_ducks.BuildIntermission();
                }

                if (m_intro.Scene_State == DrawableState.Finished &&     
                    m_ducks.Intermission())
                { 
                    m_ducks.UpdateIntermission();
                }
                else if (!m_ducks.Intermission() && 
                         m_ducks.BallsLimbo()) 
                {
                    m_ducks.ReleaseDuck();
                }

                if (m_intro.Scene_State == DrawableState.Finished)
                {
                    m_flash.Update();
                    m_clouds_p1.Update();
                    m_clouds_p2.Update();

                    m_clouds[0] = m_clouds_p1.CloudsA[0];
                    m_clouds[1] = m_clouds_p1.CloudsA[1];
                    m_clouds[2] = m_clouds_p1.CloudsA[2];
                    m_clouds[3] = m_clouds_p2.CloudsA[0];
                    m_clouds[4] = m_clouds_p2.CloudsA[1];
                    m_clouds[5] = m_clouds_p2.CloudsA[2];

                    m_duck1_flash.Update();
                    m_duck1_flash.X_Pos = m_ducks.DuckOneRectangle.X;
                    m_duck1_flash.Y_Pos = m_ducks.DuckOneRectangle.Y;                                      
                    m_duck2_flash.Update();
                    m_duck2_flash.X_Pos = m_ducks.DuckOneRectangle.X;
                    m_duck2_flash.Y_Pos = m_ducks.DuckOneRectangle.Y;
                    m_ducks.UpdateCounter();
                    m_player1.UpdateItems();
                    m_player2.UpdateItems();

                    if (m_ducks.DuckOneState == BallState.Active)
                    {
                        m_ducks.DuckOneXVelocity += m_ducks.DuckOneXVelocity * 0.0001f;
                        m_ducks.DuckOneYVelocity += m_ducks.DuckOneYVelocity * 0.0001f;
                    }
                    else if (m_ducks.DuckTwoState == BallState.Active)
                    {
                        m_ducks.DuckTwoXVelocity += m_ducks.DuckTwoXVelocity * 0.0001f;
                        m_ducks.DuckTwoYVelocity += m_ducks.DuckTwoYVelocity * 0.0001f;
                    }
                }

                if (!m_built_ending && m_ducks.GameOver())
                {
                    endround.Play();
                    m_endround.Start(4);
                    m_ducks.SetAllDucksToBlink();
                    m_built_ending = true;
                }
                if (m_endround.isRunning && m_endround.CheckTime(gameTime))
                {
                    ScreenManager.AddScreen(new DuckHuntGameOverMenuScreen(this), null);
                    m_paused = true;
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            PlayerIndex playerIndex;

            // Look up inputs for the active player profile.
            //int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState1 = input.CurrentKeyboardStates[0];
            GamePadState gamePadState1 = input.CurrentGamePadStates[0];
            KeyboardState keyboardState2 = input.CurrentKeyboardStates[1];
            GamePadState gamePadState2 = input.CurrentGamePadStates[1];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState1.IsConnected &&
                                       input.GamePadWasConnected[0] && 
                                       !gamePadState2.IsConnected &&
                                       input.GamePadWasConnected[1];

            if ((input.IsPauseGame(null) || gamePadDisconnected) && m_intro.Scene_State == DrawableState.Finished)
            {
                pause.Play();
                m_paused = true;
                ScreenManager.AddScreen(new DuckHuntPauseMenuScreen(this), null);
            }

             // Handle Cloud Input
             // (DuckHuntPlayer 1)
            if ((input.IsNewKeyPress(Keys.D, PlayerIndex.One, out playerIndex) ||
               input.IsNewButtonPress(Buttons.X, PlayerIndex.One, out playerIndex)) &&
               m_intro.Scene_State == DrawableState.Finished &&
               !m_ducks.Intermission() &&
               m_player1.CloudNum != 0 &&
               m_ducks.BallsAlive() &&
               m_clouds_p1.State == DrawableState.Finished)
            {
                m_clouds_p1.Reset();
                
                m_player1.CloudNum -= 1;
            }

            // (DuckHuntPlayer 2)
            if ((input.IsNewKeyPress(Keys.OemComma, PlayerIndex.Two, out playerIndex) ||
                input.IsNewButtonPress(Buttons.X, PlayerIndex.Two, out playerIndex)) &&
                m_intro.Scene_State == DrawableState.Finished &&
                !m_ducks.Intermission() &&
                m_player2.CloudNum != 0 &&
                m_ducks.BallsAlive()  &&
                m_clouds_p2.State == DrawableState.Finished)
            {
                m_clouds_p2.Reset();
                
                m_player2.CloudNum -= 1;
            }
            

            // Handle Duck Calls
            // (DuckHuntPlayer 1)
            if ((input.IsNewKeyPress(Keys.F, PlayerIndex.One, out playerIndex) ||
                input.IsNewButtonPress(Buttons.Y, PlayerIndex.One, out playerIndex)) &&
                m_intro.Scene_State == DrawableState.Finished && 
                !m_ducks.Intermission() && 
                m_player1.DuckCallNum != 0 &&
                m_ducks.OneBallAlive())
            {
                m_ducks.ReleaseDuck();
                // Play duck call sound
                m_player1.DuckCallNum -= 1;
            }

            // (DuckHuntPlayer 2)
            if ((input.IsNewKeyPress(Keys.OemPeriod, PlayerIndex.Two, out playerIndex) ||
                input.IsNewButtonPress(Buttons.Y, PlayerIndex.Two, out playerIndex)) &&
                m_intro.Scene_State == DrawableState.Finished &&
                !m_ducks.Intermission() &&
                m_player2.DuckCallNum != 0 &&
                m_ducks.OneBallAlive())
            {
                m_ducks.ReleaseDuck();
                // Play duck call sound
                m_player2.DuckCallNum -= 1;
            }

            // Handle Shot Input
            // (DuckHuntPlayer 1)
            if ((input.IsNewKeyPress(Keys.G, PlayerIndex.One, out playerIndex) ||
               input.IsNewButtonPress(Buttons.B, PlayerIndex.One, out playerIndex)) &&
                m_intro.Scene_State == DrawableState.Finished &&
                !m_ducks.Intermission() &&
                m_player1.ShotNum != 0 &&
                m_ducks.BallsAlive())
            {
                if (m_ducks.DuckOneState == BallState.Active)
                {
                    m_ducks.DuckOneXVelocity *= -1.5f;
                    m_ducks.DuckOneYVelocity *= 1.5f;
                    m_ducks.DuckOneHit = Direction.None;
                    if (m_ducks.DuckOneDir == Direction.Right)
                    {
                        m_ducks.DuckOneDir = Direction.Left;
                    }
                    else if (m_ducks.DuckOneDir == Direction.Left)
                    {
                        m_ducks.DuckOneDir = Direction.Right;
                    }

                    m_duck1_flash = new TimeOutDrawable(new Image(m_ducks.DuckOneRectangle.X,
                                                              m_ducks.DuckOneRectangle.Y,
                                                              40, 40, flash), 5, false);
                }

                if (m_ducks.DuckTwoState == BallState.Active)
                {
                    m_ducks.DuckTwoXVelocity *= -1.5f;
                    m_ducks.DuckTwoYVelocity *= 1.5f;
                    m_ducks.DuckTwoHit = Direction.None;
                    if (m_ducks.DuckTwoDir == Direction.Right)
                    {
                        m_ducks.DuckTwoDir = Direction.Left;
                    }
                    else if (m_ducks.DuckTwoDir == Direction.Left)
                    {
                        m_ducks.DuckTwoDir = Direction.Right;
                    }

                    m_duck2_flash = new TimeOutDrawable(new Image(m_ducks.DuckTwoRectangle.X,
                                                              m_ducks.DuckTwoRectangle.Y,
                                                              40, 40, flash), 8, false);
                }
                m_duck1_flash.Tint = new Color(255, 255, 255, 128);
                m_duck2_flash.Tint = new Color(255, 255, 255, 128);
                m_flash.Reset();
                m_player1.ShotNum -= 1;
                m_shot.Play();
            }

            // (DuckHuntPlayer 2)
            if ((input.IsNewKeyPress(Keys.OemQuestion, PlayerIndex.Two, out playerIndex) ||
                input.IsNewButtonPress(Buttons.B, PlayerIndex.Two, out playerIndex)) &&
                m_intro.Scene_State == DrawableState.Finished &&
                !m_ducks.Intermission() &&
                m_player2.ShotNum != 0 &&
                m_ducks.BallsAlive())
            {
                if (m_ducks.DuckOneState == BallState.Active)
                {
                    m_ducks.DuckOneXVelocity *= -1.5f;
                    m_ducks.DuckOneYVelocity *= 1.5f;
                    m_ducks.DuckOneHit = Direction.None;
                    if (m_ducks.DuckOneDir == Direction.Right)
                    {
                        m_ducks.DuckOneDir = Direction.Left;
                    }
                    else if (m_ducks.DuckOneDir == Direction.Left)
                    {
                        m_ducks.DuckOneDir = Direction.Right;
                    }

                    m_duck1_flash = new TimeOutDrawable(new Image(m_ducks.DuckOneRectangle.X,
                                                              m_ducks.DuckOneRectangle.Y,
                                                              40, 40, flash), 8, false);
                }

                if (m_ducks.DuckTwoState == BallState.Active)
                {
                    m_ducks.DuckTwoXVelocity *= -1.5f;
                    m_ducks.DuckTwoYVelocity *= 1.5f;
                    m_ducks.DuckTwoHit = Direction.None;
                    if (m_ducks.DuckTwoDir == Direction.Right)
                    {
                        m_ducks.DuckTwoDir = Direction.Left;
                    }
                    else if (m_ducks.DuckTwoDir == Direction.Left)
                    {
                        m_ducks.DuckTwoDir = Direction.Right;
                    }
                    m_duck2_flash = new TimeOutDrawable(new Image(m_ducks.DuckTwoRectangle.X,
                                                              m_ducks.DuckTwoRectangle.Y,
                                                              40, 40, flash), 8, false);
                }
                m_duck1_flash.Tint = new Color(255, 255, 255, 128);
                m_duck2_flash.Tint = new Color(255, 255, 255, 128);
                m_flash.Reset();
                m_shot.Play();
                m_player2.ShotNum -= 1;
            }

            if (m_intro.Scene_State == DrawableState.Finished) 
            {            
                m_player1.UpdatePaddle(keyboardState1, gamePadState1, m_clouds_p2.CloudsA);
                m_player2.UpdatePaddle(keyboardState2, gamePadState2, m_clouds_p1.CloudsA);
            }  
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {   
            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            if (m_ducks.IntermissionScene == 0)
            {
                spriteBatch.Draw(ScreenTexture, HelperUtils.SafeBoundary, new Color(247, 206, 176, 255));
                //ScreenManager.GraphicsDevice.Clear(new Color(247, 206, 176, 255));
                m_fly_sign.Draw(spriteBatch);      
            }
            else 
            {
                spriteBatch.Draw(ScreenTexture, HelperUtils.SafeBoundary, new Color(49, 192, 250, 255));
                //ScreenManager.GraphicsDevice.Clear(new Color(49, 192, 250, 255));
            }

            // Draw tree
            spriteBatch.Draw(tree, new Vector2(HelperUtils.SafeBoundary.X + 122, HelperUtils.SafeBoundary.Y + 236), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0); 
            
            // Draw Bush
            spriteBatch.Draw(bush, new Vector2(HelperUtils.SafeBoundary.X + 822, HelperUtils.SafeBoundary.Y + 356), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0); 

            // Draw Duck Balls
            m_ducks.DrawBalls(spriteBatch);

            // Draw Intermission
            m_ducks.DrawIntermission(spriteBatch);

            // Draw left ground + grass
            spriteBatch.Draw(ground, new Vector2(HelperUtils.SafeBoundary.X, HelperUtils.SafeBoundary.Y + 436), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            // Draw right ground + grass
            spriteBatch.Draw(ground, new Vector2(HelperUtils.SafeBoundary.X + 512, HelperUtils.SafeBoundary.Y + 436), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            m_clouds_p1.Draw(spriteBatch);
            m_clouds_p2.Draw(spriteBatch);

            // Draw Paddles 
            m_player1.DrawPaddle(spriteBatch);
            m_player2.DrawPaddle(spriteBatch);

            // Draw Intro
            m_intro.Draw(spriteBatch);
            m_ducks.DrawCounter(spriteBatch);

            
            m_player1.DrawItems(spriteBatch);
            m_player2.DrawItems(spriteBatch);


            /* Draw Shot */
            m_flash.Draw(spriteBatch);
            m_duck1_flash.Draw(spriteBatch);
            m_duck2_flash.Draw(spriteBatch);

            #if XBOX // Draw the WideScreen Lines (Top and bottom)
            //Top
            spriteBatch.Draw(ScreenTexture, new Rectangle(0, HelperUtils.SafeBoundary.Top - 63, 1024 + 2 * HelperUtils.SafeBoundary.X, 63), Color.Black);
            //Bottom
            spriteBatch.Draw(ScreenTexture, new Rectangle(0, HelperUtils.SafeBoundary.Bottom,1024 + 2 * HelperUtils.SafeBoundary.X, 63), Color.Black);     
            #endif

            // spriteBatch.Draw(ScreenTexture, m_background_color_rect, Color.PaleGoldenrod);
            
            //spriteBatch.Draw(ScreenTexture, boxrec, Color.Black); //new Color(0, 0, 0, 255));
            /*
           // spriteBatch.Draw(ScreenTexture, startpos, Color.Beige);

            
            spriteBatch.Draw(ScreenTexture, newbound[0], Color.Red);

            spriteBatch.Draw(ScreenTexture, newbound[1], Color.Purple);
            
            spriteBatch.Draw(ScreenTexture, newbound[2], Color.White);
            
            spriteBatch.Draw(ScreenTexture, newbound[3], Color.White);

            spriteBatch.Draw(ScreenTexture, newbound[4], Color.White);
            
            spriteBatch.Draw(ScreenTexture, newbound[5], Color.White);

            spriteBatch.Draw(ScreenTexture, newbound[6], Color.White);

            spriteBatch.Draw(ScreenTexture, newbound[7], Color.White);

            spriteBatch.Draw(ScreenTexture, newbound[8], Color.White);

            spriteBatch.Draw(ScreenTexture, newbound[9], Color.White);
            */ 

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        public void Reset()
        {
            wdog.Reset();
            sdog.Reset();
            sprdog.Reset();
            jdog1.Reset();
            jdog2.Reset();
            jdog3.Reset();

            m_intro.ResetScene();
            m_intro.Scene_State = DrawableState.Active;
            m_endround.Reset();

            m_paused = false;
            m_played_intro = false;
            m_built_ending = false;

            m_clouds_p1 = new Clouds(0, clouds);//, debug);
            m_clouds_p2 = new Clouds(1, clouds);//, debug);
            m_clouds = new Cloud[m_clouds_p1.CloudsA.Length * 2];

            m_clouds[0] = m_clouds_p1.CloudsA[0];
            m_clouds[1] = m_clouds_p1.CloudsA[1];
            m_clouds[2] = m_clouds_p1.CloudsA[2];
            m_clouds[3] = m_clouds_p1.CloudsA[0];
            m_clouds[4] = m_clouds_p1.CloudsA[1];
            m_clouds[5] = m_clouds_p1.CloudsA[2];
            m_clouds[6] = m_clouds_p2.CloudsA[0];
            m_clouds[7] = m_clouds_p2.CloudsA[1];
            m_clouds[8] = m_clouds_p2.CloudsA[2];
            m_clouds[9] = m_clouds_p2.CloudsA[0];
            m_clouds[10] = m_clouds_p2.CloudsA[1];
            m_clouds[11] = m_clouds_p2.CloudsA[2];

            m_ducks.Reset();
            m_player1.LoadContent(m_player_textures, m_num, m_score_fnt, boxrec);
            m_player2.LoadContent(m_player_textures, m_num, m_score_fnt, boxrec);
        }

        #endregion
    }
}
