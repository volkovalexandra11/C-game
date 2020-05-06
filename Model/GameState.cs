using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;
using System.Windows.Input;
using The_Game.Levels;
using The_Game.Cutscenes;
using The_Game.Model;
using The_Game.Mobs;
using The_Game.Interfaces;

namespace The_Game.Model
{
    public class GameState
    {
        public Player GamePlayer { get; }
        private Level level;
        public Level Level
        {
            get => level;
            set
            {
                level = value;
                GamePlayer.ChangeLevel(value);
                GamePlayer.Pos = level.StartPos;
                LevelChanged?.Invoke();
            }
        }
        public readonly HashSet<MobAction> PlayerActions;

        public bool IsPaused { get; private set; }

        private Cutscene currentCutscene;
        public Cutscene CurrentCutscene
        {
            get => currentCutscene;
            private set
            {
                IsPaused = value != Cutscene.None;
                currentCutscene = value;
            }
        }

        public event Action LevelChanged;

        public GameState(Player player = null)
        {
            PlayerActions = new HashSet<MobAction>();
            GamePlayer = player ?? new Player(this);
            //ChangeLevel(new TestLevelBuilder());
            ChangeLevel(new MobLevelBuilder());
            ShowCutscene(Cutscene.StartCutscene);
        }

        public void ChangeLevel(ILevelBuilder nextLevel)
        {
            var newLevel = new Level(this, nextLevel, GamePlayer);
            Level = newLevel;
        }

        public void ShowCutscene(Cutscene cutscene)
        {
            CurrentCutscene = cutscene;
        }

        public void EndCutscene()
        {
            CurrentCutscene = Cutscene.None;
        }

        public void UpdateModel()
        {
            if (IsPaused)
                throw new InvalidOperationException("GameState is paused! Cannot update!");
            foreach (var mob in Level.Mobs)
            {
                if (!mob.IsDead) mob.Update();
            }
            foreach (var deadMob in Level.Mobs.Where(mob => mob.IsDead).ToList())
            {
                Level.RemoveMob(deadMob);
            }
        }
    }
}
