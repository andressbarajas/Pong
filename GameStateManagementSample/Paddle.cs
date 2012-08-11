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
    public class Paddle
    {
        #region Properties

        public float AbsVelocity
        {
            get { return m_abs_velocity; }
            set { m_abs_velocity = value; }
        }

        private float m_abs_velocity = 7.0f;

        public float CurrVelocity
        {
            get { return m_curr_velocity; }
            set { m_curr_velocity = value; }
        }

        private float m_curr_velocity;

        public Rectangle Rect
        {
            get { return m_rect;  }
            set { m_rect = value;  }
        }

        private Rectangle m_rect;

        public CollisionData Texture_Data
        {
            get { return m_texture_data; }
            set { m_texture_data = value; }
        }

        private CollisionData m_texture_data;

        #endregion

        #region Fields

        protected int m_player;
        protected int m_xpos, m_ypos;
        protected Rectangle m_scrn_boundary;
        protected Texture2D m_texture;
        protected Color[] m_colordata;

        #endregion

        #region Initialization

        public Paddle(int player, Texture2D texture, Rectangle boundary)
        { 
            m_player = player;
            m_texture = texture;
            m_scrn_boundary = boundary;
            m_curr_velocity = m_abs_velocity;

            InitPaddle();
        }

        #endregion

        #region Update and Draw

        public virtual void Update(KeyboardState keyboardState, GamePadState gamePadState)
        {
            if (m_player == 1)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                    m_ypos -= (int)(m_curr_velocity);

                if (keyboardState.IsKeyDown(Keys.Down))
                    m_ypos += (int)(m_curr_velocity);
            }
            else if (m_player == 0)
            {
                if (keyboardState.IsKeyDown(Keys.A))
                    m_ypos -= (int)(m_curr_velocity);

                if (keyboardState.IsKeyDown(Keys.Z))
                    m_ypos += (int)(m_curr_velocity);
            }

            Vector2 thumbstick = gamePadState.ThumbSticks.Left;
            m_ypos -= (int)(thumbstick.Y * m_curr_velocity);

            // Handle boundaries
            if (m_ypos < m_scrn_boundary.Top)
            {
                m_ypos = m_scrn_boundary.Top;
            }
            if((m_ypos + m_texture.Height) > m_scrn_boundary.Bottom) {
                m_ypos = m_scrn_boundary.Bottom - m_texture.Height;
            }

            m_rect = new Rectangle(m_xpos, m_ypos, m_texture.Width, m_texture.Height);
            m_texture_data = new CollisionData(m_rect, m_colordata, Vector2.Zero, 1.0f, 0.0f, SpriteEffects.None);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_texture, m_rect, Color.White);
        }

        #endregion

        #region Public Methods

        public void InitPaddle()
        {
            m_colordata = new Color[m_texture.Width * m_texture.Height];

            m_texture.GetData(m_colordata);

            // take boundary and set position according to texture size and the boundary.
            if(m_player == 0) 
            { 
                m_xpos = m_scrn_boundary.Left - m_texture.Width; 
            } 
            else if (m_player == 1) 
            {
                m_xpos = m_scrn_boundary.Right;
            }

            // Center the paddle in the middle of the boundary
            m_ypos = ((m_scrn_boundary.Bottom - m_scrn_boundary.Top) / 2) - (m_texture.Height / 2) + m_scrn_boundary.Top;

            m_rect = new Rectangle(m_xpos, m_ypos, m_texture.Width, m_texture.Height);

            m_texture_data = new CollisionData(m_rect, m_colordata, Vector2.Zero, 1.0f, 0.0f, SpriteEffects.None);
        }

        #endregion
    }
}
