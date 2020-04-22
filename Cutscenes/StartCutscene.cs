using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Game.Interfaces;

namespace The_Game.Cutscenes
{
    class StartCutscene : ICutscene
    {
        public string ImageDirectory => "Textures";
        public string BackgroundImageName => "CastleCeremony.jpg";
        public TextureBrush BackgroundBrush { get; }

        public List<string> Text => new List<string>()
        {
            "You, the loyal Knight, have been summoned to the court of the King.",
            "He tasked you with the mission of an utmost importance:",
            "You should put the flames of the rebellion which has torn the country apart to the rest."
        };

        public int LineInd { get; set; }
        public string CurrentLine => Text[LineInd];

        public bool MoveNextLine()
        {
            LineInd++;
            return LineInd != Text.Count;
        }

        public StartCutscene(GameForm form)
        {
            BackgroundBrush = GraphicMethods.GetTextureBrush(
                Path.Combine(ImageDirectory, BackgroundImageName),
                form.InternalSize
            );
        }
    }
}
