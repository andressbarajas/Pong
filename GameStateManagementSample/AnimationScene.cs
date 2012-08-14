using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public class AnimationScene
    {
        #region Properties

        public class SceneUnit {
            public Drawable       m_sprite;
            public SoundEffect    m_sound;
        }

        public DrawableState Scene_State
        {
            get { return m_scene_state; }
            set { m_scene_state = value; }
        }

        private DrawableState m_scene_state = DrawableState.Finished;

        public SpriteEffects Effect
        {
            get { return m_effect; }
            set { m_effect = value; }
        }

        private SpriteEffects m_effect = SpriteEffects.None;

        public float X_Pos
        {
            get { return m_currUnit.m_sprite.X_Pos; }
            set { m_currUnit.m_sprite.X_Pos = value; }
        }

        public float Y_Pos
        {
            get { return m_currUnit.m_sprite.Y_Pos; }
            set { m_currUnit.m_sprite.Y_Pos = value; }
        }

        #endregion

        #region Fields

        private int[] m_animations;

        private bool m_played_sound = false;
        private SceneUnit m_currUnit;
        private Queue<SceneUnit> m_animQ = new Queue<SceneUnit>();
        private List<SceneUnit> m_animList = new List<SceneUnit>();

        #endregion 

        #region Initialization

        public AnimationScene() { }

        #endregion

        #region Update and Draw

        public void Update()
        {
            if (m_scene_state != DrawableState.Finished)
            {
                if (m_currUnit.m_sprite.Draw_State == DrawableState.Finished)
                {
                    m_played_sound = false;
                    if (m_animQ.Count == 0) // Ran out of animations to perform, reset stuff
                    {
                        m_currUnit.m_sprite.Reset(); // So it wont stay finished for the next time around
                        m_scene_state = DrawableState.Finished;
                        return;
                    }
                    else // On to the next animation
                    {
                        float x, y;
                        x = m_currUnit.m_sprite.X_Pos;
                        y = m_currUnit.m_sprite.Y_Pos;
                        m_currUnit.m_sprite.Reset(); // So it wont stay finished for the next time around
                        m_currUnit = m_animQ.Dequeue();
                        m_currUnit.m_sprite.X_Pos = x;
                        m_currUnit.m_sprite.Y_Pos = y;
                    }
                }

                if (!m_played_sound && m_currUnit.m_sound != null)
                {
                    m_currUnit.m_sound.Play();
                    m_played_sound = true;
                }

                // Update current Animation 
                m_currUnit.m_sprite.Update();
            }
        }

        public void Draw(SpriteBatch spritebatch) 
        {
            if (m_scene_state != DrawableState.Finished)
            {
                m_currUnit.m_sprite.Draw(spritebatch);
            }
        }

        #endregion

        #region Public Methods

        public void AddAnimation(Drawable sprite_anim)
        {
            SceneUnit temp = new SceneUnit();
            temp.m_sprite = sprite_anim;
            m_animList.Add(temp);
        }

        public void AddAnimation(Drawable sprite_anim, SoundEffect sound)
        {
            SceneUnit temp = new SceneUnit();
            temp.m_sprite = sprite_anim;
            temp.m_sound = sound;
            m_animList.Add(temp);
        }


        public void BuildScene(int[] animations) 
        {
            m_animations = new int[animations.Length];
            m_animQ.Clear();

            for (int i = 0; i < animations.Length; i++)
            {
                m_animations[i] = animations[i];
                /*
                if(m_effect == SpriteEffects.None) {
                    temp.Effects = SpriteEffects.None;
                } else {
                    temp.Effects = SpriteEffects.FlipHorizontally;
                }
                 * */
                m_animQ.Enqueue(m_animList[animations[i]]);
            }

            m_currUnit = m_animQ.Dequeue(); // Grab the first animation
        }

        public void ResetScene()
        {
            for (int i = 0; i < m_animations.Length; i++)
            {
                if (m_effect == SpriteEffects.None)
                {
                    m_animList[m_animations[i]].m_sprite.Effects = SpriteEffects.None;
                }
                else
                {
                    m_animList[m_animations[i]].m_sprite.Effects = SpriteEffects.FlipHorizontally;
                }
                
                m_animQ.Enqueue(m_animList[m_animations[i]]);
            }

            m_currUnit = m_animQ.Dequeue(); // Grab the first animation
        }

        public void Clear()
        {
            m_animList.Clear();
        }

        #endregion
    }
}
