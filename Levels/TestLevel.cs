using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game
{
    class TestLevel : ILevel
    {
        public Size LevelSize { get; }
        public List<IEntity> Entities { get; }
        public List<IMob> Mobs { get; }
        private readonly Dictionary<IEntity, Dictionary<string, TextureBrush>> textures;

        public TestLevel(Player player, string textureFolder)
        {
            LevelSize = new Size(1800, 1000);
            Entities = new List<IEntity>
            {
                player,
                new Background(LevelSize),
                new Ground(new Size(1800, 300), new Point(0, 700)),
                new CrumbledWall(new Size(150, 400), new Point(100, 300 + 10)),
                new CrumbledWall(new Size(200, 250), new Point(1500, 450 + 10)),
                new CrumbledWall(new Size(900, 50), new Point(700, 400 + 10))
            };
            Entities = Entities.OrderBy(ent => ent.Priority).ToList();
            Mobs = Entities.Where(ent => ent is IMob).Select(mob => (IMob)mob).ToList();
            textures = Entities
                .ToDictionary(
                    ent => ent,
                    ent => ent.Textures
                        .ToDictionary(
                            textureName => textureName,
                            textureName => GetTextureBrush(
                                Path.Combine(textureFolder, textureName),
                                Size.Round(ent.Hitbox.Size)
                            )
                        )
                );
        }

        private TextureBrush GetTextureBrush(string imgPath, Size size)
        {
            var img = Image.FromFile(imgPath);
            var bitmap = new Bitmap(img, size);
            var brush = new TextureBrush(bitmap)
            {
                WrapMode = WrapMode.Clamp
            };
            return brush;
        }

        public void Draw(Graphics g)
        {
            foreach (var entity in Entities)
            {
                var texture = textures[entity][entity.Texture];
                texture.TranslateTransform(entity.Hitbox.Left, entity.Hitbox.Top);
                g.FillRectangle(texture, entity.Hitbox);
                texture.ResetTransform();
            }
        }
    }
}
