using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Game
{
    public class Player
    {
        public int PlayerX { get; private set; }
        public int PlayerY { get; private set; }
        public static int Height = 100;

        public GameState Game { get; }

        public Player(GameState game)
        {
            Game = game;
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
        public TestForm(GameState gs)
        {
            KeyDown +=
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
            Application.Run(new TestForm());
        }
    }
}
