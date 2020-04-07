using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace The_Game
{
    static class Physics
    {
        private static Vector2 gravity = 10 * Vector2.UnitY;

        public static Vector2 GetVelocity(Vector2 velocity, float dt, Vector2? acceleration = null)
        {
            acceleration = acceleration ?? gravity;
            return velocity + (Vector2)acceleration * dt;
        }

        public static Vector2 GetPos(Vector2 pos, Vector2 velocity, float dt)
        {
            return pos + velocity * dt;
        }
    }
}
