using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Controls;
using The_Game.Levels;
//using System.Windows;
//using System.Windows.Forms;

namespace The_Game
{
    public enum Direction
    {
        Left,
        Right
    }

    public partial class Player : IMob
    {
        public bool Passable => true;
        public RectangleF Hitbox
            => new RectangleF(CornerPos, Size);
        public DrawingPriority Priority => DrawingPriority.Player;
        public string TextureDirectory => "Textures";
        public string Texture =>
            Dir == Direction.Left
                ? "KnightLeft.png"
                : "KnightRight.png";

        public string[] Textures => new[] { "KnightLeft.png", "KnightRight.png" };

        public Size Size
            => new Size(90, 210);
        public PointF CornerPos
            => new PointF(Pos.X - Size.Width / 2f, Pos.Y - Size.Height);

        public GameState Game { get; }
        public Level Level => Game.Level;
        public PlayerControls Controls { get; }

        public Vector2 Pos { get; set; }
        public PlayerState State { get; set; }
        public Direction Dir { get; set; }

        public float VerticalVel { get; set; }
        public float OwnHorVel { get; set; }
        public float HorGuidedVel { get; set; }
        public Vector2 Velocity => new Vector2(OwnHorVel + HorGuidedVel, VerticalVel);

        public Player(GameState game)
        {
            Game = game;
            State = PlayerState.Walking;
            Dir = Direction.Right;
            Controls = new PlayerControls(this);
        }

        public int JumpCount;

        public void Update()
        {
            ProcessKeys();
            UpdatePosition();
            Controls.ProcessCollisions();
            if (Game.PlayerActions.Contains(PlayerAction.Debug))
            { var a = "Debug!"; };
        }

        public void ProcessKeys()
        {
            if (Game.PlayerActions.Contains(PlayerAction.Jump))
            {
                switch (State)
                {
                    case PlayerState.Walking:
                        Controls.Jump();
                        break;
                    case PlayerState.OnLadder:
                        break; //TODO
                }
            }
            HorGuidedVel = Controls.GetNewHorGuidedVel();
        }

        public void UpdatePosition()
        {
            Pos = Controls.GetNewPosition();
        }
    }
}
