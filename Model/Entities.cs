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
        public string GetTexture() => "Background.png";

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
        public string GetTexture() => "Ground.png";

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
        public string GetTexture() => "CrumbledWall.png";

        public CrumbledWall(Size size, Point pos)
        {
            Hitbox = new RectangleF(pos, size);
        }
    }

    public class Stump : IEntity, ITrigger
    {
        public bool Passable => true;
        public DrawingPriority Priority => DrawingPriority.SolidSurface;
        public RectangleF Hitbox { get; }
        public string[] Textures => new[] { GetTexture() };
        public string GetTexture() => "Stump.png";

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
    }
}
