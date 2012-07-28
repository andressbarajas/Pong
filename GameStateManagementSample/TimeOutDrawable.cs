using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class TimeOutDrawable : SpriteDecorator
    {
        private bool m_draw;
        private int m_ticks = 0;
        private int m_total_ticks;

        public TimeOutDrawable(Drawable sprite, int ticks, bool draw) : base(sprite) {
            m_draw = draw;
            m_total_ticks = ticks;
        }

        public override void Update()
        {
            if (Draw_State != DrawableState.Finished)
            {
                if (m_ticks < m_total_ticks)
                {
                    m_ticks++;
                }
                else
                {
                    Draw_State = DrawableState.Finished;
                }
            }

            m_drawable_sprite.Update();
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (m_draw)
            {
                m_drawable_sprite.Draw(spriteBatch);
            }
            else
            {
                if (Draw_State != DrawableState.Finished)
                {
                    m_drawable_sprite.Draw(spriteBatch);
                }
            }
        }
        
        public override void Reset()
        {
            m_ticks = 0;
            Draw_State = DrawableState.Active;
        }
    }
}
