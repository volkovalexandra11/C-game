using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Game
{
    [System.ComponentModel.DesignerCategory("")]
    public partial class GameForm : Form
    {
        private GameState gs { get; set; }
        private readonly Timer timer;
        private readonly HashSet<Keys> pressedKeys;
        public Size InternalSize { get; }

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
            gs.Level.Draw(g);
        }

        public GameForm(GameState gs)
        {
            this.gs = gs;
            DoubleBuffered = true;
            BackColor = Color.Black;
            pressedKeys = new HashSet<Keys>();
            InternalSize = new Size(1800, 1000);
            timer = new Timer() { Interval = (int)(gs.Dt / 1000) };
            KeyDown += (sender, args) =>
            {
                gs.Player.UpdateState(args);
            };
            SizeChanged += (sender, args) => { Invalidate(); };
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        protected void OnTimerTick(object sender, EventArgs args)
        {
            gs.Player.UpdatePosition();
            Invalidate();
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

        private void HandleKey(Keys e, bool down)
        {
            if (down)
                pressedKeys.Add(e);
            else if (pressedKeys.Contains(e))
                pressedKeys.Remove(e);
        }
    }
}
