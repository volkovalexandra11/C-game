using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace The_Game.Extensions
{
    public static class GraphicsExtensions
    {
        private static readonly int arrowSidesLen = 35;
        private static readonly Matrix3x2 arrowSidesRotLeft
            = Matrix3x2.CreateRotation(0.5f);
        private static readonly Matrix3x2 arrowSidesRotRight
            = Matrix3x2.CreateRotation(-0.5f);

        public static void DrawArrow(this Graphics g, Pen pen, Vector2 start, Vector2 end)
        {
            var dir = Vector2.Normalize(end - start) * arrowSidesLen;
            var leftSide = Vector2.Transform(dir, arrowSidesRotLeft);
            var rightSide = Vector2.Transform(dir, arrowSidesRotRight);
            g.DrawLine(pen, start.X, start.Y, end.X, end.Y);
            g.DrawLine(pen,
                end.X - leftSide.X, end.Y - leftSide.Y,
                end.X, end.Y);
            g.DrawLine(pen,
                end.X - rightSide.X, end.Y - rightSide.Y,
                end.X, end.Y);
        }
    }
}
