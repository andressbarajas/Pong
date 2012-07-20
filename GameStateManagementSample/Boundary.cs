using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameStateManagement
{
    public class Boundary
    {
        public static Rectangle[] CreateBoundRects(Rectangle boundary) 
        {
            Rectangle[] temp = new Rectangle[6];

            temp[0].X = boundary.Left;
            temp[0].Y = boundary.Top - 1;
            temp[0].Width = boundary.Width;
            temp[0].Height = 1;

            temp[1].X = boundary.Left;
            temp[1].Y = boundary.Bottom;
            temp[1].Width = boundary.Width;
            temp[1].Height = 1;

            temp[2].X = boundary.Left - 1;
            temp[2].Y = boundary.Top;
            temp[2].Width = 1;
            temp[2].Height = boundary.Height;

            temp[3].X = boundary.Right;
            temp[3].Y = boundary.Top;
            temp[3].Width = 1;
            temp[3].Height = boundary.Height;

            temp[4].X = boundary.Left-62;
            temp[4].Y = boundary.Top;
            temp[4].Width = 1;
            temp[4].Height = boundary.Height;

            temp[5].X = boundary.Right + 62;
            temp[5].Y = boundary.Top;
            temp[5].Width = 1;
            temp[5].Height = boundary.Height;

            return temp;
        }
    }
}
