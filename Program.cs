using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Game
{
    public enum PlayerState
    {
        Walking,
        Jumping,
        OnLadder,
        Attacked
    }

    public class Player
    {
        public int PlayerX { get; private set; }
        public int PlayerY { get; private set; }
        public static int Height = 100;
        public static int Width = 100;
        private static int step = 10;

        public GameState Game { get; }

        public Player(GameState game)
        {
            Game = game;
        }

        public void Move(KeyEventArgs key)
        {
            switch (key.KeyCode)
            {
                case Keys.A:
                    if
                    PlayerX -= step;
                    break;
                case Keys.D:
                    PlayerX += step;
                    break;
            }
        }
    }

    public class GameState
    {
        public Player Player { get; }
        public Size LevelSize { get; private set; }

        public GameState(int lvlWidth, int lvlHeight)
        {
            Player = new Player(this);
            LevelSize = new Size(lvlWidth, lvlHeight);
        }
    }

    public class TestForm : Form
    {
        private GameState gs;
        public void Draw(PaintEventArgs args)
        {
            args.Graphics.FillRectangle(Brushes.Green, gs.Player.PlayerX, 300, Player.Width, Player.Height);
        }

        public TestForm(GameState gs)
        {
            this.gs = gs;
            Paint += (sender, args) =>
            {
                Draw(args);
            };
            KeyDown += (sender, args) =>
            {
                gs.Player.Move(args);
                Invalidate();
            };
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
       // [STAThread] ?
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            var gs = new GameState(100, 100);
            Application.Run(new TestForm(gs));
        }
    }
}
