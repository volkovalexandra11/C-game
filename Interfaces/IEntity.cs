using The_Game.Levels;
using System.Drawing;

namespace The_Game.Interfaces
{
    public interface IEntity
    {
        bool Passable { get; }
        DrawingPriority Priority { get; }
        RectangleF Hitbox { get; }
        string[] Textures { get; }
        
        string GetTexture();
    }

    public interface IMob : IEntity
    {
        void Update();
    }
}
