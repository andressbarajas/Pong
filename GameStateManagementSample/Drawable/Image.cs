using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public class Image : Drawable
    {
        #region Properties

        public override Texture2D Texture
        {
            get { return m_texture; }
        }

        private Texture2D m_texture;

        public override Rectangle? Sprite_Src_Rect
        {
            get { if (m_src_rect.HasValue) { return m_src_rect.Value; } else { return null; } } 
            set { m_src_rect = value; }
        }

        private Rectangle? m_src_rect = null;

        #endregion

        #region Fields

        private int m_width = 0;
        private int m_height = 0;

        #endregion

        #region Initialization

        public Image() { }

        public Image(int x, int y, int width, int height, Texture2D texture)
        {
            X_Pos = x;
            Y_Pos = y;

            m_width = width;
            m_height = height;

            m_texture = texture;

            Draw_State = DrawableState.Active;
        }

        #endregion

        #region Update and Draw
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Draw_State != DrawableState.Finished)
            {
                spriteBatch.Draw(m_texture, new Rectangle((int)X_Pos, (int)Y_Pos, m_width, m_height), m_src_rect, Tint, Rotation, Origin, Effects, 0);
                //spriteBatch.Draw(m_texture, new Vector2(X_Pos, Y_Pos), m_src_rect, Tint, Rotation, Origin, Scale, Effects, 0);
            }
        }

        #endregion
    }
}