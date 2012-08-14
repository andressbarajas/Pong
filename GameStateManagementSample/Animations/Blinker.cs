using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class Blinker : SpriteDecorator
    {
        private int m_frame_cnt = 0;
        private int m_frames;
        private Color m_init_color;
        private Color m_color;

        public Blinker(Drawable sprite, Color color, int frames) : base(sprite)
        {
            m_color = color;
            m_frames = frames;
            m_init_color = sprite.Tint;
        }

        public override void Update()
        {
            Color color;

            if (Draw_State != DrawableState.Finished)
            {
                ++m_frame_cnt;

                if (m_frame_cnt >= m_frames)
                {
                    color = Tint;
                    if (color == m_color) 
                    {
                        Tint = m_init_color;
                    }
                    else
                    {
                        Tint = m_color;
                    }
                    m_frame_cnt = 0;
                }
            }
            else
            {
                Tint = m_init_color;
            }

            m_drawable_sprite.Update();
        }
    }
}
