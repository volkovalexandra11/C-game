using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace The_Game
{
    public class Level //: ILevel
    {
        public Size LevelSize { get; }

        public List<IEntity> Entities { get; }

        public List<IMob> Mobs { get; }

        public Vector2 StartPos { get; }

        public readonly Dictionary<IEntity, Dictionary<string, TextureBrush>> Textures;

        public Level(ReadOnlyCollection<IEntity> entities, Vector2 startPos, Player player)
        {
            LevelSize = new Size(1800, 1000);
            StartPos = startPos;
            Entities = entities.Append(player).OrderBy(ent => ent.Priority).ToList();
            Mobs = Entities.Where(ent => ent is IMob).Select(mob => (IMob)mob).ToList();
            Textures = Entities
                .ToDictionary(
                    ent => ent,
                    ent => ent.Textures
                        .ToDictionary(
                            textureName => textureName,
                            textureName => GraphicMethods.GetTextureBrush(
                                Path.Combine(ent.TextureDirectory, textureName),
                                Size.Round(ent.Hitbox.Size)
                            )
                        )
                );
        }
    }

    //class TestLevel : ILevel
    //{
    //    public Size LevelSize { get; }
    //    public List<IEntity> Entities { get; }
    //    public List<IMob> Mobs { get; }
    //    public Vector2 StartPos => new Vector2(400, 700);
    //    private readonly Dictionary<IEntity, Dictionary<string, TextureBrush>> textures;

    //    public TestLevel(Player player)
    //    {
    //        LevelSize = new Size(1800, 1000);
    //        Entities = new List<IEntity>
    //        {
    //            player,
    //            new Background(LevelSize),
    //            new Ground(new Size(1800, 300), new Point(0, 700)),
    //            new CrumbledWall(new Size(150, 400), new Point(100, 300 + 10)),
    //            new CrumbledWall(new Size(200, 250), new Point(1500, 450 + 10)),
    //            new CrumbledWall(new Size(900, 50), new Point(700, 400 + 10))
    //        };
    //        Entities = Entities.OrderBy(ent => ent.Priority).ToList();
    //        Mobs = Entities.Where(ent => ent is IMob).Select(mob => (IMob)mob).ToList();
    //        textures = Entities
    //            .ToDictionary(
    //                ent => ent,
    //                ent => ent.Textures
    //                    .ToDictionary(
    //                        textureName => textureName,
    //                        textureName => GraphicMethods.GetTextureBrush(
    //                            Path.Combine(ent.TextureDirectory, textureName),
    //                            Size.Round(ent.Hitbox.Size)
    //                        )
    //                    )
    //            );
    //    }

    //    public void Draw(Graphics g)
    //    {
    //        foreach (var entity in Entities)
    //        {
    //            var texture = textures[entity][entity.Texture];
    //            texture.TranslateTransform(entity.Hitbox.Left, entity.Hitbox.Top);
    //            g.FillRectangle(texture, entity.Hitbox);
    //            texture.ResetTransform();
    //        }
    //    }
    //}
}
