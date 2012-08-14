using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public class DirXYMover : SpriteDecorator
    {
        private int m_mov_x, m_mov_y;
        private bool m_set_dst;
        private Vector2 m_vel;
        private Vector2 m_dst_pos;
        private Vector2 m_init_pos;
        private Vector2 m_sprite_pos;

        private float m_vel_mag;

        public DirXYMover(Drawable sprite, int mov_x, int mov_y, float speed) : base(sprite)
        {
            m_init_pos.X = sprite.X_Pos;
            m_init_pos.Y = sprite.Y_Pos;

            m_mov_x = mov_x;
            m_mov_y = mov_y;

            m_set_dst = false;

            m_vel_mag = speed;
        }

        public override void Update()
        {
            if (Draw_State != DrawableState.Finished)
            {
                if (m_set_dst && (Vector2.Distance(m_dst_pos, m_sprite_pos) <= m_vel_mag))
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
            m_set_dst = false;
            Draw_State = DrawableState.Active;
        }

        private void SetVelocity()
        {
            if (!m_set_dst)
            {
                if (Effects == SpriteEffects.None)
                {
                    m_dst_pos.X = X_Pos + m_mov_x;
                    m_dst_pos.Y = Y_Pos + m_mov_y;
                }
                else
                {
                    m_dst_pos.X = X_Pos - m_mov_x;
                    m_dst_pos.Y = Y_Pos - m_mov_y;
                }

                m_set_dst = true;
            }


            m_sprite_pos = new Vector2(X_Pos, Y_Pos);

            m_vel = Vector2.Subtract(m_dst_pos, m_sprite_pos);
            m_vel.Normalize();
            m_vel = Vector2.Multiply(m_vel, m_vel_mag);
            m_sprite_pos = Vector2.Add(m_sprite_pos, m_vel);
        }
    }
}
