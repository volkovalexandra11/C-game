using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Levels;

namespace The_Game
{
    public class Background : IEntity
    {
        public bool Passable => true;
        public DrawingPriority Priority => DrawingPriority.Background;
        public RectangleF Hitbox { get; }
        public string Texture => "Background.png";
        public string[] Textures => new[] { Texture };

        public Background(Size levelSize)
        {
            var cornerPos = new Point(0, 0);
            Hitbox = new RectangleF(cornerPos, levelSize);
        }
    }

    public class Ground : IEntity
    {
        public bool Passable => false;
        public DrawingPriority Priority => DrawingPriority.SolidSurface;
        public RectangleF Hitbox { get; }
        public string Texture => "Ground.png";
        public string[] Textures => new[] { Texture };

        public Ground(Size size, Point pos)
        {
            Hitbox = new RectangleF(pos, size);
        }
    }

    public class CrumbledWall : IEntity
    {
        public bool Passable => false;
        public DrawingPriority Priority => DrawingPriority.SolidSurface;
        public RectangleF Hitbox { get; }
        public string Texture => "CrumbledWall.png";
        public string[] Textures => new[] { Texture };

        public CrumbledWall(Size size, Point pos)
        {
            Hitbox = new RectangleF(pos, size);
        }
    }
}
