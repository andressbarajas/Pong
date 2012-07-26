using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameStateManagement
{
    public enum Direction
    {
        Left,
        Right,
        Top,
        Bottom,
        None,
    }

    public enum BallState
    {
        Active,
        OutofBounds,
        DeadBall,
        Limbo,
    }

    public abstract class PongBall
    {
        #region Properties

        
        /// <summary>
        /// Gets the x position of the pong ball.
        /// </summary>
        public int X_Pos
        {
            get { return m_xpos; }
            set { m_xpos = value; } // Took away protected
        }

        int m_xpos;

        /// <summary>
        /// Gets the y position of the pong ball.
        /// </summary>
        public int Y_Pos
        {
            get { return m_ypos; }
            set { m_ypos = value; }  // Took away protected
        }

        int m_ypos;
        
        /// <summary>
        /// Gets the x velocity of the pong ball.
        /// </summary>
        public float X_Vel
        {
            get { return m_xvel; }
            set { m_xvel = value; }
        }

        float m_xvel = 0;

        /// <summary>
        /// Gets the y velocity of the pong ball.
        /// </summary>
        public float Y_Vel
        {
            get { return m_yvel; }
            set { m_yvel = value; }
        }

        float m_yvel = 0;

        /// <summary>
        /// Gets the velocity magnitude of the pong ball.
        /// </summary>
        public float Curr_Vel_Mag
        {
            get { return (float)Math.Sqrt(Math.Pow(m_xvel, 2) + Math.Pow(m_yvel, 2)); }
        }

        public float Init_Vel_Mag
        {
            get { return m_vel_mag; }
            set { m_vel_mag = value; }
        }

        float m_vel_mag = 5.0f;

        
        /// <summary>
        /// Gets the scale of the pong ball.
        /// </summary>
        public float Ball_Scale
        {
            get { return m_scale; }
            set { m_scale = value; }
        }

        float m_scale = 1.0f;

        /// <summary>
        /// Gets the rectangle of the pong ball.
        /// </summary>
        public Rectangle PongBall_Rect
        {
            get { return m_pongball_rect; }
            set { m_pongball_rect = value; }
        }

        Rectangle m_pongball_rect;   

        public BallState Ball_State
        {
            get { return m_ball_state; }
            set { m_ball_state = value; }
        }

        BallState m_ball_state;

        public Direction Ball_Dir
        {
            get { return m_dir; }
            set { m_dir = value; }
        }

        Direction m_dir = Direction.None;

        #endregion

        #region Fields

        protected int m_init_x, m_init_y;
        protected CollisionData []m_scrn_boundary;
        protected Direction m_hit;

        protected Matrix m_pongBallTransform;

        protected SpriteEffects m_effect;

        protected List<AnimatedSprite>  m_animList;
        protected AnimatedSprite        m_currAnim;
        protected CollisionData          m_currAnimBData;

        #endregion

        #region Update and Draw

        public virtual void Update(CollisionData player1, CollisionData player2) { }
        
        public void Draw(SpriteBatch spritebatch)
        {          
            if(m_ball_state != BallState.DeadBall) // Dont draw the ball if we dont need to
            {
                m_currAnim.Draw(spritebatch);
            }
        }

        #endregion

        #region Public Methods

        public void ResetBall()
        {
            float[] temp = new float[2];
            m_hit = Direction.None;
            m_ball_state = BallState.Active;

            // Set to initial position
            m_xpos = m_init_x;
            m_ypos = m_init_y;

            // Reset each animation to the current location
            for (int i = 0; i < m_animList.Count; i++)
            {
                m_animList[i].X_Pos = m_xpos;
                m_animList[i].Y_Pos = m_ypos;
            }

            temp = GenerateVelocities(m_vel_mag);

            // Set initial velocity of x and y
            m_xvel = temp[0];
            m_yvel = temp[1];

            if (m_xvel < 0.0f)
            {
                m_dir = Direction.Left;
                m_effect = SpriteEffects.None;
            }
            else
            {
                m_dir = Direction.Right;
                m_effect = SpriteEffects.None;
            }
        }

        public void AddAnimation(AnimatedSprite sprite)
        {
            m_animList.Add(sprite);
        }

        public void SetCurrentAnimation(int index)
        {
            m_currAnim = m_animList[index];
            m_currAnim.Reset();
            m_currAnim.Dir = m_dir;
            m_currAnim.X_Pos = m_xpos;
            m_currAnim.Y_Pos = m_ypos; 
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Given a Velocity magnitude that we want to achieve, it generates two velocity components
        /// </summary>
        private float[] GenerateVelocities(float vel_magnitude)
        {
            float angle;
            float[] velocities = new float[2];

            // Randomly decide if we are going left or right
            if (GetRandomNumber(0.0, 10.0) < 6.0)
            {
                angle = (float)GetRandomNumber(30.0, 70.0); // Right
            }
            else 
            {
                angle = (float)GetRandomNumber(110.0, 150.0); // Left
            }

            // Generate X velocity
            velocities[0] = (float)(vel_magnitude * Math.Cos(angle * (Math.PI / 180)));

            // Generate Y velocity
            velocities[1] = (float)(vel_magnitude * Math.Sin(angle * (Math.PI / 180)));

            return velocities;
        }

        /* Random Num generator taken from http://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers */
        private double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        #endregion
    }
}
