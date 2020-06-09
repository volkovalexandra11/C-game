using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Cutscenes;
using The_Game.Levels;
using The_Game.Model;

namespace The_Game.Mobs
{
    class King : Mob
    {
        public static Size KingSize = new Size(90, 210);

        public override string[] Textures
            => new[] { "King" };
        public override Dictionary<string, Size> TextureSizes { get; }
        public override Dictionary<string, Point> TextureMobPos { get; }

        public override string GetTexture()
        {
            return "King";
        }

        protected override void TakeDamage(Mob attacker, int dmg)
        {
            base.TakeDamage(attacker, dmg);
            if (IsDead)
            {
                Game.ShowCutscene(Cutscene.FinalCutscene);
            }
        }

        public King(GameState game, Level level, Vector2 startPos)
            : base(game, level, true, KingSize,
                  DrawingPriority.Mob, startPos, 1, 20, 0)
        {
            TextureSizes = new Dictionary<string, Size>() { {"King", KingSize } };
            TextureMobPos = new Dictionary<string, Point>()
                { { "King", new Point(KingSize.Width / 2, KingSize.Height) } };
        }
    }
}
