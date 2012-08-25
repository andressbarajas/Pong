#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace PongaThemes
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Fields

        ContentManager m_content;

        SpriteFont m_fnt;

        // Create our menu entries.
        MenuFontItem m_play;
        MenuFontItem m_exit;

        #endregion

        #region Initialization
        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LevelSelectorScreen(), null);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
        }

        #endregion

        #region Public Methods

        public override void LoadContent()
        {
            if (m_content == null)
                m_content = new ContentManager(ScreenManager.Game.Services, "Content");

            m_fnt = m_content.Load<SpriteFont>("Fonts\\nesfont");

            m_play = new MenuFontItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - (int)m_fnt.MeasureString("Play").X/2, 150, "Play", m_fnt);
            m_play.Selected_Tint = Color.Yellow;
            m_exit = new MenuFontItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - (int)m_fnt.MeasureString("Play").X/2, 155 + (int)m_fnt.MeasureString("Exit").Y, "Exit", m_fnt);
            m_exit.Selected_Tint = Color.Yellow;

            // Hook up menu event handlers.
            m_play.m_selected_events += PlayGameMenuEntrySelected;
            m_exit.m_selected_events += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(m_play);
            MenuEntries.Add(m_exit);
        }
        #endregion
    }
}
