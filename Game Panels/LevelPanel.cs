using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Extensions;
using The_Game.Interfaces;
using The_Game.Levels;

namespace The_Game.Game_Panels
{
    [System.ComponentModel.DesignerCategory("")]
    class LevelPanel : GamePanel
    {
        private static bool DrawWaypointsOn = true;
        private Level PanelLevel { get; }
        public readonly Dictionary<IEntity, Dictionary<string, TextureBrush>> Textures;

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

        public override void Draw(Graphics g)
        {
            foreach (var entity in PanelLevel.Entities)
            {
                var texture = Textures[entity][entity.GetTexture()];
                texture.TranslateTransform(entity.Hitbox.Left, entity.Hitbox.Top);
                g.FillRectangle(texture, entity.Hitbox);
                texture.ResetTransform();
            }
            if (DrawWaypointsOn && PanelLevel.WPReverseGraph != null)
            {
                DrawWaypoints(g);
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
                    var thisWPPath = mob.PlannedPath.FirstWP;
                    var nextWPPath = thisWPPath;
                    while (nextWPPath != null)
                    {
                        g.FillEllipse(Brushes.DarkRed,
                            thisWPPath.Value.X - circleRad, thisWPPath.Value.Y - circleRad,
                            2*circleRad, 2*circleRad);
                        g.DrawArrow(pathArrowPen, thisWPPath.Value, nextWPPath.Value);
                        thisWPPath = nextWPPath;
                        nextWPPath = nextWPPath.Previous;
                    }
                }
            }
        }
    }
}
