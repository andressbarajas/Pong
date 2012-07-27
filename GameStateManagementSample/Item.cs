using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public abstract class ScreenItem
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

        public SpriteEffects Effects
        {
            get { return m_icon.Effects; }
            set { m_icon.Effects = value; }
        }

        #endregion

        #region Fields

        public Sprite m_icon;
        protected SpriteFont m_font;
        protected Vector2 m_text_pos;

        #endregion

        #region Update and Draw

        public virtual void Update() {
            Vector2 temp = Vector2.Zero;

            m_text_pos.X = m_icon.X_Pos + (m_icon.Width / 2);

            temp = m_font.MeasureString(m_num.ToString());
            m_text_pos.X -= (temp.X / 2);

            //m_icon.Tint = m_tint;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            m_icon.Draw(spriteBatch);
            spriteBatch.DrawString(m_font, m_num.ToString(), m_text_pos, m_tint);
        }

        #endregion
    }
}
