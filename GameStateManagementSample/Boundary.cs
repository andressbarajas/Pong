using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PongaThemes
{
    public class Boundary
    {
        public static Rectangle[] CreateBoundRects(Rectangle boundary) 
        {
            Rectangle[] temp = new Rectangle[10];

            temp[0].X = boundary.Left;
            temp[0].Y = boundary.Top - 81;
            temp[0].Width = boundary.Width;
            temp[0].Height = 80;

            temp[1].X = boundary.Left;
            temp[1].Y = boundary.Bottom;
            temp[1].Width = boundary.Width;
            temp[1].Height = 80;

            temp[2].X = boundary.Left - 21;
            temp[2].Y = boundary.Top;
            temp[2].Width = 20;
            temp[2].Height = boundary.Height;

            temp[3].X = boundary.Right + 1;
            temp[3].Y = boundary.Top;
            temp[3].Width = 20;
            temp[3].Height = boundary.Height;

            temp[4].X = boundary.Left - 62 - 63;
            temp[4].Y = boundary.Top - 62;
            temp[4].Width = 63;
            temp[4].Height = boundary.Height + 124;

            temp[5].X = boundary.Right + 62;
            temp[5].Y = boundary.Top - 62;
            temp[5].Width = 63;
            temp[5].Height = boundary.Height + 124;

            temp[6].X = boundary.Left - 62;
            temp[6].Y = boundary.Top - 81;
            temp[6].Width = 62;
            temp[6].Height = 80;

            temp[7].X = boundary.Right;
            temp[7].Y = boundary.Top - 81;
            temp[7].Width = 62;
            temp[7].Height = 80;

            temp[8].X = boundary.Left - 62;
            temp[8].Y = boundary.Bottom;
            temp[8].Width = 62;
            temp[8].Height = 80;

            temp[9].X = boundary.Right;
            temp[9].Y = boundary.Bottom;
            temp[9].Width = 62;
            temp[9].Height = 80;

            return temp;
        }
    }
}
