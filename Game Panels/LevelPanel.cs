using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Interfaces;
using The_Game.Levels;

namespace The_Game.Game_Panels
{
    [System.ComponentModel.DesignerCategory("")]
    class LevelPanel : GamePanel
    {
        private Level PanelLevel { get; }
        public readonly Dictionary<IEntity, Dictionary<string, TextureBrush>> Textures;

        public LevelPanel(Level level)
        {
            PanelLevel = level;
            Textures = PanelLevel.Entities
                .ToDictionary(
                    ent => ent,
                    ent => ent.Textures
                        .ToDictionary(
                            textureName => textureName,
                            textureName => TextureLoader.LoadTextureBrush(
                                textureName, Size.Round(ent.Hitbox.Size)
                            )
                        )
                );
        }

        public override void Draw(Graphics g)
        {
            foreach (var entity in PanelLevel.Entities)
            {
                var texture = Textures[entity][entity.GetTexture()];
                texture.TranslateTransform(entity.Hitbox.Left, entity.Hitbox.Top);
                g.FillRectangle(texture, entity.Hitbox);
                texture.ResetTransform();
            }
        }
    }
}
