using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using The_Game.Levels;
using System.Drawing;

namespace The_Game
{

    public interface IEntity
    {
        bool Passable { get; }
        DrawingPriority Priority { get; }
        RectangleF Hitbox { get; }
        string Texture { get; }
        string[] Textures { get; }
    }

    public interface IMob : IEntity
    {
        void Update();
    }
}
