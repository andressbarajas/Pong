using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public class LinearXYMover : SpriteDecorator
    {
        private Vector2 m_vel;
        private Vector2 m_dst_pos;
        private Vector2 m_init_pos;
        private Vector2 m_sprite_pos;

        private float m_vel_mag;

        public LinearXYMover(Drawable sprite, int dst_x, int dst_y, float vel_mag)
            : base(sprite)
        {
            m_dst_pos = new Vector2(dst_x,dst_y);
            m_init_pos = new Vector2(X_Pos, Y_Pos);

            m_vel_mag = vel_mag;         
        }

        public override void Update()
        {
            if (Draw_State != DrawableState.Finished)
            {
                if (Vector2.Distance(m_dst_pos, m_sprite_pos) < m_vel_mag)
                {
                    X_Pos = m_dst_pos.X;
                    Y_Pos = m_dst_pos.Y;
                    Draw_State = DrawableState.Finished;
                }
                else
                {
                    SetVelocity();
                    X_Pos = m_sprite_pos.X;
                    Y_Pos = m_sprite_pos.Y;
                }
            }

            m_drawable_sprite.Update();
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            m_drawable_sprite.Draw(spriteBatch);
        }

        public override void Reset()
        {
            X_Pos = m_init_pos.X;
            Y_Pos = m_init_pos.Y;
            Draw_State = DrawableState.Active;
        }

        private void SetVelocity()
        {
            m_sprite_pos = new Vector2(X_Pos, Y_Pos);

            m_vel = Vector2.Subtract(m_dst_pos, m_sprite_pos);
            m_vel.Normalize();
            m_vel = Vector2.Multiply(m_vel, m_vel_mag);
            m_sprite_pos = Vector2.Add(m_sprite_pos, m_vel);  
        }
    }
}
