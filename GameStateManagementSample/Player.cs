using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameStateManagement
{
    public class Player
    {
        #region Properties

        public CollisionData CData
        {
            get { return m_paddle.Texture_Data; }
        }

        public int ShotNum
        {
            get { return m_shot.NumUses; }
            set { m_shot.NumUses = value; }
        }

        public int DuckCallNum
        {
            get { return m_duckcall.NumUses; }
            set { m_duckcall.NumUses = value; }
        }

        /*
        public int CloudNum
        {
            get { return m_cloud.NumUses; }
            set { m_cloud.NumUses = value; }
        }
         * */

        public int ScoreNum
        {
            get { return m_score.NumUses; }
            set { m_score.NumUses = value; }
        }

        #endregion


        #region Fields

        private int m_player;
        private Paddle m_paddle;
        private Shot m_shot;
        private DuckCall m_duckcall;
        //public Cloud m_cloud;
        private Score m_score;
        //private ContentManager m_content;

        #endregion 

        #region Initialization

        public Player(int player)
        {
            m_player = player; 
        }

        public void LoadContent(Texture2D[] textures, SpriteFont font, Rectangle boundary)
        {
            m_paddle = new Paddle(m_player, textures[0], boundary);

            if (m_player == 0)
            {
                m_score = new Score(138, 598, textures[1], font);
                
                //m_cloud = new Cloud(268, 608, textures[2], font);
                //m_cloud.NumUses = 5;
                m_duckcall = new DuckCall(358, 608, textures[3], font);
                m_duckcall.NumUses = 3;
                m_shot = new Shot(448, 608, textures[4], font);
                m_shot.NumUses = 5;
                
            }
            else if (m_player == 1)
            {
                
                m_shot = new Shot(781, 608, textures[4], font);
                m_shot.Effects = SpriteEffects.FlipHorizontally;
                m_shot.NumUses = 5;
                m_duckcall = new DuckCall(871, 608, textures[3], font);
                m_duckcall.NumUses = 2;
                m_duckcall.Effects = SpriteEffects.FlipHorizontally;
                //m_cloud = new Cloud(961, 608, textures[2], font);
                //m_cloud.Effects = SpriteEffects.FlipHorizontally;
                //m_cloud.NumUses = 5;
                m_score = new Score(1036, 598, textures[1], font); 
            }
        }

        #endregion

        #region Update and Draw

        public void UpdatePaddle(KeyboardState keyboardState, GamePadState gamePadState) 
        {
            m_paddle.Update(keyboardState, gamePadState);
        }

        public void DrawPaddle(SpriteBatch spritebatch)
        {
            m_paddle.Draw(spritebatch);
        }

        public void UpdateItems()
        {
            m_score.Update();
            //m_cloud.Update();
            m_duckcall.Update();
            m_shot.Update();
        }

        public void DrawItems(SpriteBatch spritebatch)
        {
            m_score.Draw(spritebatch);
            //m_cloud.Draw(spritebatch);
            m_duckcall.Draw(spritebatch);
            m_shot.Draw(spritebatch);
        }

        #endregion
    }
}
