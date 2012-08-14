using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public class AlphaFader : SpriteDecorator
    {
        private int m_fade_to, m_delta;
        private byte m_init_alpha;

        public AlphaFader(Drawable sprite, int fade_to, int delta) : base(sprite)
        {
            m_delta = delta;
            m_fade_to = fade_to;
            m_init_alpha = Tint.A;
        }

        public override void Update() 
        {
            if (Draw_State != DrawableState.Finished)
            {
                int alpha;
                Color color = Tint;
                alpha = color.A;
                alpha += m_delta;
                alpha = (int)MathHelper.Clamp(alpha, 0.0f, 255.0f);
                color.A = (byte)alpha;
                Tint = color;

                if (m_delta < 0.0f)
                {
                    if (color.A <= m_fade_to)
                    {
                        color.A = (byte)m_fade_to;
                        Tint = color;
                        Draw_State = DrawableState.Finished;
                    }
                }
                else
                {
                    if (color.A >= m_fade_to)
                    {
                        color.A = (byte)m_fade_to;
                        Tint = color;
                        Draw_State = DrawableState.Finished;
                    }
                }

                m_drawable_sprite.Update();
            }
        }

        public override void Reset()
        {
            Color color = Tint;
            color.A = m_init_alpha;
            Tint = color;

            Draw_State = DrawableState.Active;
        }
    }
}
