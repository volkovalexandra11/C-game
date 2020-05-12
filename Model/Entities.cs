using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Cutscenes;
using The_Game.Interfaces;
using The_Game.Levels;

namespace The_Game.Model
{
    public class Background : IEntity
    {
        public bool Passable => true;
        public DrawingPriority Priority => DrawingPriority.Background;
        public RectangleF Hitbox { get; }
        public string[] Textures => new[] { GetTexture() };
        public string GetTexture() => "Background";

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
        public string[] Textures => new[] { GetTexture() };
        public string GetTexture() => "Ground";

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
        public string[] Textures => new[] { GetTexture() };
        public string GetTexture() => "CrumbledWall";

        public CrumbledWall(Size size, Point pos)
        {
            Hitbox = new RectangleF(pos, size);
        }

        public override int GetHashCode() => base.GetHashCode();
    }

    public class Stump : IEntity, ITrigger
    {
        public bool Passable => true;
        public DrawingPriority Priority => DrawingPriority.SolidSurface;
        public RectangleF Hitbox { get; }
        public string[] Textures => new[] { GetTexture() };
        public string GetTexture() => "Stump";

        private GameState Game { get; }

        public bool Active { get; set; }

        public void Trigger()
        {
            Active = false;
            Game.ShowCutscene(Cutscene.KickTheStumpCutscene);
        }

        public Stump(GameState game, Size size, Point pos)
        {
            Game = game;
            Hitbox = new RectangleF(pos, size);
            Active = true;
        }

        public override int GetHashCode() => base.GetHashCode();
    }

    public class Ladder : IEntity
    {
        public bool Passable => false;

        public DrawingPriority Priority => DrawingPriority.SolidSurface;

        public RectangleF Hitbox { get; }

        public string[] Textures => new[] { GetTexture() };

        public string GetTexture() => "Ladder";

        public Ladder(Size size, PointF pos)
        {
            Hitbox = new RectangleF(pos, size);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
