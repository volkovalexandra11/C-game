using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Cutscenes;
using The_Game.Interfaces;

namespace The_Game.Game_Panels
{
    [System.ComponentModel.DesignerCategory("")]
    public class CutscenePanel : GamePanel
    {
        private GameForm Form { get; }
        private TextureBrush BackgroundBrush { get; }
        private TextureBrush TextBoxBrush { get; }
        
        private readonly Size textboxSize = new Size(1600, 250);
        private readonly string textboxName = "TextBox";

        private string[] CutsceneText;
        private int LineInd { get; set; }
        
        public Cutscene PanelCutscene;
        public string CurrentLine => CutsceneText[LineInd];
        
        public bool MoveNextLine()
        {
            LineInd++;
            return LineInd != CutsceneText.Length;
        }

        public CutscenePanel(GameForm form, Cutscene cutscene, string backgroundImg, string textName)
        {
            Form = form;
            PanelCutscene = cutscene;
            CutsceneText = TextLoader.LoadCutsceneText(textName);
            BackgroundBrush = TextureLoader.LoadTextureBrush(
                backgroundImg,
                form.InternalSize
            );
            TextBoxBrush = TextureLoader.LoadTextureBrush(
                textboxName,
                textboxSize
            );
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(BackgroundBrush,
                new RectangleF(PointF.Empty, Form.InternalSize));
            g.FillRectangle(TextBoxBrush,
                new RectangleF(new PointF(100, 50), textboxSize));
            g.DrawString(CurrentLine, new Font("Arial", 30), Brushes.Black,
                new RectangleF(new PointF(130, 70), new Size(1450, 300)));
        }
    }
}
