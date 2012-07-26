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

        public Blinker(Drawable sprite, int frames) : base(sprite)
        {
            m_frames = frames;
        }

        public override void Update()
        {
            if (Draw_State != DrawableState.Finished)
            {
                Color color;
                ++m_frame_cnt;

                if (m_frame_cnt >= m_frames)
                {
                    color = Tint;
                    if (color.A == 0)
                    {
                        color.A = 255;
                        Tint = color;
                    }
                    else
                    {
                        color.A = 0;
                        Tint = color;
                    }
                    m_frame_cnt = 0;
                }
            }

            m_drawable_sprite.Update();
        }
    }
}
