using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace The_Game.Interfaces
{
    public interface IMob : IEntity
    {
        Dictionary<string, Size> TextureSizes { get; }
        Dictionary<string, Point> TextureMobPos { get; }

        void Update();
    }
}
