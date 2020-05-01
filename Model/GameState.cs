using System;
using System.Collections.Generic;
using The_Game.Levels;
using The_Game.Cutscenes;
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
                GamePlayer.MobLevel = value;
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
                IsPaused = (value != Cutscene.None);
                currentCutscene = value;
            }
        }

        public event Action LevelChanged;

        public GameState(Player player = null)
        {
            PlayerActions = new HashSet<MobAction>();
            GamePlayer = player ?? new Player(this);
            ChangeLevel(new TestLevelBuilder());
            ShowCutscene(Cutscene.StartCutscene);
        }

        public void ChangeLevel(ILevelBuilder nextLevel)
        {
            var name =nextLevel.GetType().ToString();
            var newLevel = new Level(this, nextLevel.BuildData(this), GamePlayer, name);
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
                mob.Update();
            }
        }
    }
}
