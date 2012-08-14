using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public class PongBall : Ball
    {
        #region Fields

        SoundEffect m_hitwall;
        SoundEffect m_paddlehit;

        #endregion

        #region Initialization

        public PongBall(CollisionData[] boundary, SoundEffect paddlehit, SoundEffect wallhit)
        {
            m_hitwall = wallhit;
            m_paddlehit = paddlehit;
            m_scrn_boundary = boundary;
            m_animList = new List<Drawable>();

            if (HelperUtils.GetRandomNumber(0.0, 10.0) < 5.01)
            {
                ResetBall(50.0f, -50.0f);
            }
            else
            {
                ResetBall(140.0f, 210.0f);
            }
        }

        #endregion

        #region Update

        public override void Update(CollisionData player1, CollisionData player2) 
        {
            if (Ball_State != BallState.DeadBall)
            {
                /* Update Position */
                X_Pos += (int)X_Vel;
                Y_Pos += (int)Y_Vel;

                /* Update current Animation */
                //m_currAnim.Update();

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
                if (Ball_State == BallState.Active)
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
                            m_hitwall.Play();
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
                            m_hitwall.Play();
                        }

                    }

                    // Paddle (Player 1)
                    else if (m_hit != Direction.Right && PongBall_Rect.Intersects(player1.m_rect)) // Cant hit p1 paddle twice in a row
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                             PongBall_Rect.Height, m_currAnimBData.m_color_data,
                             player1.m_transformation, player1.m_rect.Width,
                             player1.m_rect.Height, player1.m_color_data))
                        {
                            X_Vel = -X_Vel;
                            Ball_Dir = Direction.Right;
                            m_hit = Direction.Right;
                            m_paddlehit.Play();
                        }
                    }


                    // Paddle (Player 2)
                    else if (m_hit != Direction.Left && PongBall_Rect.Intersects(player2.m_rect)) // Cant hit p2 paddle twice in a row
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                            PongBall_Rect.Height, m_currAnimBData.m_color_data,
                            player2.m_transformation, player2.m_rect.Width,
                            player2.m_rect.Height, player2.m_color_data))
                        {
                            X_Vel = -X_Vel;
                            Ball_Dir = Direction.Left;
                            m_hit = Direction.Left;
                            m_paddlehit.Play();
                        }
                    }

                    // Left of boundary
                    else if (PongBall_Rect.Intersects(m_scrn_boundary[2].m_rect)) 
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                            PongBall_Rect.Height, m_currAnimBData.m_color_data,
                            m_scrn_boundary[2].m_transformation, m_scrn_boundary[2].m_rect.Width,
                            m_scrn_boundary[2].m_rect.Height, m_scrn_boundary[2].m_color_data))
                        {
                            Ball_State = BallState.DeadBall;
                        }
                    }

                    // Right of boundary
                    else if (PongBall_Rect.Intersects(m_scrn_boundary[3].m_rect)) 
                    {
                        if (HelperUtils.IntersectPixels(m_pongBallTransform, PongBall_Rect.Width,
                            PongBall_Rect.Height, m_currAnimBData.m_color_data,
                            m_scrn_boundary[3].m_transformation, m_scrn_boundary[3].m_rect.Width,
                            m_scrn_boundary[3].m_rect.Height, m_scrn_boundary[3].m_color_data))
                        {
                            Ball_State = BallState.DeadBall;  
                        }

                    }
                }
            }
        }

        #endregion
    }
}
