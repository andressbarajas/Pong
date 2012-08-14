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

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class PongGameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;

        /* * * * * * * * * * * TEXTURES * * * * * * * * * * * */

        Texture2D ScreenTexture; // Debug texture
        Texture2D paddle;
        Texture2D ball;

        /* * * * * * * * * * * * SOUNDS * * * * * * * * * * * */

        SoundEffect pause;
        SoundEffect wallhit;
        SoundEffect paddlehit;
        SoundEffect balldie;

        // Needed
        Rectangle boxrec = new Rectangle(150, 0, 724, 612);

        /**************************/
        bool m_paused = false;
        int m_player_1_score = 0;
        int m_player_2_score = 0;


        Paddle m_player1;
        Paddle m_player2;
        PongBall m_ball;

        private SpriteFont m_score_fnt;

        /****************************/

        float pauseAlpha;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public PongGameplayScreen()
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
            ScreenTexture = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            ScreenTexture.SetData(new Color[] { Color.White });
            paddle = content.Load<Texture2D>("Pong\\Textures\\pongpaddle");
            ball = content.Load<Texture2D>("Pong\\Textures\\pongball");
 
            pause = content.Load<SoundEffect>("DuckHunt\\Sounds\\pause");
            paddlehit = content.Load<SoundEffect>("Pong\\Sounds\\paddlehit");
            wallhit = content.Load<SoundEffect>("Pong\\Sounds\\wallbounce");
            balldie = content.Load<SoundEffect>("Pong\\Sounds\\deadball");

            m_score_fnt = content.Load<SpriteFont>("Fonts\\hugenesfont");

              /* Create Players */
            m_player1 = new Paddle(0, paddle, boxrec);       
            m_player2 = new Paddle(1, paddle, boxrec);

            CollisionData[] temp = new CollisionData[4];

            temp[0] = new CollisionData(new Rectangle(0,-81, 1024, 80));
            temp[1] = new CollisionData(new Rectangle(0, 613, 1024, 80));
            temp[2] = new CollisionData(new Rectangle(-81, 0, 80, 612));
            temp[3] = new CollisionData(new Rectangle(1024, 0, 80, 612));

              /* Create Ball */
            m_ball = new PongBall(temp , paddlehit, wallhit);            
            m_ball.AddAnimation(new Sprite(ball));
            m_ball.SetCurrentAnimation(0);
            m_ball.X_Pos = 512;
            m_ball.Y_Pos = (int)HelperUtils.GetRandomNumber(0.0, 608.0f);
          
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
               
                /* Update Ball */
                if(m_ball.Ball_State == BallState.Active) {
                    m_ball.Update(m_player1.Texture_Data, m_player2.Texture_Data);
                }

                if (m_ball.Ball_State == BallState.DeadBall)
                {
                    balldie.Play();
                    if (m_ball.Ball_Dir == Direction.Right)
                    {
                        m_ball.Init_x = m_ball.X_Pos;
                        m_ball.Init_y = m_ball.Y_Pos;
                        m_ball.ResetBall(50.0f, -50.0f);
                        m_player_1_score += 1;
                    }
                    else if (m_ball.Ball_Dir == Direction.Left)
                    {
                        m_ball.Init_x = m_ball.X_Pos;
                        m_ball.Init_y = m_ball.Y_Pos;
                        m_ball.ResetBall(140.0f, 210.0f);
                        m_player_2_score += 1;
                    }
                    m_ball.X_Pos = 512;
                    m_ball.Y_Pos = (int)HelperUtils.GetRandomNumber(0.0, 608.0f);
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

            //PlayerIndex playerIndex;

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

            if ((input.IsPauseGame(null) || gamePadDisconnected))
            {
                pause.Play();
                m_paused = true;
                ScreenManager.AddScreen(new PongPauseMenuScreen(this), null);
            }
            else
            {
                m_player1.Update(keyboardState1, gamePadState1);
                m_player2.Update(keyboardState2, gamePadState2);
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Draw Duck Balls
            m_ball.Draw(spriteBatch);

            // Draw Paddles 
            m_player1.Draw(spriteBatch);
            m_player2.Draw(spriteBatch);

            // Draw Scores
            spriteBatch.DrawString(m_score_fnt, m_player_1_score.ToString(), new Vector2(332 - m_score_fnt.MeasureString(m_player_1_score.ToString()).X, 100), Color.White);
            spriteBatch.DrawString(m_score_fnt, m_player_2_score.ToString(), new Vector2(844 -m_score_fnt.MeasureString(m_player_2_score.ToString()).X, 100), Color.White);

            int y = 3;

            for (int i = 0; i < 31; i++)
            {
                spriteBatch.Draw(ScreenTexture, new Rectangle(511, y, 3, 10), Color.White);
                y += 20;
            }

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
            m_paused = false;
            m_player_1_score = 0;
            m_player_2_score = 0;

            if (HelperUtils.GetRandomNumber(0.0, 10.0) < 5.01)
            {
                m_ball.ResetBall(50.0f, -50.0f);
            }
            else
            {
                m_ball.ResetBall(140.0f, 210.0f);
            }
            m_ball.X_Pos = 512;
            m_ball.Y_Pos = (int)HelperUtils.GetRandomNumber(0.0, 608.0);

            m_player1.InitPaddle();
            m_player2.InitPaddle();
        }

        #endregion
    }
}

