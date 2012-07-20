using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class AnimatedSprite : Drawable
    {
        #region Properties

        public class AnimFrame
        {
            public int m_ticks;
            public Rectangle m_source;
            public SoundEffect m_soundEffect;
        }

        public Direction Dir
        {
            get { return m_dir; }
            set { m_dir = value; }
        }

        private Direction m_dir = Direction.Right; // Default direction we are facing is right

        public int Width
        {
            get { return m_width; }
            set { m_width = value;  }
        }

        private int m_width;

        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        private int m_height;

        public Texture2D Texture
        {
            get { return m_texture; }
        }

        private Texture2D m_texture;

        public Rectangle Sprite_Src_Rect
        {
            get { return m_frames[m_currentFrame].m_source; }
        }

        #endregion

        #region Fields

        private bool m_loop;
        private int m_currentTick;
        private int m_totalTicks;

        private int m_currentFrame;
        private int m_totalFrames;

        private List<AnimFrame> m_frames; 

        #endregion

        #region Initialization

        //constructor 
        public AnimatedSprite() 
        { 
            Reset();
        }

        #endregion

        #region Update and Draw

        public override void Update()
        {
            if (Draw_State != DrawableState.Finished && m_texture != null)
            {
                if (m_currentTick == 0 && m_totalTicks == 0) { // Called update for the first time
                    m_totalTicks = m_frames[m_currentFrame].m_ticks;
                }
                if (m_currentFrame < m_totalFrames) { // Are we still doing an animation?

                    if (m_currentTick == 0 && m_frames[m_currentFrame].m_soundEffect != null) // If we have a sound effect to play this frame
                    {
                        m_frames[m_currentFrame].m_soundEffect.Play();
                    }

                    m_currentTick++;

                    if (!(m_currentTick < m_totalTicks))
                    {
                        m_currentTick = 0;
                        m_currentFrame++;
                        if (m_currentFrame < m_totalFrames)
                        {
                            m_totalTicks = m_frames[m_currentFrame].m_ticks;
                        }
                        else if (m_loop == true)             // Animation is finished, do we loop?
                        {
                            Reset();
                        }
                        else                                 // Animation is finished
                        {
                            Draw_State = DrawableState.Finished;
                            return;
                        }
                    }
                }

                if (m_dir == Direction.Right)
                {
                    Effects = SpriteEffects.None;
                }
                else
                {
                    Effects = SpriteEffects.FlipHorizontally;
                } 
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (Draw_State != DrawableState.Finished && m_texture != null)
            {  
                spritebatch.Draw(m_texture, new Vector2(X_Pos, Y_Pos), m_frames[m_currentFrame].m_source, Tint, Rotation, Origin, Scale, Effects, 0); 
            }
        }
        #endregion

        #region Public Methods

        public void SetFrame(int index, int ticks, SoundEffect sound)
        {
            if (index < m_totalFrames)
            {
                m_frames[index].m_ticks = ticks;
                m_frames[index].m_soundEffect = sound;
            }
            else
            {
                // Throw an exception
            }
        }

        public void BuildAnimation(Texture2D texture, int numRows, int numColumns, bool loop, int[] orderOfFrames)
        {
            int row, column;
            AnimFrame frame;

            m_loop = loop;
            m_texture = texture;
            m_frames = new List<AnimFrame>();
            m_totalFrames = orderOfFrames.Length;

            m_width = m_texture.Width / numColumns;
            m_height = m_texture.Height / numRows;

            for (int i = 0; i < m_totalFrames; i++)
            {
                frame = new AnimFrame();
                row = (int)((float)orderOfFrames[i] / (float)numColumns);
                column = orderOfFrames[i] % numColumns;

                frame.m_source = new Rectangle(m_width * column, m_height * row, m_width, m_height);
                m_frames.Add(frame);
            }
        }

        public override void Reset()
        {
            m_totalTicks = 0;
            m_currentTick = 0;
            m_currentFrame = 0;
            Draw_State = DrawableState.Active;
        }

        #endregion

        #region Private Methods
      
        #endregion
    }
}
