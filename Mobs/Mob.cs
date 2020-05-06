using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Model;
using The_Game.Levels;
using The_Game.Interfaces;
using The_Game.MobAI;
using NUnit.Framework.Interfaces;
using System.Runtime.CompilerServices;

namespace The_Game.Mobs
{
    public class Mob : IMob
    {
        public int HP { get; set; }
        public int MaxHP { get; }

        public int Damage { get; }
        public int AttackRange { get; }

        public bool IsDead => HP <= 0;

        public bool Passable { get; }
        public DrawingPriority Priority { get; }
        public RectangleF Hitbox => new RectangleF(CornerPos, MobSize);
        public virtual string[] Textures { get; }
        public virtual string GetTexture() => string.Empty;
        public virtual Dictionary<string, Size> TextureSizes { get; }
        public virtual Dictionary<string, Point> TextureMobPos { get; }

        private readonly HashSet<MobAction> mobActions;

        private const float proximityConst = 400;

        private const float gravAcceleration = 0.002f;
        private const float maxWalkingVelocity = 1.3f;

        private const float initialJumpVelocity = 0.5f;

        private bool IsStill => Math.Abs(HorGuidedVel) <= 1e-2f;
        private bool IsGoingRight => HorGuidedVel > 1e-2f;
        private bool IsGoingLeft => HorGuidedVel < -1e-2f;

        protected GameState Game { get; }

        protected Level MobLevel;
        public Vector2 Pos { get; set; }

        public PathNode<Vector2> PlannedPath { get; set; }

        protected MobState State { get; set; }

        private bool DirChangedThisUpdate { get; set; }
        private Direction dir;
        protected Direction Dir
        {
            get => dir;
            set
            {
                if (value != dir) DirChangedThisUpdate = true;
                dir = value;
            }
        }

        public Size MobSize { get; }
        public PointF CornerPos
            => new PointF(Pos.X - MobSize.Width / 2f, Pos.Y - MobSize.Height);

        protected float VerticalVel { get; set; }
        protected float OwnHorVel { get; set; }
        protected float HorGuidedVel { get; set; }
        protected Vector2 Velocity => new Vector2(OwnHorVel + HorGuidedVel, VerticalVel);

        protected const int MinChangeDirTimeUpdates = 10;
        protected int UpdatesSinceChangeOfDir;

        protected const int AttackTimeUpdates = 30;
        protected int UpdatesSinceLastAttack;

        public RectangleF AttackZone
        {
            get
            {
                var hitbox = Hitbox;
                return new RectangleF(
                         Dir == Direction.Left
                             ? hitbox.Left - AttackRange
                             : hitbox.Right,
                         hitbox.Top,
                         AttackRange, hitbox.Height);
            }
        }

        protected bool IsAttacking => UpdatesSinceLastAttack < AttackTimeUpdates;

        private event Action EndUpdate;

