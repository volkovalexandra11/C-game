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

        public readonly HashSet<MobAction> mobActions;

        private const float proximityConst = 400;

        private const float gravAcceleration = 0.2f;
        private const float maxWalkingVelocity = 10f;

        private const float initialJumpVelocity = 5f;

        private const float initialThrowbackVertVelocity
            = 0.6f * initialJumpVelocity;
        private const float initialThrowbackHorVelocity
            = 0.8f * maxWalkingVelocity;

        private const float ladderClimbingVelocity = 3f;

        private bool IsStill => Math.Abs(GuidedHorVel) <= 0.1f;
        private bool IsGoingRight => GuidedHorVel > 0.1f;
        private bool IsGoingLeft => GuidedHorVel < -0.1f;

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
        protected float GuidedHorVel { get; set; }
        protected Vector2 Velocity => new Vector2(OwnHorVel + GuidedHorVel, VerticalVel);

        protected const int MinChangeDirTimeUpdates = 10;
        protected int UpdatesSinceChangeOfDir;

        protected virtual int AttackTimeUpdates => 30;
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
        protected bool IsOnLadder { get; set; }

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

        public virtual void Update()
        {
            var collisions = Collisions.GetCollisions(MobLevel, Hitbox);
            PreprocessCollisions(collisions);
            if (PlannedPath != null) FillActions();
            ProcessActions(collisions);
            UpdatePosition(collisions);
            ProcessCollisions(collisions);

            if (mobActions.Contains(MobAction.Debug))
            { mobActions.Remove(MobAction.Debug); };

            EndUpdate();
        }

        private void PreprocessCollisions(List<IEntity> collisions)
        {
            IsOnLadder = collisions.Where(ent => ent is Ladder).Any();
        }

        private void ProcessCollisions(List<IEntity> collisions)
        {
            var mobOffset = Vector2.Zero;
            foreach (var ent in collisions)
            {
                if (!ent.Passable)
                    mobOffset += Collisions.GetCollisionOffset(Hitbox, ent.Hitbox);
                ProcessCollision(ent);
            }
            Pos += mobOffset;
            if (Collisions.IsStandingOnSurface(MobLevel, this) || IsOnLadder)
            {
                if (State == MobState.Jumping)
                {
                    VerticalVel = 0;
                    State = MobState.Walking;
                }
            }
            else
            {
                State = MobState.Jumping;
            }
        }

        protected virtual void ProcessCollision(IEntity otherEnt)
        {
        }

        private void UpdatePosition(List<IEntity> collisions)
        {
            GuidedHorVel = GetNewHorGuidedVel();
            OwnHorVel = Math.Abs(OwnHorVel) < 0.1f
                ? 0
                : 0.95f * OwnHorVel;
            if (State == MobState.Jumping && !IsOnLadder)
            {
                VerticalVel += gravAcceleration;
            }
            Pos += Velocity;
        }

        private void ProcessActions(List<IEntity> collisions)
        {
            if (IsOnLadder)
            {
                VerticalVel = mobActions.Contains(MobAction.GoUp)
                    ? - ladderClimbingVelocity
                    : ladderClimbingVelocity;
            }
            if (mobActions.Contains(MobAction.Jump))
            {
                if (State == MobState.Walking)
                {
                    Jump();
                }
            }
            if (mobActions.Contains(MobAction.AttackMelee))
                TryAttackMelee();
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
            else if (distToPlayer < 1050)
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
                damagedMob.TakeDamage(this, Damage);
            }
        }

        protected virtual void TakeDamage(Mob attacker, int dmg)
        {
            ThrowbackMob(attacker);
            HP -= dmg;
        }

        private void ThrowbackMob(Mob attacker)
        {
            if (VerticalVel > - 2 * initialJumpVelocity)
            {
                State = MobState.Jumping;
                VerticalVel -= initialThrowbackVertVelocity;
            }
            if (Math.Abs(OwnHorVel) < maxWalkingVelocity)
            {
                if (attacker.Pos.X < Pos.X)
                    OwnHorVel += initialThrowbackHorVelocity;
                else
                    OwnHorVel -= initialThrowbackHorVelocity;
            }
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
