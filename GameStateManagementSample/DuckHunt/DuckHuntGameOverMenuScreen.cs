#region File Description
#endregion

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
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class DuckHuntGameOverMenuScreen : MenuScreen
    {
        #region Fields

        ContentManager m_content;

        SpriteFont m_menu_fnt;
        Texture2D m_duck_txt;

        // Create our menu entries.
        MenuFontItem m_playagain;
        MenuFontItem m_quit;
        MenuFontItem m_selected;

        SoundEffect m_selector_snd;
        AnimatedSprite m_selector;

        DuckHuntGameplayScreen m_screen;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public DuckHuntGameOverMenuScreen(DuckHuntGameplayScreen screen)
            : base("GameOver")
        {
            m_screen = screen;   
        }


        #endregion

        #region Handle Input

        void PlayAgain(object sender, PlayerIndexEventArgs e)
        {
            m_screen.Reset();
            ExitScreen();
        }


        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGame(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null,
                               new BackgroundScreen(), new MainMenuScreen());
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

            m_duck_txt = m_content.Load<Texture2D>("DuckHunt\\Textures\\duck");
            m_menu_fnt = m_content.Load<SpriteFont>("Fonts\\bigfont");

            m_selector_snd = m_content.Load<SoundEffect>("DuckHunt\\Sounds\\select");

            m_playagain = new MenuFontItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - (int)m_menu_fnt.MeasureString("PLAY AGAIN").X / 2, 150, "PLAY AGAIN",m_menu_fnt);
            m_quit = new MenuFontItem(ScreenManager.GraphicsDevice.Viewport.Width / 2 - (int)m_menu_fnt.MeasureString("QUIT").X / 2, 155 + m_playagain.Height, "QUIT",m_menu_fnt);

            m_selector = new AnimatedSprite();
            m_selector.BuildAnimation(m_duck_txt, 1, 9, true, new int[4] { 3, 4, 5, 4 });
            m_selector.SetFrame(0, 9, null);
            m_selector.SetFrame(1, 9, null);
            m_selector.SetFrame(2, 9, null);
            m_selector.SetFrame(3, 9, null);
            m_selector.X_Pos = m_playagain.X_Pos - (int)(m_selector.Width * 1.5);
            m_selector.Y_Pos = m_playagain.Y_Pos + m_playagain.Height / 2 - m_selector.Height / 2;

            // Hook up menu event handlers.
            m_playagain.m_selected_events += PlayAgain;
            m_quit.m_selected_events += QuitGame;

            // Add entries to the menu.
            MenuEntries.Add(m_playagain);
            MenuEntries.Add(m_quit);

            m_selected = m_playagain;
        }
        
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            m_playagain.Tint *= TransitionAlpha;
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
