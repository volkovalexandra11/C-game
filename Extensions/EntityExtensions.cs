using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using The_Game.Interfaces;
using The_Game.Mobs;

namespace The_Game.Extensions
{
    public class TextureBox
    {
        public string TextureName { get; set; }
        public RectangleF TextureRect { get; set; }
    }

    public static class EntityExtensions
    {
        public static TextureBox GetTextureBox(this IEntity ent)
        {
            var textureName = ent.GetTexture();
            if (ent is Mob mob)
            {
                var textureSize = mob.TextureSizes[textureName];
                var textureMobPos = mob.TextureMobPos[textureName];
                var textureBoxLocation = new PointF(
                    mob.Pos.X - textureMobPos.X, mob.Pos.Y - textureMobPos.Y);
                return new TextureBox()
                {
                    TextureName = textureName,
                    TextureRect = new RectangleF(textureBoxLocation, textureSize)
                };
            }
            return new TextureBox() { TextureName = textureName, TextureRect = ent.Hitbox };
        }
    }
}
