#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
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

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        
        /* * * * * * * * * * * TEXTURES * * * * * * * * * * * */

        Texture2D tree;
        Texture2D ground;
        Texture2D bush;
        Texture2D ScreenTexture; // Debug texture
        
        Texture2D walkingdog;
        Texture2D flyawaysign;
        Texture2D flash;

        Texture2D[] clouds = new Texture2D[3];

        /* * * * * * * * * * * * SOUNDS * * * * * * * * * * * */

        SoundEffect dogbark;
        SoundEffect startround;
        SoundEffect m_shot;

        /* * * * * * * * * * * ANIMATIONS * * * * * * * * * */
        
        AnimatedSprite wdog = new AnimatedSprite();     // Walking Dog Animation (with sound)
        AnimatedSprite sdog = new AnimatedSprite();     // Sniff Dog Animation
        AnimatedSprite sprdog = new AnimatedSprite();   // Suprised Dog Animation

        /* * * * * * * * * ANIMATION SCENE * * * * * * * * */

        AnimationScene m_intro = new AnimationScene();

        // Needed
        Rectangle boxrec = new Rectangle(224, 64, 832, 512);
        Rectangle m_background_color_rect = new Rectangle(128, 64, 1024, 612);

        Sprite m_fly_sign = new Sprite();

        /**************************/
        bool m_paused = false;
        bool m_played_intro = false;

        DuckPopulation m_ducks;

        Rectangle[] newbound;
        CollisionData[] bounddata;

        Player m_player1;
        Player m_player2;

        Texture2D[] m_player_textures = new Texture2D[5];


        Clouds m_clouds;

        Drawable m_flash;
        Drawable m_duck1_flash = new Image();
        Drawable m_duck2_flash = new Image();

        private SpriteFont m_num;
        private SpriteFont m_score_fnt;

        /****************************/

        private KeyboardState p1keyOldState;
        private KeyboardState p2keyOldState;
        private GamePadState p1padOldState;
        private GamePadState p2padOldState;

        float pauseAlpha;

        #endregion
        
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);       
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            /*  LOAD TEXTURES, SOUNDS AND FONTS */

            tree = content.Load<Texture2D>("tree");
            ground = content.Load<Texture2D>("ground");
            bush = content.Load<Texture2D>("bush");
            m_player_textures[0] = content.Load<Texture2D>("panel");            
            walkingdog = content.Load<Texture2D>("walkdog");
            flyawaysign = content.Load<Texture2D>("flyawaysign");
            m_player_textures[1] = content.Load<Texture2D>("score");
            m_player_textures[2] = content.Load<Texture2D>("clouds");
            m_player_textures[3] = content.Load<Texture2D>("duckcall");
            m_player_textures[4] = content.Load<Texture2D>("shot");
            clouds[0] = content.Load<Texture2D>("smallcloud");
            clouds[1] = content.Load<Texture2D>("medcloud");
            clouds[2] = content.Load<Texture2D>("bigcloud");

            m_shot = content.Load<SoundEffect>("sshot");
            startround = content.Load<SoundEffect>("startround");

            m_num = content.Load<SpriteFont>("bitfont");
            m_score_fnt = content.Load<SpriteFont>("bigfont");

            m_clouds = new Clouds(clouds);
            
            /* SETUP ALL ANIMATIONSPRITES */
            wdog.BuildAnimation(walkingdog, 1, 8, true, new int[4] { 1, 2, 3, 4 });
            wdog.SetFrame(0, 8, null);
            wdog.SetFrame(1, 8, null);
            wdog.SetFrame(2, 8, null);
            wdog.SetFrame(3, 8, null);
            wdog.X_Pos = 150;
            wdog.Y_Pos = 535;

            sdog.BuildAnimation(walkingdog, 1, 8, true, new int[6] { 1, 0, 1, 0, 1, 0 });
            sdog.SetFrame(0, 8, null);
            sdog.SetFrame(1, 8, null);
            sdog.SetFrame(2, 8, null);
            sdog.SetFrame(3, 8, null);
            sdog.SetFrame(4, 8, null);
            sdog.SetFrame(5, 8, null);

            sprdog.BuildAnimation(walkingdog, 1, 8, true, new int[1] { 5 });
            sprdog.SetFrame(0, 100, null);

            /*   MAKE INTRO ANIMATION   */
            m_intro.AddAnimation(new DirXYMover(wdog, 180, 0, 1.7f));
            m_intro.AddAnimation(new TimeOutDrawable(sdog, 49, true));
            m_intro.AddAnimation(new DirXYMover(wdog, 180, 0, 1.7f));
            m_intro.AddAnimation(new TimeOutDrawable(sdog, 49, true));
            m_intro.AddAnimation(new DirXYMover(wdog, 10, 0, 1.7f));
            m_intro.AddAnimation(new TimeOutDrawable(sprdog, 49, true));
            m_intro.BuildScene(new int[6] { 0, 1, 2, 3, 4, 5 });
            m_intro.Scene_State = DrawableState.Active;

            /* Make a texture for bg. NOT gonna be needed when I change the dimension of screen */
            ScreenTexture = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            ScreenTexture.SetData(new Color[] { Color.White });

            /* Build the SHOT animations */
            flash = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            flash.SetData(new Color[] { new Color(255, 255, 255, 128) });

            m_flash = new TimeOutDrawable(new Image(128, 64, 1024, 612, flash), 8, false);
            m_flash.Draw_State = DrawableState.Finished;
            
            /* Create Players */
            m_player1 = new Player(0); 
            m_player1.LoadContent(m_player_textures, m_num, m_score_fnt, boxrec);
            m_player2 = new Player(1); 
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
            m_fly_sign.Sprite_Texture = flyawaysign;
            m_fly_sign.X_Pos = ScreenManager.GraphicsDevice.Viewport.Width / 2 - m_fly_sign.Sprite_Texture.Width / 2;
            m_fly_sign.Y_Pos = 320;
            
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
                    m_ducks.UpdateBalls(m_player1.CData, m_player2.CData, m_clouds.CloudsA);
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
                    m_clouds.Update();
                    m_duck1_flash.Update();
                    m_duck1_flash.X_Pos = m_ducks.DuckOneRectangle.X;
                    m_duck1_flash.Y_Pos = m_ducks.DuckOneRectangle.Y;                                      
                    m_duck2_flash.Update();
                    m_duck2_flash.X_Pos = m_ducks.DuckOneRectangle.X;
                    m_duck2_flash.Y_Pos = m_ducks.DuckOneRectangle.Y;
                    m_ducks.UpdateCounter();
                    m_player1.UpdateItems();
                    m_player2.UpdateItems();
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

            if ((input.IsPauseGame(null) || gamePadDisconnected) && m_intro.Scene_State == DrawableState.Finished)// || m_ducks.GameOver())
            {
                m_paused = true;
                ScreenManager.AddScreen(new PauseMenuScreen(), null);
            }
            
             // Handle Cloud Input
             // (Player 1)
            if (((keyboardState1.IsKeyDown(Keys.D) &&                   // Handle Keyboard
                p1keyOldState.IsKeyUp(Keys.D)) ||
                (gamePadState1.Buttons.X == ButtonState.Pressed &&      // Handle Gamepad
                p1padOldState.Buttons.X == ButtonState.Released)) &&
                m_intro.Scene_State == DrawableState.Finished && 
                !m_ducks.Intermission() && 
                m_player1.CloudNum != 0 &&
                m_ducks.BallsAlive())
            {
                m_clouds.Reset();
                // Play duck call sound
                m_player1.CloudNum -= 1;
            }

            // (Player 2)
            if (((keyboardState2.IsKeyDown(Keys.OemComma) &&            // Handle Keyboard
                p2keyOldState.IsKeyUp(Keys.OemComma)) ||
                (gamePadState2.Buttons.X == ButtonState.Pressed &&      // Handle Gamepad
                p2padOldState.Buttons.X == ButtonState.Released)) &&
                m_intro.Scene_State == DrawableState.Finished &&
                !m_ducks.Intermission() &&
                m_player2.CloudNum != 0 &&
                m_ducks.BallsAlive())
            {
                m_clouds.Reset();
                
                m_player2.CloudNum -= 1;
            }
            

            // Handle Duck Calls
            // (Player 1)
            if (((keyboardState1.IsKeyDown(Keys.F) &&                   // Handle Keyboard
                p1keyOldState.IsKeyUp(Keys.F)) ||
                (gamePadState1.Buttons.Y == ButtonState.Pressed &&      // Handle Gamepad
                p1padOldState.Buttons.Y == ButtonState.Released)) &&
                m_intro.Scene_State == DrawableState.Finished && 
                !m_ducks.Intermission() && 
                m_player1.DuckCallNum != 0 &&
                m_ducks.OneBallAlive())
            {
                m_ducks.ReleaseDuck();
                // Play duck call sound
                m_player1.DuckCallNum -= 1;
            }

            // (Player 2)
            if (((keyboardState2.IsKeyDown(Keys.OemPeriod) &&                   // Handle Keyboard
                p2keyOldState.IsKeyUp(Keys.OemPeriod)) ||
                (gamePadState2.Buttons.Y == ButtonState.Pressed &&      // Handle Gamepad
                p2padOldState.Buttons.Y == ButtonState.Released)) &&
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
            // (Player 1)
            if (((keyboardState1.IsKeyDown(Keys.G) &&                   // Handle Keyboard
                p1keyOldState.IsKeyUp(Keys.G)) ||
                (gamePadState1.Buttons.B == ButtonState.Pressed &&      // Handle Gamepad
                p1padOldState.Buttons.B == ButtonState.Released)) &&
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

            // (Player 2)
            if (((keyboardState2.IsKeyDown(Keys.OemQuestion) &&                   // Handle Keyboard
                p2keyOldState.IsKeyUp(Keys.OemQuestion)) ||
                (gamePadState2.Buttons.B == ButtonState.Pressed &&      // Handle Gamepad
                p2padOldState.Buttons.B == ButtonState.Released)) &&
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

            if (m_intro.Scene_State == DrawableState.Finished) // &&
                //m_pongBall.Ball_State != BallState.OutofBounds && )
            {
               // m_paused = false;
                m_player1.UpdatePaddle(keyboardState1, gamePadState1);
                m_player2.UpdatePaddle(keyboardState2, gamePadState2);
            }

            // Save old state
            p1keyOldState = keyboardState1;
            p2keyOldState = keyboardState2;
            p1padOldState = gamePadState1;
            p2padOldState = gamePadState2;
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(Color.Black);//new Color(49, 192, 250, 255));
            /*
            if (m_pongBall.Ball_State == BallState.DeadBall &&
                         !m_finished_intermission &&
                         m_intro.Scene_State == DrawableState.Finished)
                ScreenManager.GraphicsDevice.Clear(Color.White);
            */
            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            if (m_ducks.IntermissionScene == 0)
            {
                spriteBatch.Draw(ScreenTexture, m_background_color_rect, new Color(247, 206, 176, 255));
                m_fly_sign.Draw(spriteBatch);      
            }
            else 
            {
                spriteBatch.Draw(ScreenTexture, m_background_color_rect, new Color(49, 192, 250, 255));
            }

            // Draw tree
            spriteBatch.Draw(tree, new Vector2(250, 300), null, Color.White, 0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);
            
            // Draw Bush
            spriteBatch.Draw(bush, new Vector2(950, 420), null, Color.White, 0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);

            // Draw Duck Balls
            m_ducks.DrawBalls(spriteBatch);

            // Draw Intermission
            m_ducks.DrawIntermission(spriteBatch);

            // Draw left ground + grass
            spriteBatch.Draw(ground, new Vector2(128, 500), null, Color.White, 0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);

            // Draw right ground + grass
            spriteBatch.Draw(ground, new Vector2(640, 500), null, Color.White, 0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);

            // Draw Paddles 
            m_player1.DrawPaddle(spriteBatch);
            m_player2.DrawPaddle(spriteBatch);

            // Draw Intro
            m_intro.Draw(spriteBatch);

            m_ducks.DrawCounter(spriteBatch);

            m_clouds.Draw(spriteBatch);

            m_player1.DrawItems(spriteBatch);
            m_player2.DrawItems(spriteBatch);


            /* Draw Shot */
            m_flash.Draw(spriteBatch);
            m_duck1_flash.Draw(spriteBatch);
            m_duck2_flash.Draw(spriteBatch);

            // spriteBatch.Draw(ScreenTexture, m_background_color_rect, Color.PaleGoldenrod);
            /*
            spriteBatch.Draw(ScreenTexture, boxrec, Color.Black); //new Color(0, 0, 0, 255));

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
             * */

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
