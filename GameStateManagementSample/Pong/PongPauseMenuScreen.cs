

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Audio;
#endregion


namespace PongaThemes
{
    class PongPauseMenuScreen : MenuScreen
    {
        #region Fields

        ContentManager m_content;

        SpriteFont m_menu_fnt;
        Texture2D m_selector_txt;

        MenuFontItem m_resume;
        MenuFontItem m_restart;
        MenuFontItem m_quit;
        MenuFontItem m_selected;

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
                m_selected = (MenuFontItem)SelectedItem;
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

            m_menu_fnt = m_content.Load<SpriteFont>("Fonts\\atarifont");
            
            m_selector_txt = m_content.Load<Texture2D>("Pong\\Textures\\selector");

            m_selector_snd = m_content.Load<SoundEffect>("Pong\\Sounds\\paddlehit");

            m_resume = new MenuFontItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - (int)m_menu_fnt.MeasureString("RESUME").X/2, 150, "RESUME", m_menu_fnt);
            m_restart = new MenuFontItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - (int)m_menu_fnt.MeasureString("RESTART").X / 2, 155 + m_resume.Height, "RESTART", m_menu_fnt);
            m_quit = new MenuFontItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - (int)m_menu_fnt.MeasureString("QUIT").X / 2, 155 + m_resume.Height + m_restart.Height, "QUIT", m_menu_fnt);

            m_selector = new Sprite(m_selector_txt);
            m_selector.X_Pos = m_resume.X_Pos - (int)(m_selector.Width*1.5);
            m_selector.Y_Pos = m_resume.Y_Pos + m_resume.Height / 2 - m_selector.Height/2;

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
