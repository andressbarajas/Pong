using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class AnimationManager
    {
        #region Properties

        public DrawableState Manager_State
        {
            get { return m_manager_state; }
            set { m_manager_state = value; }
        }

        private DrawableState m_manager_state = DrawableState.Finished;

        public float X_Pos
        {
            get { return m_currScene.X_Pos; }
            set { m_currScene.X_Pos = value; }
        }

        public float Y_Pos
        {
            get { return m_currScene.Y_Pos; }
            set { m_currScene.Y_Pos = value; }
        }

        #endregion 

        #region Fields

        private int[] m_scenes;
        private AnimationScene m_currScene;
        private Queue<AnimationScene> m_sceneQ = new Queue<AnimationScene>();
        private List<AnimationScene> m_sceneList = new List<AnimationScene>();

        #endregion 

        #region Initialization

        public AnimationManager() { }

        #endregion

        #region Update and Draw

        public void Update()
        {
            if (m_manager_state != DrawableState.Finished)
            {
                if (m_currScene.Scene_State == DrawableState.Finished)
                {
                    if (m_sceneQ.Count == 0) // Ran out of scenes to perform, reset stuff
                    {
                        m_currScene.ResetScene(); // So it wont stay finished for the next time around
                        m_manager_state = DrawableState.Finished;
                        return;
                    }
                    else // On to the next animation
                    {
                        m_currScene.ResetScene(); // So it wont stay finished for the next time around
                        m_currScene = m_sceneQ.Dequeue();
                    }
                }

                // Update current Animation 
                m_currScene.Update();
            }

        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (m_manager_state != DrawableState.Finished)
            {
                m_currScene.Draw(spritebatch);
            }
        }

        #endregion

        #region Public Methods

        public void AddScene(AnimationScene scene)
        {
            m_sceneList.Add(scene);
        }

        public void Build(int[] scenes) 
        {
            m_scenes = new int[scenes.Length];
            m_sceneQ.Clear();

            for (int i = 0; i < scenes.Length; i++)
            {
                m_scenes[i] = scenes[i];
                m_sceneQ.Enqueue(m_sceneList[scenes[i]]);
            }

            m_currScene = m_sceneQ.Dequeue(); // Grab the first animation
        }

        public void Reset()
        {
            for (int i = 0; i < m_scenes.Length; i++)
            {
                m_sceneQ.Enqueue(m_sceneList[m_scenes[i]]);
            }

            m_currScene = m_sceneQ.Dequeue(); // Grab the first animation
        }

        public void ClearList()
        {
            m_sceneList.Clear();
        }

        #endregion
    }
}
