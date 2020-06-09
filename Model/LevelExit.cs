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
    class LevelExit : IEntity, ITrigger
    {
        protected GameState Game { get; }
        private ILevelBuilder NextLevel { get; }
        public bool Passable => true;
        public DrawingPriority Priority => DrawingPriority.Background;
        public RectangleF Hitbox { get; }
        public string[] Textures => new [] { GetTexture() };
        public string GetTexture() => "Empty";

        public bool Active { get ; set; }


        public virtual void Trigger()
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

    class LevelCutsceneExit : LevelExit
    {
        private Cutscene Cutscene { get; }

        public override void Trigger()
        {
            base.Trigger();
            Game.ShowCutscene(Cutscene);
        }

        public LevelCutsceneExit(
            GameState game, ILevelBuilder nextLevel,
            Size size, Point pos, Cutscene cutscene)
            : base(game, nextLevel, size, pos)
        {
            Cutscene = cutscene;
        }
    }
}
