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
        public string Texture { get
            {
                return Dir == Direction.Left
                    ? "KnightLeft.png"
                    : "KnightRight.png";
            }
        }
        public string[] Textures => new[] { "KnightLeft.png", "KnightRight.png" };

        public Size Size
            => new Size(90, 210);
        public PointF CornerPos
            => new PointF(Pos.X - Size.Width / 2f, Pos.Y - Size.Height);

        public GameState Game { get; }
        public ILevel Level;
        public PlayerControls Controls { get; }

        public Vector2 Pos { get; set; }
        public PlayerState State { get; set; }
        public Direction Dir { get; set; }

        public float VerticalVel { get; set; }
        public float OwnHorVel { get; set; }
        public float HorGuidedVel { get; set; }
        public Vector2 Velocity => new Vector2(OwnHorVel + HorGuidedVel, VerticalVel);

        public Player(GameState game, Vector2 startPos)
        {
            Game = game;
            State = PlayerState.Walking;
            Pos = startPos;
            Dir = Direction.Right;
            Controls = new PlayerControls(this);
        }

        public int JumpCount;

        public void Update()
        {
            ProcessKeys();
            UpdatePosition();
            Controls.ProcessCollisions();
            if (Game.PressedKeys.Contains(Keys.F))
            { var a = "Dedug!"; };
        }

        public void ProcessKeys()
        {
            if (Game.PressedKeys.Contains(Keys.Space))
            {
                if (State == PlayerState.Walking)
                { 
                    Controls.Jump();
                }
                else if (State == PlayerState.OnLadder)
                {
                } //TODO
            }
            HorGuidedVel = Controls.GetNewHorGuidedVel();
        }

        public void UpdatePosition()
        {
            Pos = Controls.GetNewPosition();
        }
    }
}
