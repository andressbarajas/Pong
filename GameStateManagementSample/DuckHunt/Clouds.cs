using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public class Cloud : ScreenObject
    {
        public Cloud() { }
        public float m_slow_down = 0.30f;
        public bool m_used;
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
        //private Texture2D m_debug;
        private Cloud[] m_clouds = new Cloud[6];

        #endregion 

        #region Initialization

        public Clouds(int player, Texture2D[] textures)//, Texture2D debug)
        {
            m_num = 6;
           // m_debug = debug;
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
                int tnum = 0;

                for (int i = 0; i < m_num/2; i++, tnum++)
                {
                    m_clouds[i].m_cloud.Update();
                    m_clouds[i].m_collision_rect = new Rectangle((int)m_clouds[i].m_cloud.X_Pos + m_textures[tnum].Width / 4, (int)m_clouds[i].m_cloud.Y_Pos + m_textures[tnum].Height / 2, m_textures[tnum].Width / 2, 4);
                    m_clouds[i].m_coldata = new CollisionData(new Rectangle((int)m_clouds[i].m_cloud.X_Pos, (int)m_clouds[i].m_cloud.Y_Pos, m_clouds[i].m_coldata.m_rect.Width, m_clouds[i].m_coldata.m_rect.Height),
                                                              m_clouds[i].m_coldata.m_color_data, m_clouds[i].m_cloud.Origin, m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
                }

                tnum = 0;

                for (int i = m_num/2; i < m_num; i++, tnum++)
                {
                    m_clouds[i].m_cloud.Update();
                    m_clouds[i].m_collision_rect = new Rectangle((int)m_clouds[i].m_cloud.X_Pos + m_textures[tnum].Width / 4, (int)m_clouds[i].m_cloud.Y_Pos + m_textures[tnum].Height / 2, m_textures[tnum].Width / 2, 4);
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
                //spritebatch.Draw(m_debug, m_clouds[i].m_collision_rect, Color.Red);
            }
        }

        #endregion

        #region Public Methods

        public void Reset()
        {
            Sprite temp;
            int tnum = 0;

            if (m_player == 0)
            {
                for (int i = 0; i < m_num/2; i++, tnum++)
                {
                    temp = new Sprite();
                    temp.Texture = m_textures[tnum];
                    temp.X_Pos = (float)HelperUtils.GetRandomNumber(-412 - HelperUtils.SafeBoundary.X, HelperUtils.SafeBoundary.X + -m_textures[tnum].Width);
                    temp.Y_Pos = (float)HelperUtils.GetRandomNumber(HelperUtils.SafeBoundary.Y, HelperUtils.SafeBoundary.Y + 200);
                    m_clouds[i] = new Cloud();
                    m_clouds[i].m_used = false;
                    m_clouds[i].m_cloud = new LinearXYMover(temp, 2*HelperUtils.SafeBoundary.X + 1024, (int)temp.Y_Pos, 1.0f);
                    m_clouds[i].m_collision_rect = new Rectangle((int)temp.X_Pos + m_textures[tnum].Width / 4, (int)temp.Y_Pos + m_textures[tnum].Height / 2, m_textures[tnum].Width / 2, 4);
                    m_clouds[i].m_coldata = new CollisionData((int)temp.X_Pos, (int)temp.Y_Pos, m_textures[tnum],
                         new Rectangle(0, 0, m_textures[tnum].Width, m_textures[tnum].Height), m_clouds[i].m_cloud.Origin,
                         m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
                }
                tnum = 0;

                for (int i = m_num/2; i < m_num; i++, tnum++)
                {
                    temp = new Sprite();
                    temp.Texture = m_textures[tnum];
                    temp.X_Pos = (float)HelperUtils.GetRandomNumber(-412 - HelperUtils.SafeBoundary.X, HelperUtils.SafeBoundary.X + -m_textures[tnum].Width);
                    temp.Y_Pos = (float)HelperUtils.GetRandomNumber(HelperUtils.SafeBoundary.Y, HelperUtils.SafeBoundary.Y + 200);
                    m_clouds[i] = new Cloud();
                    m_clouds[i].m_used = false;
                    m_clouds[i].m_cloud = new LinearXYMover(temp, 2*HelperUtils.SafeBoundary.X + 1024, (int)temp.Y_Pos, 1.0f);
                    m_clouds[i].m_collision_rect = new Rectangle((int)temp.X_Pos + m_textures[tnum].Width / 4, (int)temp.Y_Pos + m_textures[tnum].Height / 2, m_textures[tnum].Width / 2, 4);
                    m_clouds[i].m_coldata = new CollisionData((int)temp.X_Pos, (int)temp.Y_Pos, m_textures[tnum],
                         new Rectangle(0, 0, m_textures[tnum].Width, m_textures[tnum].Height), m_clouds[i].m_cloud.Origin,
                         m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
                }

            }
            else if (m_player == 1)
            {
                for (int i = 0; i < m_num / 2; i++, tnum++)
                {
                    temp = new Sprite();
                    temp.Texture = m_textures[tnum];
                    temp.X_Pos = (float)HelperUtils.GetRandomNumber(HelperUtils.SafeBoundary.X + 1024, HelperUtils.SafeBoundary.X + 1024 + 412);
                    temp.Y_Pos = (float)HelperUtils.GetRandomNumber(HelperUtils.SafeBoundary.Y, HelperUtils.SafeBoundary.Y + 200);
                    m_clouds[i] = new Cloud();
                    m_clouds[i].m_used = false;
                    m_clouds[i].m_cloud = new LinearXYMover(temp, -m_textures[tnum].Width - 2*HelperUtils.SafeBoundary.X, (int)temp.Y_Pos, 1.0f);
                    m_clouds[i].m_collision_rect = new Rectangle((int)temp.X_Pos + m_textures[tnum].Width / 4, (int)temp.Y_Pos + m_textures[tnum].Height / 2, m_textures[tnum].Width / 2, 4);
                    m_clouds[i].m_coldata = new CollisionData((int)temp.X_Pos, (int)temp.Y_Pos, m_textures[tnum],
                         new Rectangle(0, 0, m_textures[tnum].Width, m_textures[tnum].Height), m_clouds[i].m_cloud.Origin,
                         m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
                }

                tnum = 0;

                for (int i = m_num / 2; i < m_num; i++, tnum++)
                {
                    temp = new Sprite();
                    temp.Texture = m_textures[tnum];
                    temp.X_Pos = (float)HelperUtils.GetRandomNumber(HelperUtils.SafeBoundary.X + 1024, HelperUtils.SafeBoundary.X + 1024 + 412);
                    temp.Y_Pos = (float)HelperUtils.GetRandomNumber(HelperUtils.SafeBoundary.Y, HelperUtils.SafeBoundary.Y + 200);
                    m_clouds[i] = new Cloud();
                    m_clouds[i].m_used = false;
                    m_clouds[i].m_cloud = new LinearXYMover(temp, -m_textures[tnum].Width - 2*HelperUtils.SafeBoundary.X, (int)temp.Y_Pos, 1.0f);
                    m_clouds[i].m_collision_rect = new Rectangle((int)temp.X_Pos + m_textures[tnum].Width / 4, (int)temp.Y_Pos + m_textures[tnum].Height / 2, m_textures[tnum].Width / 2, 4);
                    m_clouds[i].m_coldata = new CollisionData((int)temp.X_Pos, (int)temp.Y_Pos, m_textures[tnum],
                         new Rectangle(0, 0, m_textures[tnum].Width, m_textures[tnum].Height), m_clouds[i].m_cloud.Origin,
                         m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
                }

            }

            m_state = DrawableState.Active;
        }

        #endregion
    }
}
