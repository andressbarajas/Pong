using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameStateManagement
{
    public class DuckHuntPaddle : Paddle
    {
        private float m_decrease;

        public DuckHuntPaddle(int player, Texture2D texture, Rectangle boundary)
            : base(player, texture, boundary)
        {
            m_decrease = 0.0f;
        }

        public void Update(Cloud[] clouds)
        {
            m_decrease = 0.0f;

            /* Handle collision with clouds */
            for (int i = 0; i < clouds.Length; i++)
            {
                if (Texture_Data.m_rect.Intersects(clouds[i].m_coldata.m_rect))
                {
                    if (HelperUtils.IntersectPixels(Texture_Data.m_transformation, Texture_Data.m_rect.Width,
                           Texture_Data.m_rect.Height, Texture_Data.m_color_data,
                           clouds[i].m_coldata.m_transformation, clouds[i].m_coldata.m_rect.Width,
                           clouds[i].m_coldata.m_rect.Height, clouds[i].m_coldata.m_color_data))
                    {
                        m_decrease -= AbsVelocity * clouds[i].m_slow_down;
                    }
                }
            }

            CurrVelocity = AbsVelocity + m_decrease;
        }
    }
}
