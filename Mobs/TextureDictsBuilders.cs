using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game.Mobs
{
    static class TextureSizesBuilder
    {
        public static Dictionary<string, Size> Build(
            string walkLeftTexture, string walkRightTexture,
            string attackLeftTexture, string attackRightTexture,
            Size size, Size attackSize)
        {
            return new Tuple<string, Size>[]
                {
                    Tuple.Create(walkLeftTexture, size),
                    Tuple.Create(walkRightTexture, size),
                    Tuple.Create(attackLeftTexture, attackSize),
                    Tuple.Create(attackRightTexture, attackSize)
                }
                .ToDictionary(textAndSize => textAndSize.Item1, textAndSize => textAndSize.Item2);
        }
    }

    static class TextureMobPosBuilder
    {
        public static Dictionary<string, Point> Build(
            string walkLeftTexture, string walkRightTexture,
            string attackLeftTexture, string attackRightTexture,
            Size size, Size attackSize)
        {
            return new Tuple<string, Point>[]
                {
                    Tuple.Create(walkLeftTexture, new Point(size.Width / 2, size.Height)),
                    Tuple.Create(walkRightTexture, new Point(size.Width / 2, size.Height)),
                    Tuple.Create(attackLeftTexture, new Point(
                        attackSize.Width - size.Width / 2, size.Height)),
                    Tuple.Create(attackRightTexture, new Point(size.Width / 2, size.Height))
                }
                .ToDictionary(textAndPos => textAndPos.Item1, textAndPos => textAndPos.Item2);
        }
    }
}
