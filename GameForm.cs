using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using The_Game.Cutscenes;
using The_Game.Interfaces;
using The_Game.Menu;

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
        public GameplayStage Stage { get; private set; }

        private GameState Game { get; set; }
        private ICutscene CurrentCutscene { get; set; }
        private Timer Timer { get; set; }

        public int TimerInterval { get; private set; }
        public Size InternalSize { get; }

        private readonly HashSet<Keys> pressedKeys;
        private readonly HashSet<MouseButtons> pressedButtons;

        protected override void OnPaint(PaintEventArgs e)
        {
            var horizontalScalar = ClientSize.Width / (float)InternalSize.Width;
            var verticalScalar = ClientSize.Height / (float)InternalSize.Height; //BASIC internal coords 1800x1000
            var minScalar = Math.Min(horizontalScalar, verticalScalar);
            e.Graphics.ScaleTransform(minScalar, minScalar);
            if (verticalScalar <= horizontalScalar)
                e.Graphics.TranslateTransform((ClientSize.Width/verticalScalar - InternalSize.Width)/2, 0);
            else
                e.Graphics.TranslateTransform(0, (ClientSize.Height/horizontalScalar - InternalSize.Height)/2);
            DrawNormalizedForm(e.Graphics);
        }

        private void DrawNormalizedForm(Graphics g)
        {
            switch (Stage)
            {
                case GameplayStage.InGame:
                    DrawLevel(g);
                    break;
                case GameplayStage.InGameMenu:
                    //TODO
                    break;
                case GameplayStage.MainMenu:
                    break;
                case GameplayStage.Cutscene:
                    DrawCutscene(g);
                    break;
            }
        }

        private void DrawCutscene(Graphics g)
        {
            g.FillRectangle(CurrentCutscene.BackgroundBrush,
                new RectangleF(PointF.Empty, InternalSize));
            g.DrawString(CurrentCutscene.CurrentLine, new Font("Arial", 30), Brushes.Black, new PointF(
                InternalSize.Width / 10, InternalSize.Height / 6));
        }

        private void DrawLevel(Graphics g)
        {
            foreach (var entity in Game.Level.Entities)
            {
                var texture = Game.Level.Textures[entity][entity.Texture];
                texture.TranslateTransform(entity.Hitbox.Left, entity.Hitbox.Top);
                g.FillRectangle(texture, entity.Hitbox);
                texture.ResetTransform();
            }
        }

        public void StartNewGame()
        {
            if (Stage == GameplayStage.MainMenu)
                MoveNextStage();
            else
                throw new InvalidOperationException("User isn't on the main menu");
        }

        public void StartGame(GameState gameState = null)
        {
            Game = gameState ?? new GameState();
            Stage = GameplayStage.InGame;
            Timer = new Timer() { Interval = TimerInterval };
            Timer.Tick += OnTimerTick;
            Timer.Start();
        }

        public void StartCutscene(ICutscene cutscene = null)
        {
            CurrentCutscene = cutscene ?? new StartCutscene(this);
            Stage = GameplayStage.Cutscene;
            Controls.Clear();
            Timer = new Timer() { Interval = TimerInterval };
            Timer.Tick += (_, __) => { Invalidate(); };
            Timer.Start();
        }

        private void MoveNextStage()
        {
            switch (Stage)
            {
                case GameplayStage.Cutscene:
                    CurrentCutscene = null;
                    StartGame();
                    break;
                case GameplayStage.MainMenu:
                    StartCutscene();
                    break;
            }
        }

        public GameForm(int timerInterval)
        {
            ClientSize = new Size(1260, 700);
            SizeChanged += (sender, args) => Invalidate();
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);  // performance?
            BackColor = Color.Black;
            TimerInterval = timerInterval;
            InternalSize = new Size(1800, 1000);
            pressedKeys = new HashSet<Keys>();
            pressedButtons = new HashSet<MouseButtons>();
            Stage = GameplayStage.MainMenu;
            GameMainMenu.Initialize(this);
        }

        public bool IsPressed(Keys key) => pressedKeys.Contains(key);

        protected void OnTimerTick(object sender, EventArgs args)
        {
            Game.UpdateModel();
            Invalidate();
        }

        private void HandleKey(Keys key, bool down)
        {
            PlayerAction keyAction;
            if (down)
            {
                pressedKeys.Add(key);
                if (Game != null && PlayerActions.ActionByKey.TryGetValue(key, out keyAction))
                    Game.PlayerActions.Add(keyAction);
            }
            else
            {
                pressedKeys.Remove(key);
                if (Game != null && PlayerActions.ActionByKey.TryGetValue(key, out keyAction))
                    Game.PlayerActions.Remove(keyAction);
            }
        }

        private void HandleButton(MouseButtons button, bool down)
        {
            PlayerAction buttonAction;
            if (down)
            {
                pressedButtons.Add(button);
                if (Game != null && PlayerActions.ActionByButton.TryGetValue(button, out buttonAction))
                    Game.PlayerActions.Add(buttonAction);
            }
            else
            {
                pressedButtons.Remove(button);
                if (Game != null && PlayerActions.ActionByButton.TryGetValue(button, out buttonAction))
                    Game.PlayerActions.Remove(buttonAction);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, true);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            HandleButton(e.Button, false);

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            HandleButton(e.Button, false);
            if (Stage == GameplayStage.Cutscene)
            {
                if (!CurrentCutscene.MoveNextLine())
                    MoveNextStage();
            }
        }
    }
}