        public Vector2 GetClosestWaypoint()
        {
            if (MobLevel.WPReverseGraph == null) throw new InvalidOperationException();
            return MobLevel.Waypoints
                .MinBy(wp => Vector2.DistanceSquared(Pos, wp));
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
                    return -maxWalkingVelocity;
                }
            }
            else if (!isDrivenLeft && isDrivenRight)
            {
                if (IsStill || IsGoingLeft)
                {
                    Dir = Direction.Right;
                    return maxWalkingVelocity;
                }
            }
            return 0f;
        }

        private void Jump()
        {
            VerticalVel -= initialJumpVelocity;
            State = MobState.Jumping;
        }

        private void TryAttackMelee()
        {
            if (!IsDead && !IsAttacking) AttackMelee();
        }

        private void AttackMelee()
        {
            mobActions.Remove(MobAction.AttackMelee);
            UpdatesSinceLastAttack = 0;
            var hitbox = Hitbox;
            var dmgZone = AttackZone;
            foreach (var damagedMob in Collisions.GetCollisions(MobLevel, dmgZone)
                .Where(ent => ent is Mob)
                .Select(ent => (Mob)ent)
            )
            {
                damagedMob.TakeDamage(Damage);
            }
        }

        private void TakeDamage(int dmg)
        {
            HP -= dmg;
        }

        private Vector2 GetNewPosition()
        {
            if (State == MobState.Jumping)
            {
                var newPos = Pos + 10 * Velocity;
                VerticalVel += 10 * gravAcceleration;
                return newPos;
            }
            return Pos + 10 * Velocity;
        }

        private void ProcessCollisions()
        {
            var mobOffset = Vector2.Zero;
            var collisions = Collisions.GetCollisions(MobLevel, Hitbox);
            foreach (var ent in collisions)
            {
                if (!ent.Passable)
                    mobOffset += Collisions.GetCollisionOffset(Hitbox, ent.Hitbox);
                ProcessCollision(ent);
            }
            Pos += mobOffset;
            if (Collisions.IsStandingOnSurface(MobLevel, this))
            {
                if (State == MobState.Jumping && VerticalVel > 0)
                    State = MobState.Walking;
            }
            else
            {
                if (State != MobState.Jumping)
                    State = MobState.Jumping;
            }
        }

        protected virtual void ProcessCollision(IEntity otherEnt)
        {
        }

        private void UpdatePosition()
        {
            Pos = GetNewPosition();
        }

        public virtual void Update()
        {
            if (PlannedPath != null) FillActions();
            ProcessActions();
            UpdatePosition();
            ProcessCollisions();

            if (mobActions.Contains(MobAction.Debug))
            { mobActions.Remove(MobAction.Debug); };

            EndUpdate();
        }

        private void FillActions()
        {
            var distToPathWPSq = (Pos - PlannedPath.Position).LengthSquared();
            if (distToPathWPSq < proximityConst) PlannedPath = PlannedPath.Previous;
            mobActions.Clear();

            var distToPlayer = (Pos - Game.GamePlayer.Pos).Length();
            var distToPlayerHitbox = AttackRange + Game.GamePlayer.MobSize.Width / 2 + MobSize.Width / 2;
            if (distToPlayer < distToPlayerHitbox)
                AttackPlayer();
            else
            {
                ChooseWalkingDirection();
            }
        }

        private void AttackPlayer()
        {
            var attackDir = Game.GamePlayer.Pos.X < Pos.X
                ? Direction.Left
                : Direction.Right;
            Dir = attackDir;
            TryAttackMelee();
        }

        private void ChooseWalkingDirection()
        {
            if (PlannedPath == null || UpdatesSinceChangeOfDir < MinChangeDirTimeUpdates)
                return;
            if (PlannedPath.Position.X > Pos.X) mobActions.Add(MobAction.GoRight);
            if (PlannedPath.Position.X < Pos.X) mobActions.Add(MobAction.GoLeft);
        }


        private void ProcessActions()
        {
            if (mobActions.Contains(MobAction.Jump))
            {
                if (State == MobState.Walking)
                {
                    Jump();
                }
                else if (State == MobState.OnLadder)
                {
                } //TODO
            }
            if (mobActions.Contains(MobAction.AttackMelee))
                TryAttackMelee();
            HorGuidedVel = GetNewHorGuidedVel();
        }

        private void ProcessDirectionChangeTimer()
        {
            if (DirChangedThisUpdate) UpdatesSinceChangeOfDir = 0;
            else UpdatesSinceChangeOfDir++;
            DirChangedThisUpdate = false;
        }

        private void ProcessAttackTimer()
        {
            UpdatesSinceLastAttack++;
        }

        private void SetUpEndUpdateEvent()
        {
            EndUpdate += ProcessDirectionChangeTimer;
            EndUpdate += ProcessAttackTimer;
        }

        public Mob(GameState game, Level level,
            bool isPassable, Size size,
            DrawingPriority priority,
            Vector2 startPos,
            int maxHP, int dmg, int atckRng
            )
        {
            Game = game;
            Passable = isPassable;
            MobSize = size;
            Priority = priority;
            MobLevel = level;
            if (priority == DrawingPriority.Player)
                mobActions = game.PlayerActions;
            else
                mobActions = new HashSet<MobAction>();
            Pos = startPos;

            HP = maxHP;
            MaxHP = maxHP;
            Damage = dmg;
            AttackRange = atckRng;
            SetUpEndUpdateEvent();
        }
    }
}
