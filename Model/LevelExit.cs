using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Interfaces;
using The_Game.Levels;

namespace The_Game.Model
{
    class LevelExit : IEntity, ITrigger
    {
        private GameState Game { get; }
        private ILevelBuilder NextLevel { get; }
        public bool Passable => true;
        public DrawingPriority Priority => DrawingPriority.Background;
        public RectangleF Hitbox { get; }
        public string[] Textures => new [] { GetTexture() };
        public string GetTexture() => "Empty.png";

        public bool Active { get ; set; }


        public void Trigger()
        {
            Game.ChangeLevel(NextLevel);
        }

        public LevelExit(
            GameState game, ILevelBuilder nextLevel,
            Size size, Point pos)
        {
            Game = game;
            NextLevel = nextLevel;
            Hitbox = new RectangleF(pos, size);
            Active = true;
        }
    }
}
