using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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

        public float DuckOneXVelocity
        {
            get { return m_pongballs[0].X_Vel; }
            set { m_pongballs[0].X_Vel = value; }
        }

        public float DuckOneYVelocity
        {
            get { return m_pongballs[0].Y_Vel; }
            set { m_pongballs[0].Y_Vel = value; }
        }

        public float DuckTwoXVelocity
        {
            get { return m_pongballs[1].X_Vel; }
            set { m_pongballs[1].X_Vel = value; }
        }

        public float DuckTwoYVelocity
        {
            get { return m_pongballs[1].Y_Vel; }
            set { m_pongballs[1].Y_Vel = value; }
        }

        public BallState DuckOneState
        {
            get { return m_pongballs[0].Ball_State; }
        }

        public BallState DuckTwoState
        {
            get { return m_pongballs[1].Ball_State; }
        }

        #endregion

        #region Fields

        /* * * * * * * * * * * TEXTURES * * * * * * * * * * * */

        private Texture2D m_duck_txt;
        private Texture2D m_flyingaway_txt;
        private Texture2D m_laughingdog_txt;
        private Texture2D m_duckcount_txt;
        private Texture2D m_countbg_txt;

        /* * * * * * * * * * * * SOUNDS * * * * * * * * * * * */

        private SoundEffect m_flapwing_snd;
        private SoundEffect m_doglaugh_snd;
        private SoundEffect m_duckquack_snd;

        /* * * * * * * * * * * ANIMATIONS * * * * * * * * * */

        private AnimatedSprite m_flyawayduck_anim_1 = new AnimatedSprite();  // Flyaway Duck inter 1
        private AnimatedSprite m_flyawayduck_anim_2 = new AnimatedSprite();  // Flyaway Duck inter 1
        private AnimatedSprite m_laughdog_anim = new AnimatedSprite();     // Laughing Dog Animation

        /* * * * * * * * * ANIMATION SCENE * * * * * * * * */

        private AnimationScene m_flyaway_scn = new AnimationScene();
        private AnimationScene m_flyaway_scn_inter = new AnimationScene();
        private AnimationScene m_dog_laugh_scn_inter = new AnimationScene();
        
        /* * * * * * * * * INTERMISSION * * * * * * * * * */

        private AnimationManager m_intermission1 = new AnimationManager();
        private AnimationManager m_intermission2 = new AnimationManager();
         
        private Sprite m_counter_spr;
        private ContentManager m_content;
        private Drawable[] m_ducks = new Drawable[10];
        private List<DuckHuntBall> m_pongballs = new List<DuckHuntBall>();

        private bool m_built1 = false;
        private bool m_built2 = false;
        private bool m_built_intermission = false;
        private bool m_finished_intermission = false;

        #endregion

        #region Initialization

        public DuckPopulation(ContentManager content, CollisionData[] boundary)
        {
            m_duck_count = 10;
            m_content = content;

            LoadContent();

            BuildAnimations();

            // Build duckhuntballs
            m_pongballs.Add(BuildDuck(boundary));
            m_pongballs.Add(BuildDuck(boundary));
  
        }

        #endregion

        #region Update and Draw

        public void UpdateCounter()
        {
            // current duck is blinking 
            // past ducks are red 
        }

        public void DrawCounter(SpriteBatch spritebatch)
        {
            m_counter_spr.Draw(spritebatch);

            for (int i = 0; i < m_ducks.Length; i++)
            {
                m_ducks[i].Draw(spritebatch);
            }
        }

        public void UpdateIntermission()
        {
           
            // Build intermission

            if (!m_built1 && ((m_pongballs[0].Ball_State == BallState.Active &&
                    m_pongballs[1].Ball_State == BallState.DeadBall) ||
                    (m_pongballs[0].Ball_State == BallState.DeadBall &&
                    m_pongballs[1].Ball_State == BallState.Active)))
            {
                // Make intermission with flyaway 
                if (m_pongballs[0].Ball_State == BallState.DeadBall) 
                {
                    m_flyawayduck_anim_1.X_Pos = m_pongballs[0].X_Pos;
                    m_flyawayduck_anim_1.Y_Pos = m_pongballs[0].Y_Pos;

                    m_flyaway_scn.AddAnimation(new DirXYMover(m_flyawayduck_anim_1, 0, -(m_pongballs[0].Y_Pos + m_flyawayduck_anim_1.Height), 2.0f)); //m_pongBall.Curr_Vel_Mag));                       
                }
                else
                {
                    m_flyawayduck_anim_1.X_Pos = m_pongballs[1].X_Pos;
                    m_flyawayduck_anim_1.Y_Pos = m_pongballs[1].Y_Pos;

                    m_flyaway_scn.AddAnimation(new DirXYMover(m_flyawayduck_anim_1, 0, -(m_pongballs[1].Y_Pos + m_flyawayduck_anim_1.Height), 2.0f)); //m_pongBall.Curr_Vel_Mag));
                }

                m_flyaway_scn.BuildScene(new int[1] { 0 });
                m_flyaway_scn.Scene_State = DrawableState.Active;

                m_intermission1.AddScene(m_flyaway_scn);
                m_intermission1.Build(new int[1] { 0 });
                m_intermission1.Manager_State = DrawableState.Active;

                // DeadBall is now in limbo mode
                m_built1 = true;
            }
            else if (!m_built2 && ((m_pongballs[0].Ball_State == BallState.DeadBall &&
                    m_pongballs[1].Ball_State == BallState.Limbo) ||
                    (m_pongballs[0].Ball_State == BallState.Limbo &&
                    m_pongballs[1].Ball_State == BallState.DeadBall)))
            {
                // Make intermission with flyaway duck and laughing dog
                if (m_pongballs[0].Ball_State == BallState.DeadBall)
                {
                    m_flyawayduck_anim_1.X_Pos = m_pongballs[0].X_Pos;
                    m_flyawayduck_anim_1.Y_Pos = m_pongballs[0].Y_Pos;

                    m_flyaway_scn_inter.AddAnimation(new DirXYMover(m_flyawayduck_anim_1, 0, -(m_pongballs[0].Y_Pos + m_flyawayduck_anim_1.Height), 2.0f)); //m_pongBall.Curr_Vel_Mag));                       
                }
                else
                {
                    m_flyawayduck_anim_1.X_Pos = m_pongballs[1].X_Pos;
                    m_flyawayduck_anim_1.Y_Pos = m_pongballs[1].Y_Pos;

                    m_flyaway_scn_inter.AddAnimation(new DirXYMover(m_flyawayduck_anim_1, 0, -(m_pongballs[1].Y_Pos + m_flyawayduck_anim_1.Height), 2.0f)); //m_pongBall.Curr_Vel_Mag));
                }

                /* Flyaway Duck */
                m_flyaway_scn_inter.BuildScene(new int[1] { 0 });
                m_flyaway_scn_inter.Scene_State = DrawableState.Active;

                /* Laughing Dog */
                m_laughdog_anim.X_Pos = ScreenManager.GraphicsDevice.Viewport.Width / 2 - m_laughdog_anim.Width / 2;
                m_laughdog_anim.Y_Pos = 530;
                m_dog_laugh_scn_inter.BuildScene(new int[3] { 0, 1, 2 });
                m_dog_laugh_scn_inter.Scene_State = DrawableState.Active;

                /* Intermission */
                m_intermission2.AddScene(m_flyaway_scn_inter);
                m_intermission2.AddScene(m_dog_laugh_scn_inter);
                m_intermission2.Build(new int[2] { 0, 1 });
                m_intermission2.Manager_State = DrawableState.Active;

                m_built2 = true;
            }

            m_intermission1.Update();
            m_intermission2.Update();

            if (!Intermission())
            {
                m_built1 = false;
                m_built2 = false;
                m_intermission1.ClearList();
                m_intermission2.ClearList();
            }
        }

        public void DrawIntermission(SpriteBatch spritebatch)
        {
            m_intermission1.Draw(spritebatch);
            m_intermission2.Draw(spritebatch);
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
            if (m_pongballs[0].Ball_State == BallState.DeadBall ||
                m_pongballs[0].Ball_State == BallState.Limbo)
            {
                m_pongballs[0].ResetBall();
                // Set location if need to
                m_pongballs[0].Ball_State = BallState.Active;
            }
            else if (m_pongballs[1].Ball_State == BallState.DeadBall ||
                     m_pongballs[1].Ball_State == BallState.Limbo)
            {
                m_pongballs[0].ResetBall();
                // Set location if need to
                m_pongballs[1].Ball_State = BallState.Active;
            }
        }

        public bool BallsDead()
        {
            return (m_pongballs[0].Ball_State == BallState.DeadBall || 
                    m_pongballs[1].Ball_State == BallState.DeadBall);
        }

        public bool BallsAlive()
        {
            return (m_pongballs[0].Ball_State != BallState.DeadBall && 
                    m_pongballs[0].Ball_State != BallState.Limbo || 
                    m_pongballs[1].Ball_State != BallState.DeadBall &&
                    m_pongballs[1].Ball_State != BallState.Limbo);
        }

        public bool Intermission()
        {
            return (m_intermission1.Manager_State == DrawableState.Active || m_intermission2.Manager_State == DrawableState.Active);
        }

        public bool GameOver()
        {
            return (m_duck_count == 0) ? true : false;
        }

        #endregion

        #region Private Methods

        private void LoadContent()
        {
             /*  LOAD SOUNDS AND TEXTURES */
            
            m_duck_txt = m_content.Load<Texture2D>("blackduck");
            m_flyingaway_txt = m_content.Load<Texture2D>("flyaway");
            m_laughingdog_txt = m_content.Load<Texture2D>("laughdog");
            m_duckcount_txt = m_content.Load<Texture2D>("duckcount");
           
            m_doglaugh_snd = m_content.Load<SoundEffect>("doglaugh");
            //m_flapwing_snd = m_content.Load<SoundEffect>("wingflaps");
            //m_duckquack_snd = m_content.Load<SoundEffect>("quack");
        }

        private void BuildAnimations()
        {
            //              Counter Stuff
            int x, y;
            Sprite temp;

            x = 500; 
            y = 600;

            // Create the ducks for the counter
            for (int i = 0; i < 10; i++)
            {
                temp = new Sprite();
                temp.Sprite_Texture = m_duckcount_txt;
                temp.X_Pos = x;
                temp.Y_Pos = y;
                temp.Draw_State = DrawableState.Active;
                m_ducks[i] = temp;
                x -= (m_duckcount_txt.Width + 2);
            }

            m_counter_spr.Sprite_Texture = m_countbg_txt;
            // Set location

            //            Intermission Stuff

            m_flyawayduck_anim_1.BuildAnimation(m_flyingaway_txt, 1, 3, true, new int[4] { 0, 1, 2, 1 });
            m_flyawayduck_anim_1.SetFrame(0, 8, null);
            m_flyawayduck_anim_1.SetFrame(1, 8, null);
            m_flyawayduck_anim_1.SetFrame(2, 8, null);
            m_flyawayduck_anim_1.SetFrame(3, 8, null);

            m_flyawayduck_anim_2.BuildAnimation(m_flyingaway_txt, 1, 3, true, new int[4] { 0, 1, 2, 1 });
            m_flyawayduck_anim_2.SetFrame(0, 8, null);
            m_flyawayduck_anim_2.SetFrame(1, 8, null);
            m_flyawayduck_anim_2.SetFrame(2, 8, null);
            m_flyawayduck_anim_2.SetFrame(3, 8, null);

            m_laughdog_anim.BuildAnimation(m_laughingdog_txt, 1, 2, true, new int[2] { 0, 1 });
            m_laughdog_anim.SetFrame(0, 6, null);
            m_laughdog_anim.SetFrame(1, 6, null);
            // Set location

            m_dog_laugh_scn_inter.AddAnimation(new DirXYMover(m_laughdog_anim, 0, -47, 1.4f), m_doglaugh_snd);
            m_dog_laugh_scn_inter.AddAnimation(new TimeOutDrawable(m_laughdog_anim, 60));
            m_dog_laugh_scn_inter.AddAnimation(new DirXYMover(m_laughdog_anim, 0, 47, 1.6f));

        }

        private DuckHuntBall BuildDuck(CollisionData[] boundary)
        {
            DuckHuntBall duckball;
            AnimatedSprite dscduck = new AnimatedSprite();
            AnimatedSprite ascduck = new AnimatedSprite();          

            dscduck.BuildAnimation(m_duck_txt, 1, 9, true, new int[16] { 3, 4, 5, 4, 3, 4, 5, 4, 3, 4, 5, 4, 3, 4, 5, 4 });
            dscduck.SetFrame(0, 5, m_flapwing_snd);
            dscduck.SetFrame(1, 5, null);
            dscduck.SetFrame(2, 5, m_flapwing_snd);
            dscduck.SetFrame(3, 5, null);
            dscduck.SetFrame(4, 5, m_flapwing_snd);
            dscduck.SetFrame(5, 5, null);
            dscduck.SetFrame(6, 5, m_flapwing_snd);
            dscduck.SetFrame(7, 5, null);
            dscduck.SetFrame(8, 5, m_flapwing_snd); //m_duckquack_snd);
            dscduck.SetFrame(9, 5, null);
            dscduck.SetFrame(10, 5, m_flapwing_snd);
            dscduck.SetFrame(11, 5, null);
            dscduck.SetFrame(12, 5, m_flapwing_snd);
            dscduck.SetFrame(13, 5, null);
            dscduck.SetFrame(14, 5, m_flapwing_snd);
            dscduck.SetFrame(15, 5, null);

            ascduck.BuildAnimation(m_duck_txt, 1, 9, true, new int[16] { 0, 1, 2, 1, 0, 1, 2, 1, 0, 1, 2, 1, 0, 1, 2, 1 });
            ascduck.SetFrame(0, 5, null);
            ascduck.SetFrame(1, 5, m_flapwing_snd);
            ascduck.SetFrame(2, 5, null);
            ascduck.SetFrame(3, 5, m_flapwing_snd);
            ascduck.SetFrame(4, 5, null);
            ascduck.SetFrame(5, 5, m_flapwing_snd);
            ascduck.SetFrame(6, 5, null);
            ascduck.SetFrame(7, 5, m_flapwing_snd);
            ascduck.SetFrame(8, 5, null); //m_duckquack_snd);
            ascduck.SetFrame(9, 5, m_flapwing_snd);
            ascduck.SetFrame(10, 5, null);
            ascduck.SetFrame(11, 5, m_flapwing_snd);
            ascduck.SetFrame(12, 5, null);
            ascduck.SetFrame(13, 5, m_flapwing_snd);
            ascduck.SetFrame(14, 5, null);
            ascduck.SetFrame(15, 5, m_flapwing_snd);

            duckball = new DuckHuntBall(0, 540, boundary);
            duckball.AddAnimation(dscduck);
            duckball.AddAnimation(ascduck);
            duckball.Ball_State = BallState.DeadBall;

            return duckball;
        }

        private void SetBlinkerToCurrentDuck()
        {
            //m_ducks[m_duck_count - 1] = new Blinker(m_ducks[m_duck_count - 1], frames);
        }

        #endregion
    }
}
