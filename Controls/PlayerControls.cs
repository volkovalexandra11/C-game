using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Model;

namespace The_Game.Controls
{
    public class PlayerControls
    {
        private readonly Player player;
        private readonly HashSet<PlayerAction> playerActions;

        private const float gravAcceleration = 0.002f;
        private const float maxWalkingVelocity = 1.3f;

        private const float initialJumpVelocity = 0.5f;

        private bool IsStill => Math.Abs(player.HorGuidedVel) <= 1e-2f;
        private bool IsGoingRight => player.HorGuidedVel > 1e-2f;
        private bool IsGoingLeft => player.HorGuidedVel < -1e-2f;

        public float GetNewHorGuidedVel()
        {
            var isDrivenLeft = playerActions.Contains(PlayerAction.GoLeft);
            var isDrivenRight = playerActions.Contains(PlayerAction.GoRight);
            if (isDrivenLeft && !isDrivenRight)
            {
                if (IsStill || IsGoingRight)
                {
                    player.Dir = Direction.Left;
                    return -maxWalkingVelocity;
                }
            }
            else if (!isDrivenLeft && isDrivenRight)
            {
                if (IsStill || IsGoingLeft)
                {
                    player.Dir = Direction.Right;
                    return maxWalkingVelocity;
                }
            }
            return 0f;
        }

        public void Jump()
        {
            player.VerticalVel -= initialJumpVelocity;
            player.State = PlayerState.Jumping;
            player.JumpCount++;
        }

        public Vector2 GetNewPosition()
        {
            if (player.State == PlayerState.Jumping)
            {
                var newPos = player.Pos + 10 * player.Velocity;
                player.VerticalVel += 10 * gravAcceleration;
                return newPos;
            }
            return player.Pos + 10 * player.Velocity;
        }

        public void ProcessCollisions()
        {
            var playerOffset = Vector2.Zero;
            var collisions = Collisions.GetCollisions(player.Level, player);
            foreach (var solidEnt in collisions.Where(ent => !ent.Passable))
                playerOffset += Collisions.GetCollisionOffset(player.Hitbox, solidEnt.Hitbox);
            player.Pos += playerOffset;
            if (Collisions.IsStandingOnSurface(player.Level, player))
                if (player.State == PlayerState.Jumping && player.VerticalVel > 0)
                    player.State = PlayerState.Walking;
        }

        public PlayerControls(Player player)
        {
            this.player = player;
            playerActions = player.Game.PlayerActions;
        }
    }
}
