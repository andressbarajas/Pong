using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class DuckPopulation
    {
        #region Properties

        public int DuckCount
        {
            get { return m_duck_count; }
            set { m_duck_count = value; }
        }

        private int m_duck_count;

        #endregion

        #region Fields
        private Texture2D m_duck_txt;
        
        private Sprite m_counter;
        private Drawable[] m_ducks = new Drawable[10];
        private List<DuckHuntBall> m_pongballs = new List<DuckHuntBall>();

        #endregion

        #region Initialization

        public DuckPopulation(Texture2D duck, Sprite counter, DuckHuntBall ballone, DuckHuntBall balltwo)
        {
            int x, y;
            Sprite temp;

            m_duck_txt = duck;
            m_duck_count = 10;
            m_counter = counter;
            m_pongballs.Add(ballone);
            m_pongballs.Add(balltwo);

            x = 0; 
            y = 0;

            // Create the ducks for the counter
            for (int i = 0; i < 10; i++)
            {
                temp = new Sprite();
                temp.Sprite_Texture = duck;
                temp.X_Pos = x;
                temp.Y_Pos = y;
                temp.Draw_State = DrawableState.Active;
                m_ducks[i] = temp;
                x += 8;
                y += 8;
            }
        }

        #endregion

        #region Update and Draw

        public void UpdateCounter()
        {

        }

        public void DrawCounter(SpriteBatch spritebatch)
        {
            m_counter.Draw(spritebatch);

            for (int i = 0; i < m_ducks.Length; i++)
            {
                m_ducks[i].Draw(spritebatch);
            }
        }

        public void UpdateBalls(CollisionData p1paddle, CollisionData p2paddle)
        {
            m_pongballs[0].Update(p1paddle, p2paddle);
            m_pongballs[1].Update(p1paddle, p2paddle);
        }

        public void DrawBalls(SpriteBatch spritebatch)
        {
            m_pongballs[0].Draw(spritebatch);
            m_pongballs[1].Draw(spritebatch);
        }

        #endregion 

        #region Public Methods

        // Release another ball on the screen only when there isnt already two in play
        public void ReleaseDuck()
        {
            if (m_pongballs[0].Ball_State == BallState.DeadBall)
            {
                m_pongballs[0].Ball_State = BallState.Active;
            }
            else if (m_pongballs[1].Ball_State == BallState.DeadBall)
            {
                m_pongballs[1].Ball_State = BallState.Active;
            }
        }

        public bool Intermission()
        {
            return (m_pongballs[0].Ball_State == BallState.DeadBall && m_pongballs[1].Ball_State == BallState.DeadBall);
        }

        public bool GameOver()
        {
            return (m_duck_count == 0) ? true : false;
        }

        #endregion

        #region Private Methods

        private void SetBlinkerToCurrentDuck()
        {
            //m_ducks[m_duck_count - 1] = new Blinker(m_ducks[m_duck_count - 1], frames);
        }

        #endregion
    }
}
