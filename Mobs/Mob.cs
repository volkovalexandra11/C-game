﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Drawing;
using The_Game.Model;
using The_Game.Levels;
using The_Game.Interfaces;

namespace The_Game.Mobs
{
    public class Mob : IMob
    {
        public bool Passable { get; }
        public DrawingPriority Priority { get; }
        public RectangleF Hitbox => new RectangleF(CornerPos, MobSize);
        public string[] Textures { get; }
        public string GetTexture() => getTexture();
        protected Func<string> getTexture;

        private readonly HashSet<MobAction> mobActions;

        private const float GravAcceleration = 0.002f;
        private const float MaxWalkingVelocity = 1.3f;

        private const float InitialJumpVelocity = 0.5f;

        private bool IsStill => Math.Abs(HorGuidedVel) <= 1e-2f;
        private bool IsGoingRight => HorGuidedVel > 1e-2f;
        private bool IsGoingLeft => HorGuidedVel < -1e-2f;

        protected GameState Game { get; }

        public Level MobLevel;
        public Vector2 Pos { get; set; }

        protected MobState State { get; set; }
        protected Direction Dir { get; set; }

        public Size MobSize { get; }
        public PointF CornerPos
            => new PointF(Pos.X - MobSize.Width / 2f, Pos.Y - MobSize.Height);

        protected float VerticalVel { get; set; }
        protected float OwnHorVel { get; set; }
        protected float HorGuidedVel { get; set; }
        protected Vector2 Velocity => new Vector2(OwnHorVel + HorGuidedVel, VerticalVel);

        public virtual void Update()
        {
            ProcessActions();
            UpdatePosition();
            ProcessCollisions();
            if (Game.PlayerActions.Contains(MobAction.Debug))
            { Game.PlayerActions.Remove(MobAction.Debug); };
        }

        private float GetNewHorGuidedVel()
        {
            var isDrivenLeft = mobActions.Contains(MobAction.GoLeft);
            var isDrivenRight = mobActions.Contains(MobAction.GoRight);
            if (isDrivenLeft && !isDrivenRight)
            {
                if (IsStill || IsGoingRight)
                {
                    Dir = Direction.Left;
                    return -MaxWalkingVelocity;
                }
            }
            else if (!isDrivenLeft && isDrivenRight)
            {
                if (IsStill || IsGoingLeft)
                {
                    Dir = Direction.Right;
                    return MaxWalkingVelocity;
                }
            }
            return 0f;
        }

        private void Jump()
        {
            VerticalVel -= InitialJumpVelocity;
            State = MobState.Jumping;
        }

        private Vector2 GetNewPosition()
        {
            if (State == MobState.Jumping)
            {
                var newPos = Pos + 10 * Velocity;
                VerticalVel += 10 * GravAcceleration;
                return newPos;
            }
            return Pos + 10 * Velocity;
        }

        private void ProcessCollisions()
        {
            var mobOffset = Vector2.Zero;
            var collisions = Collisions.GetCollisions(MobLevel, this);
            foreach (var ent in collisions)
            {
                if (!ent.Passable)
                    mobOffset += Collisions.GetCollisionOffset(Hitbox, ent.Hitbox);
                ProcessCollision(ent);
            }
            Pos += mobOffset;
            if (Collisions.IsStandingOnSurface(MobLevel, this))
                if (State == MobState.Jumping && VerticalVel > 0)
                    State = MobState.Walking;
        }

        protected virtual void ProcessCollision(IEntity otherEnt)
        {
        }

        private void UpdatePosition()
        {
            Pos = GetNewPosition();
        }
        
        private void ProcessActions()
        {
            if (Game.PlayerActions.Contains(MobAction.Jump))
            {
                if (State == MobState.Walking)
                { 
                    Jump();
                }
                else if (State == MobState.OnLadder)
                {
                } //TODO
            }
            HorGuidedVel = GetNewHorGuidedVel();
        }

        public Mob(GameState game,
            bool isPassable, Size size,
            DrawingPriority priority,
            string[] textures, Vector2 startPos)
        {
            Game = game;
            Passable = isPassable;
            MobSize = size;
            Priority = priority;
            Textures = textures;
            MobLevel = game.Level;
            mobActions = priority == DrawingPriority.Player ? game.PlayerActions : new HashSet<MobAction>();
            Pos = startPos;
        }
    }
}
