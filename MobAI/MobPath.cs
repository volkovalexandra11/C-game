using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Mobs;

namespace The_Game.MobAI
{
    public class MobPath
    {
        public Mob Mob { get; }
        public PathNode<Vector2> FirstWP { get; private set; }

        public MobPath(Mob mob, PathNode<Vector2> firstWP)
        {
            Mob = mob;
            FirstWP = firstWP;
        }
    }
}
