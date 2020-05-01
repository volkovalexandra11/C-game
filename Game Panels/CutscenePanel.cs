using System.Drawing;
using The_Game.Cutscenes;

namespace The_Game.Game_Panels
{
    [System.ComponentModel.DesignerCategory("")]
    public class CutscenePanel : GamePanel
    {
        private GameForm Form { get; }
        private TextureBrush BackgroundBrush { get; }

        private string[] CutsceneText;
        private int LineInd { get; set; }
        
        public Cutscene PanelCutscene;
        public string CurrentLine => CutsceneText[LineInd];
        
        public bool MoveNextLine()
        {
            LineInd++;
            return LineInd != CutsceneText.Length;
        }

        public CutscenePanel(GameForm form, Cutscene cutscene, string backgroundImg, string textFile)
        {
            Form = form;
            PanelCutscene = cutscene;
            CutsceneText = TextLoader.LoadCutsceneText(textFile);
            BackgroundBrush = TextureLoader.LoadTextureBrush(
                backgroundImg,
                form.InternalSize
            );
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(BackgroundBrush,
                new RectangleF(PointF.Empty, Form.InternalSize));
            g.DrawString(CurrentLine, new Font("Arial", 30), Brushes.Black, new PointF(
                Form.InternalSize.Width / 10, Form.InternalSize.Height / 6));
        }
    }
}
