using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class MenuFontItem : MenuItem
    {
        #region Properties

        public SpriteFont Sprite_Font
        {
            get { return m_font; }
            set { Draw_State = DrawableState.Active; m_font = value; }
        }

        private SpriteFont m_font;

        public string Text 
        {
            set { m_text = value; }
        }

        private string m_text = "";

        public override int Width
        {
            get { return (int)m_font.MeasureString(m_text).X; }
        }

        public override int Height
        {
            get { return (int)m_font.MeasureString(m_text).Y; }
        }

        #endregion

        #region Fields

        #endregion

        #region Initialization

        public MenuFontItem() { }

        public MenuFontItem(int x, int y, string text, SpriteFont font)
        {
            X_Pos = x;
            Y_Pos = y;
            m_text = text;
            m_font = font;
            Draw_State = DrawableState.Active;
        }

        #endregion

        #region Update and Draw

        public override void Update()
        {
            if (Selected)
            {
                Tint = Selected_Tint;
            }
            else
            {
                Tint = Color.White;
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(m_font, m_text, new Vector2(X_Pos, Y_Pos), Tint, Rotation, Origin, Scale, Effects, 0);
        }

        #endregion
    }
}