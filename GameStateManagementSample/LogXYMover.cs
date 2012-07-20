using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class LogXYMover : SpriteDecorator
    {
        private float m_x_dst, m_y_dst;
        private float m_init_x, m_init_y;

        public LogXYMover(Drawable sprite, int dst_x, int dst_y) : base(sprite)
        {
            m_x_dst = dst_x;
            m_y_dst = dst_y;
            m_init_x = X_Pos;
            m_init_y = Y_Pos;
        }

        public override void Update()
        {
            if (Draw_State != DrawableState.Finished)
            {
                if (((X_Pos - m_x_dst) <= 0) && ((Y_Pos - m_y_dst) <= 0))
                {
                    X_Pos = m_x_dst;
                    Y_Pos = m_y_dst;
                    Draw_State = DrawableState.Finished;
                }
                else
                {
                    // Move 1/8 of the distance each frame
                    float dx = m_x_dst - X_Pos;
                    float dy = m_y_dst - Y_Pos;
                    X_Pos = (int)(X_Pos + (dx / 8.0f));
                    Y_Pos = (int)(Y_Pos + (dy / 8.0f));
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
            X_Pos = m_init_x;
            Y_Pos = m_init_y;
            Draw_State = DrawableState.Active;
        }
    }
}
