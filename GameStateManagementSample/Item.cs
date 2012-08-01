using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class ScreenItem
    {
        #region Properties

        public int NumUses
        {
            get { return m_num; }
            set { m_num = value; }
        }

        private int m_num = 0;

        public Color Tint
        {
            get { return m_tint; }
            set { m_tint = value; }
        }

        private Color m_tint = Color.White;

        public Color Font_Tint
        {
            get { return m_font_tint; }
            set { m_font_tint = value; }
        }

        private Color m_font_tint = Color.White;

        public SpriteEffects Effects
        {
            get { return m_icon.Effects; }
            set { m_icon.Effects = value; }
        }

        public bool Alpha
        {
            set { m_alpha = value; }
        }

        private bool m_alpha = true;
        #endregion

        #region Fields

        public Sprite m_icon;
        protected SpriteFont m_font;
        protected Vector2 m_text_pos;

        #endregion

        #region Initialization

        public ScreenItem(int x, int y, int fx, int fy, Texture2D texture, SpriteFont font)
        {
            Vector2 temp;

            m_font = font;

            m_icon = new Sprite();
            m_icon.Sprite_Texture = texture;
            m_icon.X_Pos = x;
            m_icon.Y_Pos = y;

            m_text_pos = Vector2.Zero;
            m_text_pos.X = fx; // m_icon.X_Pos + (m_icon.Width / 2);
            m_text_pos.Y = fy; //    m_icon.Y_Pos + 10;

            temp = m_font.MeasureString(NumUses.ToString());
            m_text_pos.X -= (temp.X / 2);
        }

        #endregion

        #region Update and Draw

        public virtual void Update() {
            Vector2 temp = Vector2.Zero;

            m_text_pos.X = m_icon.X_Pos + (m_icon.Width / 2);

            temp = m_font.MeasureString(m_num.ToString());
            m_text_pos.X -= (temp.X / 2);

            m_font_tint.A = m_tint.A;

            if (NumUses == 0 && m_alpha)
            {
                Tint = new Color(255, 255, 255, 128);
                m_icon.Tint = Tint;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {          
            m_icon.Draw(spriteBatch);
            spriteBatch.DrawString(m_font, m_num.ToString(), m_text_pos, m_font_tint, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }

        #endregion
    }
}
