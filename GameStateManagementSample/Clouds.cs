using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class CloudI
    {
        public CloudI() { }
        public bool m_used;
        public Drawable m_cloud;
        public CollisionData m_coldata;
    }

    public class Clouds
    {
        #region Properties

        public CloudI[] CloudsA
        {
            get { return m_clouds; }
        }


        #endregion

        #region Fields

        private int m_num;
        private Texture2D[] m_textures;
        private CloudI[] m_clouds = new CloudI[3];

        #endregion 

        #region Initialization

        public Clouds(Texture2D[] textures)
        {
            m_num = 3;
            m_textures = textures;

            Reset();
        }

        #endregion


        #region Update and Draw

        public void Update()
        {
            for (int i = 0; i < m_num; i++)
            {
                m_clouds[i].m_cloud.Update();
                m_clouds[i].m_coldata = new CollisionData(new Rectangle((int)m_clouds[i].m_cloud.X_Pos, (int)m_clouds[i].m_cloud.Y_Pos, m_clouds[i].m_coldata.m_rect.Width, m_clouds[i].m_coldata.m_rect.Height), 
                                                          m_clouds[i].m_coldata.m_color_data, m_clouds[i].m_cloud.Origin, m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
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

            for (int i = 0; i < m_num; i++)
            {              
                temp = new Sprite();
                temp.Sprite_Texture = m_textures[i];
                temp.X_Pos = (float)HelperUtils.GetRandomNumber(1024, 1024 + 412);
                temp.Y_Pos = (float)HelperUtils.GetRandomNumber(64, 250);
                m_clouds[i] = new CloudI();
                m_clouds[i].m_used = false;
                m_clouds[i].m_cloud = new LinearXYMover(temp, -m_textures[i].Width, (int)temp.Y_Pos, 1.0f);    
                m_clouds[i].m_coldata = new CollisionData((int)temp.X_Pos, (int)temp.Y_Pos, m_textures[i],
                     new Rectangle(0, 0, m_textures[i].Width, m_textures[i].Height), m_clouds[i].m_cloud.Origin,
                     m_clouds[i].m_cloud.Scale, m_clouds[i].m_cloud.Rotation, m_clouds[i].m_cloud.Effects);
            }
        }

        #endregion
    }
}
