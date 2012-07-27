using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class Sprite : Drawable
    {
        #region Properties

        public Texture2D Sprite_Texture
        {
            get { return m_texture; }
            set { Draw_State = DrawableState.Active; m_texture = value; }
        }

        private Texture2D m_texture;

        public Rectangle Sprite_Src_Rect
        {
            get { return m_src_rect; }
            set { m_src_rect = value; }
        }

        private Rectangle m_src_rect;

        public Rectangle Sprite_Rect
        {
            get { return m_rect; }
        }

        private Rectangle m_rect = new Rectangle();

        public int Width
        {
            get { return m_texture.Width; }
        }

        public int Height
        {
            get { return m_texture.Height; }
        }

        #endregion

        #region Initialization

        public Sprite() { }

        public Sprite(int x, int y)
        {
            X_Pos = x;
            Y_Pos = y;
        }

        #endregion

        #region Update and Draw

        public override void Update() 
        {
            m_rect.X = (int)X_Pos;
            m_rect.Y = (int)Y_Pos;

            if (m_src_rect != null)
            {
                m_rect.Width = m_src_rect.Width;
                m_rect.Height = m_src_rect.Height;
            }
            else 
            {
                m_rect.Width = m_texture.Width;
                m_rect.Height = m_texture.Height;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Draw_State != DrawableState.Finished)
            {
                //spriteBatch.Draw(m_texture, new Rectangle((int)X_Pos, (int)Y_Pos, m_texture.Width, m_texture.Height), Tint);
                spriteBatch.Draw(m_texture, new Vector2(X_Pos,Y_Pos), null, Tint, Rotation, Origin, Scale, Effects, 0);
            }
        }

        #endregion
    }
}
