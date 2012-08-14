using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace PongaThemes
{
    public class DuckHuntPlayer
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
        private DuckHuntPaddle m_paddle;
        private ScreenItem m_shot;
        private ScreenItem m_duckcall;
        public ScreenItem m_cloud;
        private ScreenItem m_score;

        #endregion 

        #region Initialization

        public DuckHuntPlayer(int player)
        {
            m_player = player; 
        }

        public void LoadContent(Texture2D[] textures, SpriteFont font, SpriteFont font2, Rectangle boundary)
        {
            m_paddle = new DuckHuntPaddle(m_player, textures[0], boundary);

            if (m_player == 0)
            {
                m_score = new ScreenItem(HelperUtils.SafeBoundary.X + 10, HelperUtils.SafeBoundary.Y + 536, HelperUtils.SafeBoundary.X + 10 + textures[1].Width / 2, HelperUtils.SafeBoundary.Y + 544, textures[1], font2);
                m_score.Alpha = false;
                m_cloud = new ScreenItem(HelperUtils.SafeBoundary.X + 140, HelperUtils.SafeBoundary.Y + 544, HelperUtils.SafeBoundary.X + 140 + textures[2].Width / 2, HelperUtils.SafeBoundary.Y + 581, textures[2], font);
                m_cloud.NumUses = 5;
                m_cloud.Font_Tint = Color.Black;
                m_duckcall = new ScreenItem(HelperUtils.SafeBoundary.X + 230, HelperUtils.SafeBoundary.Y + 544, HelperUtils.SafeBoundary.X + 230 + textures[3].Width / 2, HelperUtils.SafeBoundary.Y + 581, textures[3], font);
                m_duckcall.NumUses = 2;
                m_duckcall.Font_Tint = Color.Black;
                m_shot = new ScreenItem(HelperUtils.SafeBoundary.X + 320, HelperUtils.SafeBoundary.Y + 544, HelperUtils.SafeBoundary.X + 320 + textures[4].Width / 2, HelperUtils.SafeBoundary.Y + 581, textures[4], font);
                m_shot.NumUses = 5;
                m_shot.Font_Tint = Color.Black;
                
            }
            else if (m_player == 1)
            {
                m_shot = new ScreenItem(HelperUtils.SafeBoundary.X + 653, HelperUtils.SafeBoundary.Y + 544, HelperUtils.SafeBoundary.X + 653 + textures[4].Width / 2, HelperUtils.SafeBoundary.Y + 581, textures[4], font);
                m_shot.Effects = SpriteEffects.FlipHorizontally;
                m_shot.NumUses = 5;
                m_shot.Font_Tint = Color.Black;
                m_duckcall = new ScreenItem(HelperUtils.SafeBoundary.X + 743, HelperUtils.SafeBoundary.Y + 544, HelperUtils.SafeBoundary.X + 743 + textures[3].Width / 2, HelperUtils.SafeBoundary.Y + 581, textures[3], font);
                m_duckcall.NumUses = 2;
                m_duckcall.Effects = SpriteEffects.FlipHorizontally;
                m_duckcall.Font_Tint = Color.Black;
                m_cloud = new ScreenItem(HelperUtils.SafeBoundary.X + 833, HelperUtils.SafeBoundary.Y + 544, HelperUtils.SafeBoundary.X + 833 + textures[2].Width / 2, HelperUtils.SafeBoundary.Y + 581, textures[2], font);
                m_cloud.Effects = SpriteEffects.FlipHorizontally;
                m_cloud.NumUses = 5;
                m_cloud.Font_Tint = Color.Black;
                m_score = new ScreenItem(HelperUtils.SafeBoundary.X + 908, HelperUtils.SafeBoundary.Y + 536, HelperUtils.SafeBoundary.X + 908 + textures[1].Width / 2, HelperUtils.SafeBoundary.Y + 544, textures[1], font2);
                m_score.Alpha = false;
            }
        }

        #endregion

        #region Update and Draw

        public void UpdatePaddle(KeyboardState keyboardState, GamePadState gamePadState, Cloud[] clouds) 
        {
            m_paddle.Update(clouds);
            m_paddle.Update(keyboardState, gamePadState);
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
