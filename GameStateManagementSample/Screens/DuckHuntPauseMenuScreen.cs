#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Audio;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class DuckHuntPauseMenuScreen : MenuScreen
    {
        #region Fields

        ContentManager m_content;

        Texture2D m_quit_txt;
        Texture2D m_resume_txt;
        Texture2D m_duck_txt;

        // Create our menu entries.
        MenuSpriteItem m_resume;
        MenuSpriteItem m_quit;
        MenuSpriteItem m_selected;

        SoundEffect m_selector_snd;
        AnimatedSprite m_selector;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public DuckHuntPauseMenuScreen()
            : base("Paused")
        {
            //TransitionOnTime = TimeSpan.FromSeconds(1.5);
            //TransitionOffTime = TimeSpan.FromSeconds(0.5);  
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (m_selected != SelectedItem)
            {
                m_selector_snd.Play();
                m_selected = (MenuSpriteItem)SelectedItem;
            }
            m_selector.X_Pos = SelectedItem.X_Pos - (int)(m_selector.Width * 1.5);
            m_selector.Y_Pos = SelectedItem.Y_Pos + SelectedItem.Height / 2 - m_selector.Height/2;
            m_selector.Update();
        }


        #endregion

        #region Public Methods

        public override void LoadContent()
        {
            if (m_content == null)
                m_content = new ContentManager(ScreenManager.Game.Services, "Content");

            m_duck_txt = m_content.Load<Texture2D>("DuckHunt\\Textures\\duck");
            m_resume_txt = m_content.Load<Texture2D>("DuckHunt\\Textures\\resume");
            m_quit_txt = m_content.Load<Texture2D>("DuckHunt\\Textures\\quit");

            m_selector_snd = m_content.Load<SoundEffect>("DuckHunt\\Sounds\\select");

            m_resume = new MenuSpriteItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - m_resume_txt.Width/2, 150, m_resume_txt);
            m_quit = new MenuSpriteItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - m_quit_txt.Width/2, 155+m_resume.Height, m_quit_txt);

            m_selector = new AnimatedSprite();
            m_selector.BuildAnimation(m_duck_txt, 1, 9, true, new int[4] { 3, 4, 5, 4 });
            m_selector.SetFrame(0, 9, null);
            m_selector.SetFrame(1, 9, null);
            m_selector.SetFrame(2, 9, null);
            m_selector.SetFrame(3, 9, null);
            m_selector.X_Pos = m_resume.X_Pos - (int)(m_selector.Width*1.5);
            m_selector.Y_Pos = m_resume.Y_Pos + m_resume_txt.Height / 2 - m_selector.Height/2;

            // Hook up menu event handlers.
            m_resume.m_selected_events += OnCancel;
            m_quit.m_selected_events += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(m_resume);
            MenuEntries.Add(m_quit);

            m_selected = m_resume;
        }
        
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            m_resume.Tint *= TransitionAlpha;
            m_quit.Tint *= TransitionAlpha;

            Color color = Color.White * TransitionAlpha;
            m_selector.Tint = color;

            base.Draw(gameTime);

            spriteBatch.Begin();

            m_selector.Draw(spriteBatch);

            spriteBatch.End();
        }

        #endregion
    }
}
