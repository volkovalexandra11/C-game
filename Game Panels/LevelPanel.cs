using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Extensions;
using The_Game.Interfaces;
using The_Game.Levels;
using The_Game.Mobs;
using The_Game.Model;

namespace The_Game.Game_Panels
{
    [System.ComponentModel.DesignerCategory("")]
    class LevelPanel : GamePanel
    {
        private static readonly bool DrawWaypointsOn = true;
        private static readonly bool DrawAttackZonesOn = true;

        private Level PanelLevel { get; }
        public readonly Dictionary<IEntity, Dictionary<string, TextureBrush>> Textures;

        private static readonly Pen healthBarPen
            = new Pen(Color.Black, 4);
        private static readonly Rectangle playerHealthBarRect
            = new Rectangle(10, 10, 300, 50);
        private static readonly Size mobHealthBarSize
            = new Size(100, 20);
        private static readonly Brush hpBrush = Brushes.IndianRed;
        private static readonly Brush lostHpBrush = Brushes.LightGray;

        private static readonly Pen edgeArrowPen
            = new Pen(Color.BlueViolet, 4);
        private static readonly Pen pathArrowPen
            = new Pen(Color.DarkRed, 4);

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

        private void DrawInterface(Graphics g)
        {
            DrawPlayerHealthBar(g);
            DrawMobHealthBars(g);
        }

        private void DrawPlayerHealthBar(Graphics g)
        {
            DrawHealthBar(g, playerHealthBarRect.Location, playerHealthBarRect.Size,
                PanelLevel.Game.GamePlayer);
        }

        private void DrawMobHealthBars(Graphics g)
        {
            foreach (var mob in PanelLevel.Mobs.Where(mob => !(mob is Player)))
            {
                var hitbox = mob.Hitbox;
                DrawHealthBar(g,
                    new Point(
                        (int)( (hitbox.Left + hitbox.Right) / 2 - mobHealthBarSize.Width / 2 ),
                        (int)(hitbox.Top - mobHealthBarSize.Height)
                    ),
                    mobHealthBarSize,
                    mob
                );
            }
        }

        private void DrawHealthBar(Graphics g, Point location, Size size, Mob mob)
        {
            var hpFraction = (float)mob.HP / mob.MaxHP;
            g.FillRectangle(hpBrush,
                location.X, location.Y,
                size.Width * hpFraction, size.Height);
            g.FillRectangle(lostHpBrush,
                location.X + size.Width * hpFraction, location.Y,
                size.Width * (1 - hpFraction), size.Height);
            g.DrawRectangle(healthBarPen, new Rectangle(location, size));
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
            if (DrawAttackZonesOn)
            {
                DrawAttackZones(g);
            }
            if (DrawWaypointsOn && PanelLevel.WPReverseGraph != null)
            {
                DrawWaypoints(g);
            }
            DrawInterface(g);
        }

        private void DrawAttackZones(Graphics g)
        {
            foreach (var mob in PanelLevel.Mobs)
            {
                g.DrawRectangle(Pens.PaleVioletRed,
                    Rectangle.Round(mob.AttackZone));
            }
        }

        private void DrawWaypoints(Graphics g)
        {
            var wpCircleRadius = 15;
            DrawWaypoints(g, wpCircleRadius);
            DrawWPEdges(g);
            DrawFoundPaths(g, wpCircleRadius);
        }

        private void DrawWaypoints(Graphics g, int circleRad)
        {
            foreach (var wp in PanelLevel.Waypoints)
            {
                g.FillEllipse(Brushes.BlueViolet,
                    wp.X - circleRad, wp.Y - circleRad,
                    2*circleRad, 2*circleRad);
            }
        }

        private void DrawWPEdges(Graphics g)
        {
            foreach (var wpEdge in PanelLevel.WPReverseGraph)
                foreach (var startWP in wpEdge.Value.Keys)
                    g.DrawArrow(edgeArrowPen, startWP, wpEdge.Key);
        }

        private void DrawFoundPaths(Graphics g, int circleRad)
        {
            foreach (var mob in PanelLevel.Mobs)
            {
                var wp = mob.GetClosestWaypoint();
                g.FillEllipse(Brushes.IndianRed,
                            wp.X - circleRad, wp.Y - circleRad,
                            2*circleRad, 2*circleRad);
                if (mob.PlannedPath != null)
                {
                    var thisWPPath = mob.PlannedPath;
                    var nextWPPath = thisWPPath;
                    while (nextWPPath != null)
                    {
                        g.FillEllipse(Brushes.DarkRed,
                            thisWPPath.Position.X - circleRad, thisWPPath.Position.Y - circleRad,
                            2*circleRad, 2*circleRad);
                        g.DrawArrow(pathArrowPen, thisWPPath.Position, nextWPPath.Position);
                        thisWPPath = nextWPPath;
                        nextWPPath = nextWPPath.Previous;
                    }
                }
            }
        }
    }
}
