using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using The_Game.Cutscenes;
using The_Game.Game_Panels;
using The_Game.Interfaces;
using The_Game.Model;

namespace The_Game
{
    public enum GameplayStage
    {
        MainMenu,
        Cutscene,
        InGame,
        InGameMenu
    }

    [System.ComponentModel.DesignerCategory("")]
    public partial class GameForm : Form
    {
        private GamePanel CurrentPanel { get; set; }

        private MainMenuPanel GameMainMenu { get; set; }
        private CutscenePanel CutsceneShown { get; set; }
        private LevelPanel LevelShown { get; set; }


        private GameplayStage Stage { get; set; }

        private GameState Game { get; set; }
        private Timer Timer { get; set; }

        private int TimerInterval { get; set; }
        public Size InternalSize { get; }

        private readonly HashSet<Keys> pressedKeys;
        private readonly HashSet<MouseButtons> pressedButtons;

        public Rectangle GetWorkingSpace()
        {
            var horizontalScalar = ClientSize.Width / (float)InternalSize.Width;
            var verticalScalar = ClientSize.Height / (float)InternalSize.Height;
            if (verticalScalar < horizontalScalar)
            {
                var spaceWidth = (int)(InternalSize.Width * verticalScalar);
                return new Rectangle(
                    new Point((ClientSize.Width - spaceWidth) / 2, 0),
                    new Size(spaceWidth, ClientSize.Height)
                );
            }
            var spaceHeight = (int)(InternalSize.Height * horizontalScalar);
            return new Rectangle(
                new Point(0, (ClientSize.Height - spaceHeight) / 2),
                new Size(ClientSize.Width, spaceHeight)
            );
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var horizontalScalar = ClientSize.Width / (float)InternalSize.Width;
            var verticalScalar = ClientSize.Height / (float)InternalSize.Height; //BASIC internal coords 1800x1000
            var minScalar = Math.Min(horizontalScalar, verticalScalar);
            e.Graphics.ScaleTransform(minScalar, minScalar);
            if (verticalScalar <= horizontalScalar)
                e.Graphics.TranslateTransform((ClientSize.Width/verticalScalar - InternalSize.Width) / 2, 0);
            else
                e.Graphics.TranslateTransform(0, (ClientSize.Height/horizontalScalar - InternalSize.Height) / 2);
            DrawNormalizedForm(e.Graphics);
        }

        private void DrawNormalizedForm(Graphics g)
        {
            CurrentPanel.Draw(g);
        }

        public GameForm(int timerInterval)
        {
            ClientSize = new Size(1260, 700);
            InternalSize = new Size(1800, 1000);

            DoubleBuffered = true;
            BackColor = Color.Black;

            TimerInterval = timerInterval;
            KeyPreview = true;

            GameMainMenu = new MainMenuPanel(this);
            CurrentPanel = GameMainMenu;
            Controls.Add(CurrentPanel);

            pressedKeys = new HashSet<Keys>();
            pressedButtons = new HashSet<MouseButtons>();
            Stage = GameplayStage.MainMenu;

            Timer = new Timer() { Interval = TimerInterval };
        }

        public void StartGame(GameState gameState = null)
        {
            Game = gameState ?? new GameState();
            Game.LevelChanged += ChangeLevelPanel;
            LevelShown = new LevelPanel(Game.Level);
            CurrentPanel = LevelShown;
            Stage = GameplayStage.InGame;
            Timer.Tick += OnTimerTick;
            Timer.Start();
        }

        public void StartCutscene(Cutscene cutscene)
        {
            CutsceneShown = CutsceneFactory.BuildCutscene(this, cutscene);
            CurrentPanel = CutsceneShown;
            Stage = GameplayStage.Cutscene;
            while (Controls.Count > 0) Controls[0].Dispose();
        }

        public void EndCutscene()
        {
            CutsceneShown = null;
            CurrentPanel = LevelShown;
            Stage = GameplayStage.InGame;
            Game.EndCutscene();
        }

        private void ChangeLevelPanel()
        {
            LevelShown = new LevelPanel(Game.Level);
            CurrentPanel = LevelShown;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        public bool IsPressed(Keys key) => pressedKeys.Contains(key);

        protected void OnTimerTick(object sender, EventArgs args)
        {
            if (Game.CurrentCutscene != Cutscene.None)
            {
                if (CutsceneShown == null || CutsceneShown.PanelCutscene != Game.CurrentCutscene)
                    StartCutscene(Game.CurrentCutscene);
            }
            else
                Game.UpdateModel();
            Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            pressedKeys.Remove(e.KeyCode);
            if (Game != null && MobActions.ActionByKey.TryGetValue(e.KeyCode, out var keyAction))
                Game.PlayerActions.Remove(keyAction);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            pressedKeys.Add(e.KeyCode);
            if (Game != null && MobActions.ActionByKey.TryGetValue(e.KeyCode, out var keyAction))
                Game.PlayerActions.Add(keyAction);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            pressedButtons.Remove(e.Button);
            if (Game != null && MobActions.ActionByButton.TryGetValue(e.Button, out var buttonAction))
                Game.PlayerActions.Add(buttonAction);

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            pressedButtons.Add(e.Button);
            if (Stage == GameplayStage.Cutscene)
            {
                if (!CutsceneShown.MoveNextLine())
                {
                    EndCutscene();
                }
            }
            else
            {
                if (Game != null && MobActions.ActionByButton.TryGetValue(e.Button, out var buttonAction))
                    Game.PlayerActions.Add(buttonAction);
            }
        }
    }
}
