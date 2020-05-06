
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;
//using The_Game.Levels;
//using System.Drawing;
//using System.Runtime.CompilerServices;

//namespace The_Game.Interfaces
//{
//    public interface IEntity
//    {
//        bool Passable { get; }
//        DrawingPriority Priority { get; }
//        RectangleF Hitbox { get; }
//        string[] Textures { get; }
        
//        string GetTexture();
//    }

//    public static class EntityExtensions
//    {
//        public static Tuple<string, RectangleF> GetTextureBox(this IEntity entity)
//        {
//            var textureName = entity.GetTexture();
//            if (entity is IMob mob)
//            {
//                var textureData = mob.TextureInfo[textureName]; 
//                return Tuple.Create(textureName, new RectangleF(
//                    mob.Pos.X - textureData.MobPos.X, mob.Pos.Y - textureData.MobPos.Y,
//                    textureData.Size.Width, textureData.Size.Height
//                    )
//                );
//            }
//            return Tuple.Create(textureName, entity.Hitbox);
//        }
//    }
//}
