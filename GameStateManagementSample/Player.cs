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

        
        public int CloudNum
        {
            get { return m_cloud.NumUses; }
            set { m_cloud.NumUses = value; }
        }
         

        public int ScoreNum
        {
            get { return m_score.NumUses; }
            set { m_score.NumUses = value; }
        }

        #endregion


        #region Fields

        private int m_player;
        private Paddle m_paddle;
        private ScreenItem m_shot;
        private ScreenItem m_duckcall;
        public ScreenItem m_cloud;
        private ScreenItem m_score;

        #endregion 

        #region Initialization

        public Player(int player)
        {
            m_player = player; 
        }

        public void LoadContent(Texture2D[] textures, SpriteFont font, SpriteFont font2, Rectangle boundary)
        {
            m_paddle = new Paddle(m_player, textures[0], boundary);

            if (m_player == 0)
            {
                m_score = new ScreenItem(10, 534, 10 + textures[1].Width/2, 544, textures[1], font2);
                m_score.Alpha = false;
                m_cloud = new ScreenItem(140, 544, 140 + textures[2].Width/2, 581, textures[2], font);
                m_cloud.NumUses = 5;
                m_cloud.Font_Tint = Color.Black;
                m_duckcall = new ScreenItem(230, 544, 230 + textures[3].Width/2, 581, textures[3], font);
                m_duckcall.NumUses = 3;
                m_duckcall.Font_Tint = Color.Black;
                m_shot = new ScreenItem(320, 544, 320+textures[4].Width/2, 581, textures[4], font);
                m_shot.NumUses = 5;
                m_shot.Font_Tint = Color.Black;
                
            }
            else if (m_player == 1)
            {

                m_shot = new ScreenItem(653, 544, 653 + textures[4].Width/2, 581, textures[4], font);
                m_shot.Effects = SpriteEffects.FlipHorizontally;
                m_shot.NumUses = 5;
                m_shot.Font_Tint = Color.Black;
                m_duckcall = new ScreenItem(743, 544, 743 + textures[3].Width / 2, 581, textures[3], font);
                m_duckcall.NumUses = 2;
                m_duckcall.Effects = SpriteEffects.FlipHorizontally;
                m_duckcall.Font_Tint = Color.Black;
                m_cloud = new ScreenItem(833, 544, 833 + textures[2].Width / 2, 581, textures[2], font);
                m_cloud.Effects = SpriteEffects.FlipHorizontally;
                m_cloud.NumUses = 5;
                m_cloud.Font_Tint = Color.Black;
                m_score = new ScreenItem(908, 534, 908 + textures[1].Width/2, 544, textures[1], font2);
                m_score.Alpha = false;
            }
        }

        #endregion

        #region Update and Draw

        public void UpdatePaddle(KeyboardState keyboardState, GamePadState gamePadState, Cloud[] clouds) 
        {
            m_paddle.Update(keyboardState, gamePadState, clouds);
        }

        public void DrawPaddle(SpriteBatch spritebatch)
        {
            m_paddle.Draw(spritebatch);
        }

        public void UpdateItems()
        {
            m_score.Update();
            m_cloud.Update();
            m_duckcall.Update();
            m_shot.Update();
        }

        public void DrawItems(SpriteBatch spritebatch)
        {
            m_score.Draw(spritebatch);
            m_cloud.Draw(spritebatch);
            m_duckcall.Draw(spritebatch);
            m_shot.Draw(spritebatch);
        }

        #endregion
    }
}
