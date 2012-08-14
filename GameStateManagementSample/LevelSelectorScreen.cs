#region File Description
//-----------------------------------------------------------------------------
// MessageBoxScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace GameStateManagement
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class LevelSelectorScreen : GameScreen
    {
        #region Fields
         
        Texture2D m_pln_txt;
  
        Texture2D m_pong_lvl;
        Texture2D m_duck_hunt_lvl;

        AudioEngine m_engine;
        SoundBank m_soundBank;
        WaveBank m_waveBank;

        Sprite m_pong;
        Sprite m_duckh;

        GridMenu m_grid;
 
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public LevelSelectorScreen() 
        { 
            IsPopup = true;

            m_pong = new Sprite();
            m_duckh = new Sprite();

            m_engine = new AudioEngine("Content\\LevelSelection\\LevelSnds.xgs");
            m_soundBank = new SoundBank(m_engine, "Content\\LevelSelection\\Sound Bank.xsb");
            m_waveBank = new WaveBank(m_engine, "Content\\LevelSelection\\Wave Bank.xwb");
  
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);
        }

        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = new ContentManager(ScreenManager.Game.Services, "Content");

            m_pln_txt = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            m_pln_txt.SetData(new Color[] { Color.White });

            m_pong_lvl = content.Load<Texture2D>("LevelSelection\\pong");
            m_duck_hunt_lvl = content.Load<Texture2D>("LevelSelection\\duckhuntpong");

            m_pong.Texture = m_pong_lvl;
            m_duckh.Texture = m_duck_hunt_lvl;

            m_grid = new GridMenu(66, 40, 4, m_pln_txt, m_soundBank);
            m_grid.Spacing = 30;
            m_grid.SelectedTint = Color.Blue;
            m_grid.AddItem(m_pong, new Selected(PongSelected), "duckhunt");
            m_grid.AddItem(m_duckh, new Selected(DuckHuntSelected), "duckhunt");
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        internal void DuckHuntSelected()
        {        
            LoadingScreen.Load(ScreenManager, true, null,
                               new DuckHuntGameplayScreen());
        }

        internal void PongSelected()
        {
             LoadingScreen.Load(ScreenManager, true, null,
                               new PongGameplayScreen());     
        }

        /// <summary>
        /// Responds to user input, accepting or cancelling the Level selector.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            m_grid.HandleInput(input);

            if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                ExitScreen();
            }
        }


        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            m_grid.Update();
            m_engine.Update();
        }


        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();
            spriteBatch.Draw(m_pln_txt, HelperUtils.BuildRect(new Rectangle(0, 0, 1024, 612), -0.03f), new Color(0, 0, 0, 128));
            m_grid.Draw(spriteBatch);
            spriteBatch.End();
        }

        #endregion
    }
}
