using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class Cloud
    {
        public Cloud() { }
        public float m_slow_down = 0.35f;
        public bool m_duck_used;
        public Drawable m_cloud;
        public CollisionData m_coldata;
        public Rectangle m_collision_rect;
    }

    public class Clouds
    {
        #region Properties

        public Cloud[] CloudsA
        {
            get { return m_clouds; }
        }

        public DrawableState State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        DrawableState m_state = DrawableState.Finished;

        #endregion

        #region Fields

        private int m_num;
        private int m_player;
        private Texture2D[] m_textures;
        private Cloud[] m_clouds = new Cloud[3];

        #endregion 

        #region Initialization

        public Clouds(int player, Texture2D[] textures)
        {
            m_num = 3;
            m_player = player;
            m_textures = textures;

            Reset();

            for (int i = 0; i < m_num; i++)
            {
                m_clouds[i].m_cloud.Draw_State = DrawableState.Finished;
            }
        }

        #endregion


        #region Update and Draw

        public void Update()
        {
            bool finished = true;

            for (int i = 0; i < m_num; i++)
            {
                if (m_clouds[i].m_cloud.Draw_State == DrawableState.Active)
                {
                    finished = false;
                    break;
                }
            }

            if (finished)
            {
                m_state = DrawableState.Finished;
            }

            if (m_state != DrawableState.Finished)
            {
                for (int i = 0; i < m_num; i++)
                {
                    m_clouds[i].m_cloud.Update();
                    m_clouds[i].m_collision_rect = new Rectangle((int)m_clouds[i].m_cloud.X_Pos + m_textures[i].Width / 4, (int)m_clouds[i].m_cloud.Y_Pos + m_textures[i].Height / 2, m_textures[i].Width / 2, 1);
                    m_clouds[i].m_coldata = new CollisionData(new Rectangle((int)m_clouds[i].m_cloud.X_Pos, (int)m_clouds[i].m_cloud.Y_Pos, m_clouds[i].m_coldata.m_rect.Width, m_clouds[i].m_coldata.m_rect.Height),
                                                              m_clouds[i].m_coldata.m_color_data, m_clouds[i].m_cloud.Origin, m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < m_num; i++)
            {
                m_clouds[i].m_cloud.Draw(spritebatch);
            }
        }

        #endregion

        #region Public Methods

        public void Reset()
        {
            Sprite temp;

            if (m_player == 0)
            {
                for (int i = 0; i < m_num; i++)
                {
                    temp = new Sprite();
                    temp.Sprite_Texture = m_textures[i];
                    temp.X_Pos = (float)HelperUtils.GetRandomNumber(-412, 0 - m_textures[i].Width);
                    temp.Y_Pos = (float)HelperUtils.GetRandomNumber(0, 200);
                    m_clouds[i] = new Cloud();
                    //m_clouds[i].m_pad_used = false;
                    m_clouds[i].m_duck_used = false;
                    m_clouds[i].m_cloud = new LinearXYMover(temp, 1024, (int)temp.Y_Pos, 1.0f);
                    m_clouds[i].m_collision_rect = new Rectangle((int)temp.X_Pos + m_textures[i].Width / 4, (int)temp.Y_Pos + m_textures[i].Height / 2, m_textures[i].Width / 2, 1);
                    m_clouds[i].m_coldata = new CollisionData((int)temp.X_Pos, (int)temp.Y_Pos, m_textures[i],
                         new Rectangle(0, 0, m_textures[i].Width, m_textures[i].Height), m_clouds[i].m_cloud.Origin,
                         m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
                }
            }
            else if (m_player == 1)
            {
                for (int i = 0; i < m_num; i++)
                {
                    temp = new Sprite();
                    temp.Sprite_Texture = m_textures[i];
                    temp.X_Pos = (float)HelperUtils.GetRandomNumber(1024, 1024 + 412);
                    temp.Y_Pos = (float)HelperUtils.GetRandomNumber(64, 200);
                    m_clouds[i] = new Cloud();
                    //m_clouds[i].m_pad_used = false;
                    m_clouds[i].m_duck_used = false;
                    m_clouds[i].m_cloud = new LinearXYMover(temp, -m_textures[i].Width, (int)temp.Y_Pos, 1.0f);
                    m_clouds[i].m_collision_rect = new Rectangle((int)temp.X_Pos + m_textures[i].Width / 4, (int)temp.Y_Pos + m_textures[i].Height / 2, m_textures[i].Width / 2, 1);
                    m_clouds[i].m_coldata = new CollisionData((int)temp.X_Pos, (int)temp.Y_Pos, m_textures[i],
                         new Rectangle(0, 0, m_textures[i].Width, m_textures[i].Height), m_clouds[i].m_cloud.Origin,
                         m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
                }

            }

            m_state = DrawableState.Active;
        }

        #endregion
    }
}
