using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public class SpriteDecorator : Drawable
    {

        public override float X_Pos
        {
            get { return m_drawable_sprite.X_Pos; }
            set { m_drawable_sprite.X_Pos = value; }
        }

        public override float Y_Pos
        {
            get { return m_drawable_sprite.Y_Pos; }
            set { m_drawable_sprite.Y_Pos = value; }
        }

        public override Vector2 Origin
        {
            get { return m_drawable_sprite.Origin; }
            set { m_drawable_sprite.Origin = value; }
        }

        public override float Scale
        {
            get { return m_drawable_sprite.Scale; }
            set { m_drawable_sprite.Scale = value; }
        }

        public override float Rotation
        {
            get { return m_drawable_sprite.Rotation; }
            set { m_drawable_sprite.Rotation = value; }
        }

        public override Color Tint
        {
            get { return m_drawable_sprite.Tint; }
            set { m_drawable_sprite.Tint = value; }
        }

        public override SpriteEffects Effects
        {
            get { return m_drawable_sprite.Effects; }
            set { m_drawable_sprite.Effects = value; }
        }
        /*
        public override DrawableState Draw_State
        {
            get { return m_drawable_sprite.Draw_State; }
            set { m_drawable_sprite.Draw_State = value; }
        }*/

        protected Drawable m_drawable_sprite;

        public SpriteDecorator(Drawable sprite)
        {
            m_drawable_sprite = sprite;
            Draw_State = DrawableState.Active;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Draw_State != DrawableState.Finished)
            {
                m_drawable_sprite.Draw(spriteBatch);
            }
        }
    }
}
