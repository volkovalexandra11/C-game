using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace The_Game
{
    public class Player
    {
        public ILevel Level;
        public Vector2 Pos;
        private PlayerState state { get; set; }
        
        public static float Height = 100;
        public static float Width = 40;
        private static float accRate = 0.0000003f;
        private Vector2 horizontalAcc = accRate * Vector2.UnitX;
        private Vector2 velocity;
        //private static int step = 3;
        private static float maxVelocity = 0.0008f;
        private static float maxVelocitySq = maxVelocity * maxVelocity;

        public GameState Game { get; }

        public Player(GameState game)
        {
            Game = game;
            velocity = Vector2.Zero;
            state = PlayerState.Walking;
        }

        public void UpdateState(KeyEventArgs key)
        {
            switch (key.KeyCode)
            {
                case Keys.A:
                    velocity = Physics.GetVelocity(velocity, Game.Dt, -horizontalAcc);
                    break;
                case Keys.D:
                    velocity = Physics.GetVelocity(velocity, Game.Dt, horizontalAcc);
                    break;
                //default:
                //    if (state == PlayerState.Walking)
                //        velocity = Vector2.Zero;
                //    break;

            }
        }

        public void UpdatePosition()
        {
            if (velocity.LengthSquared() > maxVelocitySq)
                velocity *= maxVelocity / velocity.Length();
            Pos = Physics.GetPos(Pos, velocity, Game.Dt);
        }
    }
}
