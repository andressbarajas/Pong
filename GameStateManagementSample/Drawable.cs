using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public enum DrawableState
    {
        Active,
        Finished,
    }

    public abstract class Drawable
    {
        #region Properties

        public virtual float X_Pos
        {
            get { return m_xpos; }
            set { m_xpos = value; }
        }

        private float m_xpos;

        public virtual float Y_Pos
        {
            get { return m_ypos; }
            set { m_ypos = value; }
        }

        private float m_ypos;

        public virtual Vector2 Origin
        {
            get { return m_origin; }
            set { m_origin = value; }
        }

        private Vector2 m_origin = Vector2.Zero;

        public virtual float Scale
        {
            get { return m_scale; }
            set { m_scale = value; }
        }

        private float m_scale = 1.0f;

        public virtual float Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        private float m_rotation = 0.0f;

        public virtual Color Tint
        {
            get { return m_tint; }
            set { m_tint = value; }
        }

        private Color m_tint = Color.White;

        public virtual float Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }

        private float m_layer = 1.0f;

        public virtual SpriteEffects Effects
        {
            get { return m_effects; }
            set { m_effects = value; }
        }

        private SpriteEffects m_effects = SpriteEffects.None;

        public virtual DrawableState Draw_State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        private DrawableState m_state = DrawableState.Finished;
 
        #endregion

        public virtual void Update() { }
        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual void Reset() { m_state = DrawableState.Active; m_effects = SpriteEffects.None; }
        
    }
}
