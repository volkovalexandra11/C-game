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

namespace The_Game
{
    public enum PlayerAction
    {
        GoLeft,
        GoRight,
        GoUp,
        GoDown,
        Jump,
        PickUp,
        AttackMelee,
        AttackRanged,
        Debug
    }

    public class GameState
    {
        public Player Player { get; }
        private Level level;
        public Level Level
        {
            get => level;
            set
            {
                level = value;
                Player.Pos = level.StartPos;
            }
        }
        public readonly HashSet<PlayerAction> PlayerActions;

        public GameState()
        {
            PlayerActions = new HashSet<PlayerAction>();
            Player = new Player(this);
            Level = new Level(TestLevel.Entities, TestLevel.StartPos, Player);
        }

        public void UpdateModel()
        {
            foreach (var mob in Level.Mobs)
            {
                mob.Update();
            }
        }
    }
}
