using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class Score : ScreenItem
    {
        public Score(int x, int y, Texture2D texture, SpriteFont font)
        {
            Vector2 temp;

            m_font = font;

            m_icon = new Sprite();
            m_icon.Sprite_Texture = texture;
            m_icon.X_Pos = x;
            m_icon.Y_Pos = y;

            m_text_pos = Vector2.Zero;
            m_text_pos.X = m_icon.X_Pos + (m_icon.Width / 2);
            m_text_pos.Y = m_icon.Y_Pos + 20;

            temp = m_font.MeasureString(NumUses.ToString());
            m_text_pos.X -= (temp.X / 2);
        }
    }
}