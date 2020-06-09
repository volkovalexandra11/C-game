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
using System.Runtime.InteropServices;
using The_Game.Levels.ActiveLevels;

namespace The_Game.Model
{
    public class GameState
    {
        public Player GamePlayer { get; }
        public int UpdatesOnLvl { get; private set; }

        private Level level;

        private static readonly ILevelBuilder initialLevelBuilder
            = new IntroLevelBuilder();

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

        public GameState(Player player = null, ILevelBuilder startingLevelBuilder = null)
        {
            PlayerActions = new HashSet<MobAction>();
            GamePlayer = player ?? new Player(this);
            startingLevelBuilder = startingLevelBuilder ?? initialLevelBuilder;
            ChangeLevel(startingLevelBuilder);
            ShowCutscene(Cutscene.StartCutscene);
        }

        public void ChangeLevel(ILevelBuilder nextLevel)
        {
            UpdatesOnLvl = 0;
            var newLevel = new Level(this, nextLevel, GamePlayer);
            Level = newLevel;
            GamePlayer.HP = GamePlayer.MaxHP;
            GamePlayer.mobActions.Clear();
        }

        public void ShowCutscene(Cutscene cutscene)
        {
            CurrentCutscene = cutscene;
        }

        public void EndCutscene()
        {
            if (CurrentCutscene != Cutscene.RebellionOverCutscene)
                CurrentCutscene = Cutscene.None;
            else
                CurrentCutscene = Cutscene.CapitalReturnCutscene;
        }

        public void TeleportToLevelStart()
        {
            if (IsPaused)
                throw new InvalidOperationException();
            GamePlayer.Pos = Level.StartPos;
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
            unchecked { UpdatesOnLvl++; };
        }
    }
}
