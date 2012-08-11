

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
    class PongPauseMenuScreen : MenuScreen
    {
        #region Fields

        ContentManager m_content;

        Texture2D m_resume_txt;
        Texture2D m_restart_txt;
        Texture2D m_quit_txt;
        Texture2D m_selector_txt;

        MenuSpriteItem m_resume;
        MenuSpriteItem m_restart;
        MenuSpriteItem m_quit;
        MenuSpriteItem m_selected;

        SoundEffect m_selector_snd;
        Sprite m_selector;

        PongGameplayScreen m_screen;

        #endregion

        #region Initialization

        public PongPauseMenuScreen(PongGameplayScreen screen) : base("Paused") { 
            m_screen = screen;
        }

        #endregion

        #region Handle Input

        void RestartGameMenuEntrySelected(object sender, PlayerIndexEventArgs e) 
        {
            m_screen.Reset();
            ExitScreen();
        }

        // <summary>
        // Event handler for when the Quit Game menu entry is selected.
        // </summary>
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


            m_resume_txt = m_content.Load<Texture2D>("DuckHunt\\Textures\\resume");//("Pong\\Textures\\resume");
            m_restart_txt = m_content.Load<Texture2D>("DuckHunt\\Textures\\playagain");//("Pong\\Textures\\restart");
            m_quit_txt = m_content.Load<Texture2D>("DuckHunt\\Textures\\quit");//("Pong\\Textures\\quit");
            m_selector_txt = m_content.Load<Texture2D>("DuckHunt\\Textures\\smallcloud");//("Pong\\Textures\\selector");

            m_selector_snd = m_content.Load<SoundEffect>("DuckHunt\\Sounds\\select");//("Pong\\Sounds\\select");

            m_resume = new MenuSpriteItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - m_resume_txt.Width/2, 150, m_resume_txt);
            m_restart = new MenuSpriteItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - m_restart_txt.Width/2, 155+m_resume.Height, m_restart_txt);
            m_quit = new MenuSpriteItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - m_quit_txt.Width/2, 155+m_resume.Height+m_restart.Height, m_quit_txt);

            m_selector = new Sprite(m_selector_txt);
            m_selector.X_Pos = m_resume.X_Pos - (int)(m_selector.Width*1.5);
            m_selector.Y_Pos = m_resume.Y_Pos + m_resume_txt.Height / 2 - m_selector.Height/2;

            // Hook up menu event handlers.
            m_resume.m_selected_events += OnCancel;
            m_restart.m_selected_events += RestartGameMenuEntrySelected;
            m_quit.m_selected_events += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(m_resume);
            MenuEntries.Add(m_restart);
            MenuEntries.Add(m_quit);

            m_selected = m_resume;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            m_resume.Tint *= TransitionAlpha;
            m_restart.Tint *= TransitionAlpha;
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
