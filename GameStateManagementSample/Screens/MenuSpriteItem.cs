using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public class MenuSpriteItem : MenuItem
    {
        #region Properties

        public override Texture2D Texture
        {
            get { return m_texture; }
            set { Draw_State = DrawableState.Active; m_texture = value; }
        }

        private Texture2D m_texture;

        public override int Width
        {
            get { return m_texture.Width; }
        }

        public override int Height
        {
            get { return m_texture.Height; }
        }

        #endregion

        #region Fields

        #endregion

        #region Initialization

        public MenuSpriteItem() { }

        public MenuSpriteItem(int x, int y, Texture2D texture) 
        {
            X_Pos = x;
            Y_Pos = y;
            m_texture = texture;
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
            spritebatch.Draw(m_texture, new Vector2(X_Pos, Y_Pos), null, Tint, Rotation, Origin, Scale, Effects, 0);
        }

        #endregion
       
    }
}
