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

        public override Texture2D Texture
        {
            get { return m_texture; }
            set { Draw_State = DrawableState.Active; m_texture = value; }
        }

        private Texture2D m_texture;

        public override Rectangle Sprite_Src_Rect
        {
            get { return new Rectangle(0, 0, m_texture.Width, m_texture.Height); }// m_src_rect; }
            set { m_src_rect = value; }
        }

        private Rectangle m_src_rect;
       
        public override int Width
        {
            get { return m_texture.Width; }
        }

        public override int Height
        {
            get { return m_texture.Height; }
        }

        #endregion

        #region Initialization

        public Sprite() { }

        public Sprite(Texture2D texture)
        {
            m_texture = texture;
            //Draw_State = DrawableState.Active;
            m_src_rect = new Rectangle(0, 0, m_texture.Width, m_texture.Height);
        }

        public Sprite(int x, int y)
        {
            X_Pos = x;
            Y_Pos = y;
            Draw_State = DrawableState.Active;
        }

        #endregion

        #region Update and Draw

        public override void Update()
        {
            m_src_rect = new Rectangle(0, 0, m_texture.Width, m_texture.Height);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Draw_State != DrawableState.Finished)
            {
                spriteBatch.Draw(m_texture, new Vector2(X_Pos, Y_Pos), null, Tint, Rotation, Origin, Scale, Effects, 0);
            }
        }

        #endregion
    }
}
