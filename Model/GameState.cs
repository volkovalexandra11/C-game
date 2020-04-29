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
                GamePlayer.MobLevel = value;
                GamePlayer.Pos = level.StartPos;
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

        public GameState(Player player = null)
        {
            PlayerActions = new HashSet<MobAction>();
            GamePlayer = player ?? new Player(this);
            Level = new Level(this, new TestLevelBuilder().BuildData(this), GamePlayer);
            ShowCutscene(Cutscene.StartCutscene);
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
                mob.Update();
            }
        }
    }
}
