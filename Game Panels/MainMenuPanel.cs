using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Interfaces;

namespace The_Game.Game_Panels
{
    [System.ComponentModel.DesignerCategory("")]
    class MainMenuPanel : GamePanel
    {
        private readonly GameForm form;

        public MainMenuPanel(GameForm form)
        {
            this.form = form;
            var startButton = GetNewGameButton();
            startButton.Click += (_, __) => form.StartGame();
            var menuTable = new TableLayoutPanel();
            InitializeMenuTable(menuTable, startButton);
            Controls.Add(menuTable);
            Dock = DockStyle.Fill;
        }

        private void InitializeMenuTable(TableLayoutPanel menuTable, Button startButton)
        {
            menuTable.RowStyles.Clear();
            menuTable.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
            menuTable.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            menuTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            menuTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500));
            menuTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));

            menuTable.Dock = DockStyle.Fill;
            menuTable.Controls.Add(startButton, 1, 1);

            menuTable.ColumnCount = 3;
            menuTable.RowCount = 3;

            menuTable.BackgroundImage = TextureLoader.LoadImage("MainMenu");
            menuTable.BackgroundImageLayout = ImageLayout.Stretch;

            //menuTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            var workingSpace = form.GetWorkingSpace();
            Location = new Point(workingSpace.Left, workingSpace.Top);
            Size = new Size(workingSpace.Width, workingSpace.Height);
        }

        private static Button GetNewGameButton()
           => new PictureButton(
                "SngButton",
                "SngButtonHover",
                "SngButtonPressed"
           )
           { 
               Dock = DockStyle.Fill,
           };

        public override void Draw(Graphics g)
        {
        }
    }
}
