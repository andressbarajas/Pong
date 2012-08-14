using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class DuckHuntBall : Ball
    {
        #region Initialization

        public DuckHuntBall(int x, int y, CollisionData []boundary)
        {
            m_init_x = x;
            m_init_y = y;
            m_scrn_boundary = boundary;
            m_animList = new List<Drawable>();

            if(HelperUtils.GetRandomNumber(0.0, 10.0) < 5.01) 
            {
                ResetBall(30.0f, 50.0f);
            } 
            else 
            {
                ResetBall(140.0f, 160.0f);
            }
        }

        #endregion

        #region Update

        public override void Update(CollisionData p1paddle, CollisionData p2paddle) 
        {
            if (Ball_State != BallState.DeadBall && Ball_State != BallState.Limbo)
            {
                /* Update Position */
                X_Pos += (int)X_Vel;
                Y_Pos += (int)Y_Vel;

                /* Update current Animation */
                m_currAnim.Update();

                m_currAnim.X_Pos = X_Pos;
                m_currAnim.Y_Pos = Y_Pos;
                m_currAnim.Scale = Ball_Scale;

                m_currAnimBData = new CollisionData((int)m_currAnim.X_Pos,
                                                    (int)m_currAnim.Y_Pos, 
                                                    m_currAnim.Texture,
                                                    m_currAnim.Sprite_Src_Rect,
                                                    m_currAnim.Origin,
                                                    m_currAnim.Scale,
                                                    m_currAnim.Rotation,
                                                    SpriteEffects.None);

                PongBall_Rect = m_currAnimBData.m_rect; 
                m_pongBallTransform = m_currAnimBData.m_transformation;

                /* Detect Collisions */
                if (Ball_State == BallState.Active) // No need to detect for collision if we are outofbounds
                {
                    // Top of boundary
                    if (m_hit != Direction.Top && PongBall_Rect.Intersects(m_scrn_boundary[0].m_rect))
                    {
                        // Check collision with person

                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                            PongBall_Rect.Height, m_currAnimBData.m_color_data,
                            m_scrn_boundary[0].m_transformation, m_scrn_boundary[0].m_rect.Width,
                            m_scrn_boundary[0].m_rect.Height, m_scrn_boundary[0].m_color_data))
                        {
                            Y_Vel = -Y_Vel;
                            m_hit = Direction.Top;
                        }
                    }

                    // Bottom of boundary
                    else if (m_hit != Direction.Bottom && PongBall_Rect.Intersects(m_scrn_boundary[1].m_rect)) // Cant hit bottom twice in row
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                            PongBall_Rect.Height, m_currAnimBData.m_color_data,
                            m_scrn_boundary[1].m_transformation, m_scrn_boundary[1].m_rect.Width,
                            m_scrn_boundary[1].m_rect.Height, m_scrn_boundary[1].m_color_data))
                        {
                            Y_Vel = -Y_Vel;
                            m_hit = Direction.Bottom;
                        }

                    }

                    // Paddle (DuckHuntPlayer 1)
                    else if (m_hit != Direction.Right && PongBall_Rect.Intersects(p1paddle.m_rect)) // Cant hit p1 paddle twice in a row
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                             PongBall_Rect.Height, m_currAnimBData.m_color_data,
                             p1paddle.m_transformation, p1paddle.m_rect.Width,
                             p1paddle.m_rect.Height, p1paddle.m_color_data))
                        {
                            X_Vel = -X_Vel;
                            Ball_Dir = Direction.Right;
                            m_hit = Direction.Right;
                            
                        }
                    }


                    // Paddle (DuckHuntPlayer 2)
                    else if (m_hit != Direction.Left && PongBall_Rect.Intersects(p2paddle.m_rect)) // Cant hit p2 paddle twice in a row
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                            PongBall_Rect.Height, m_currAnimBData.m_color_data,
                            p2paddle.m_transformation, p2paddle.m_rect.Width,
                            p2paddle.m_rect.Height, p2paddle.m_color_data))
                        {
                            X_Vel = -X_Vel;
                            Ball_Dir = Direction.Left;
                            m_hit = Direction.Left;
                        }
                    }

                    /* Out of bounds */

                    // Got past DuckHuntPlayer 1
                    else if (m_hit != Direction.Right && PongBall_Rect.Intersects(m_scrn_boundary[2].m_rect))
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                           PongBall_Rect.Height, m_currAnimBData.m_color_data,
                           m_scrn_boundary[2].m_transformation, m_scrn_boundary[2].m_rect.Width,
                           m_scrn_boundary[2].m_rect.Height, m_scrn_boundary[2].m_color_data))
                        {
                            m_currAnim.Effects = SpriteEffects.None;
                            Ball_State = BallState.OutofBounds;
                        }
                    }

                    // Got past DuckHuntPlayer 2
                    else if (m_hit != Direction.Left && PongBall_Rect.Intersects(m_scrn_boundary[3].m_rect))
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                           PongBall_Rect.Height, m_currAnimBData.m_color_data,
                           m_scrn_boundary[3].m_transformation, m_scrn_boundary[3].m_rect.Width,
                           m_scrn_boundary[3].m_rect.Height, m_scrn_boundary[3].m_color_data))
                        {
                            m_currAnim.Effects = SpriteEffects.FlipHorizontally;
                            Ball_State = BallState.OutofBounds;
                        }
                    }
                    /*
                    for (int i = 0; i < clouds.Length; i++)
                    {
                        if (clouds[i].m_used != true && PongBall_Rect.Intersects(clouds[i].m_collision_rect))
                        {
                            Y_Vel = -Y_Vel;
                            //clouds[i].m_duck_used = true;
                            m_hit = Direction.None;
                        }
                    }
                    */
                    if ((X_Vel > 0 && Y_Vel < 0) || (X_Vel < 0 && Y_Vel < 0))
                    {
                        m_currAnim = m_animList[1];
                    }
                    else
                    {
                        m_currAnim = m_animList[0];
                    }

                    if (Ball_Dir == Direction.Right)
                    {
                        m_currAnim.Effects = SpriteEffects.None;
                    }
                    else
                    {
                        m_currAnim.Effects = SpriteEffects.FlipHorizontally;
                    }
                }
                else if(Ball_State == BallState.OutofBounds) 
                {
                    if(PongBall_Rect.Intersects(m_scrn_boundary[4].m_rect))
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                           PongBall_Rect.Height, m_currAnimBData.m_color_data,
                           m_scrn_boundary[4].m_transformation, m_scrn_boundary[4].m_rect.Width,
                           m_scrn_boundary[4].m_rect.Height, m_scrn_boundary[4].m_color_data))
                        {
                            Ball_State = BallState.DeadBall;
                        }
                    }
                    else if (PongBall_Rect.Intersects(m_scrn_boundary[5].m_rect))
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                           PongBall_Rect.Height, m_currAnimBData.m_color_data,
                           m_scrn_boundary[5].m_transformation, m_scrn_boundary[5].m_rect.Width,
                           m_scrn_boundary[5].m_rect.Height, m_scrn_boundary[5].m_color_data))
                        {
                            Ball_State = BallState.DeadBall;
                        }
                    }
                    else if (PongBall_Rect.Intersects(m_scrn_boundary[6].m_rect))
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                           PongBall_Rect.Height, m_currAnimBData.m_color_data,
                           m_scrn_boundary[6].m_transformation, m_scrn_boundary[6].m_rect.Width,
                           m_scrn_boundary[6].m_rect.Height, m_scrn_boundary[6].m_color_data))
                        {
                            Ball_State = BallState.DeadBall;
                        }
                    }
                    else if (PongBall_Rect.Intersects(m_scrn_boundary[7].m_rect))
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                           PongBall_Rect.Height, m_currAnimBData.m_color_data,
                           m_scrn_boundary[7].m_transformation, m_scrn_boundary[7].m_rect.Width,
                           m_scrn_boundary[7].m_rect.Height, m_scrn_boundary[7].m_color_data))
                        {
                            Ball_State = BallState.DeadBall;
                        }
                    }
                    else if (PongBall_Rect.Intersects(m_scrn_boundary[8].m_rect))
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                           PongBall_Rect.Height, m_currAnimBData.m_color_data,
                           m_scrn_boundary[8].m_transformation, m_scrn_boundary[8].m_rect.Width,
                           m_scrn_boundary[8].m_rect.Height, m_scrn_boundary[8].m_color_data))
                        {
                            Y_Vel = -Y_Vel;
                            m_hit = Direction.Bottom;
                            
                        }
                    }
                    else if (PongBall_Rect.Intersects(m_scrn_boundary[9].m_rect))
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                           PongBall_Rect.Height, m_currAnimBData.m_color_data,
                           m_scrn_boundary[9].m_transformation, m_scrn_boundary[9].m_rect.Width,
                           m_scrn_boundary[9].m_rect.Height, m_scrn_boundary[9].m_color_data))
                        {
                            Y_Vel = -Y_Vel;
                            m_hit = Direction.Bottom;
                        }
                    }

                }

                m_currAnim.X_Pos = X_Pos;
                m_currAnim.Y_Pos = Y_Pos;
                m_currAnim.Scale = Ball_Scale;
            }
        }

        
         public void Update(Cloud[] clouds)
         {
            if (Ball_State == BallState.Active) 
            {   
                for (int i = 0; i < clouds.Length; i++)
                {
                    if (PongBall_Rect.Intersects(clouds[i].m_collision_rect)) // clouds[i].m_used != true &&
                    {
                        Console.WriteLine("Collision with cloud");
                        Y_Vel = -Y_Vel;
                        m_hit = Direction.None;
                    }
                }
            }
        }
         

        #endregion
    }
}
