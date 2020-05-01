using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Interfaces;

namespace The_Game.Menu
{
    static class GameMainMenu
    {
        public static string TextureFolder => "Textures";
        public static string BackgroundTexture => "MainMenu.png";

        public static void Initialize(GameForm form)
        {
            var startButton = GetStartButton();
            startButton.Click += (_, __) => form.StartNewGame();
            var menuTable = new TableLayoutPanel();
            InitializeMenuTable(menuTable, startButton);
            form.Controls.Add(menuTable);
        }

        private static void InitializeMenuTable(TableLayoutPanel menuTable, Button startButton)
        {
            menuTable.RowStyles.Clear();
            menuTable.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            menuTable.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            menuTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            menuTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500));
            menuTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            menuTable.Dock = DockStyle.Fill;
            menuTable.Controls.Add(startButton, 1, 1);
            menuTable.BackgroundImage = Image.FromFile(Path.Combine(TextureFolder, BackgroundTexture));
            menuTable.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private static Button GetStartButton()
        {
            return new Button()
            {
                Text = "Start new game",
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(500, 100)
            };
        }
    }
}
