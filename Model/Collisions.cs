using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using The_Game.Interfaces;
using The_Game.Levels;

namespace The_Game.Model
{
    public static class Collisions
    {
        private static Dictionary<RectangleSide, Point> directions =
            new Dictionary<RectangleSide, Point>() {
                { RectangleSide.Top,    new Point(0, -1) },
                { RectangleSide.Right,  new Point(1, 0) },
                { RectangleSide.Bottom, new Point(0, 1) },
                { RectangleSide.Left,   new Point(-1, 0) }
            };

        public static IEnumerable<IEntity> GetCollisions(Level level, IEntity ent)
        {
            return level.Entities
                .Where(otherEnt => AreColliding(ent, otherEnt));
        }

        private static bool AreColliding(IEntity ent1, IEntity ent2)
        {
            return !RectangleF.Intersect(ent1.Hitbox, ent2.Hitbox).IsEmpty;
        }

        public static Vector2 GetCollisionOffset(RectangleF mobRect, RectangleF entRect)
        {
            var offsets = new [] {
                Tuple.Create(RectangleSide.Top,    mobRect.Bottom - entRect.Top),
                Tuple.Create(RectangleSide.Right,  entRect.Right - mobRect.Left),
                Tuple.Create(RectangleSide.Bottom, entRect.Bottom - mobRect.Top),
                Tuple.Create(RectangleSide.Left,   mobRect.Right - entRect.Left)
            };
            var (offsetSide, offset) = offsets
                .Where(dCoord => dCoord.Item2 > 0)
                .DefaultIfEmpty(Tuple.Create(RectangleSide.Right, 0f))
                .MinBy(dCoord => dCoord.Item2);
            var offsetDir = directions[offsetSide];
            return new Vector2(offsetDir.X * offset, offsetDir.Y * offset);
        }

        public static bool IsStandingOnSurface(Level level, IEntity ent)
        {
            var possibleGround = new RectangleF(
                ent.Hitbox.Left, ent.Hitbox.Bottom + 1e-3f,
                ent.Hitbox.Width, 1e-3f
            );
            return level.Entities
                .Where(entity => !entity.Passable)
                .Any(
                otherEnt => !RectangleF.Intersect(possibleGround, otherEnt.Hitbox).IsEmpty
            );
        }

    }

    public static class RectangleFExtensions
    {
        public static PointF GetCenter(this RectangleF rect)
            => new PointF(rect.Left + rect.Width/2, rect.Top + rect.Height/2);
    }

    public enum RectangleSide
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public static class EnumerableExtensions
    {
        public static TSource MinBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector
        )
            where TKey : IComparable
        {
            return source
                .Aggregate((minEl, nextEl) =>
                            keySelector(minEl).CompareTo(keySelector(nextEl)) > 0
                            ? nextEl : minEl
                );
        }
    }
}
