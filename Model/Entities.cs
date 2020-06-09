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
    public abstract class Background : IEntity
    {
        public bool Passable => true;
        public DrawingPriority Priority => DrawingPriority.Background;
        public RectangleF Hitbox { get; }
        public string[] Textures => new[] { GetTexture() };
        public virtual string GetTexture() => null;

        public Background(Size levelSize)
        {
            var cornerPos = new Point(0, 0);
            Hitbox = new RectangleF(cornerPos, levelSize);
        }
    }

    
    public class TutorialBackground : Background
    {
        public override string GetTexture() => "TutorialBackground";

        public TutorialBackground(Size levelSize)
            : base(levelSize)
        {
        }
    }

    public class CountrysideBackground : Background
    {
        public override string GetTexture() => "Background";

        public CountrysideBackground(Size levelSize)
            : base(levelSize)
        {
        }
    }

    public class RuinBackground : Background
    {
        public override string GetTexture() => "RuinBackground"; // TODO

        public RuinBackground(Size levelSize)
            : base(levelSize)
        {
        }
    }

    public class CaveBackground : Background
    {
        public override string GetTexture() => "CaveBackground"; // TODO

        public CaveBackground(Size levelSize)
            : base(levelSize)
        {
        }
    }

    public class CastleBackground : Background
    {
        public override string GetTexture() => "Castle";

        public CastleBackground(Size levelSize)
            : base(levelSize)
        {
        }
    }

    public abstract class SolidSurface : IEntity
    {
        public bool Passable => false;
        public DrawingPriority Priority => DrawingPriority.SolidSurface;
        public RectangleF Hitbox { get; }
        public virtual string[] Textures => new[] { GetTexture() };
        public virtual string GetTexture() => null;

        public SolidSurface(Size size, Point pos)
        {
            Hitbox = new RectangleF(pos, size);
        }
    }

    public class InvisibleWall : SolidSurface
    {
        public override string GetTexture() => "Empty";

        public InvisibleWall(Size size, Point pos)
            : base(size, pos)
        {
        }
    }

    public class Ground : SolidSurface
    {
        public override string GetTexture() => "Ground";

        public Ground(Size size, Point pos)
            : base(size, pos)
        {
        }
    }

    public class CaveFloor : SolidSurface
    {
        public override string GetTexture() => "CaveFloor";

        public CaveFloor(Size size, Point pos)
            : base(size, pos)
        {
        }
    }

    public class UndergroundWall : SolidSurface
    {
        public override string GetTexture() => "UndergroundWall";

        public UndergroundWall(Size size, Point pos)
            : base(size, pos)
        {
        }
    }

    public class CrumbledWall : SolidSurface
    {
        public override string GetTexture() => "CrumbledWall";

        public CrumbledWall(Size size, Point pos)
            : base(size, pos)
        {
        }
    }

    public class MagicWallOdd : IEntity
    {
        public bool Passable => IsntActive;
        public DrawingPriority Priority => DrawingPriority.SolidSurface;
        public RectangleF Hitbox { get; }
        public string[] Textures => new[] { "CrumbledWall", "Empty" };
        public string GetTexture() =>
            !IsntActive ? "CrumbledWall" : "Empty";

        private bool IsntActive => Game.UpdatesOnLvl % 160 <= 80;

        private GameState Game { get; }

        public MagicWallOdd(GameState game, Size size, Point pos)
        {
            Game = game;
            Hitbox = new RectangleF(pos, size);
        }
    }

    public class MagicWallEven : IEntity
    {
        public bool Passable => !IsActive;
        public DrawingPriority Priority => DrawingPriority.SolidSurface;
        public RectangleF Hitbox { get; }
        public string[] Textures => new[] { "CrumbledWall", "Empty" };
        public string GetTexture() =>
            IsActive ? "CrumbledWall" : "Empty";

        private bool IsActive => Game.UpdatesOnLvl % 160 <= 80;

        private GameState Game { get; }

        public MagicWallEven(GameState game, Size size, Point pos)
        {
            Game = game;
            Hitbox = new RectangleF(pos, size);
        }
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
    }

    public class Ladder : IEntity
    {
        public bool Passable => true;

        public DrawingPriority Priority => DrawingPriority.SolidSurface;

        public RectangleF Hitbox { get; }

        public string[] Textures => new[] { GetTexture() };

        public string GetTexture() => "Ladder";

        public Ladder(Size size, Point pos)
        {
            Hitbox = new RectangleF(pos, size);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
