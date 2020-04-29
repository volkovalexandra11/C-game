using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Game.Game_Panels
{
    [System.ComponentModel.DesignerCategory("")]
    class PictureButton : Button
    {
        private Image TextureDefault { get; }
        private Image TextureHover { get; }
        private Image TexturePressed { get; }

        public PictureButton(string img, string imgHover, string imgPressed)
        {
            TextureDefault = TextureLoader.LoadImage(img);
            TextureHover = TextureLoader.LoadImage(imgHover);
            TexturePressed = TextureLoader.LoadImage(imgPressed);
            BackgroundImage = TextureDefault;
            BackgroundImageLayout = ImageLayout.Stretch;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            BackgroundImage = TextureHover;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            BackgroundImage = TextureDefault;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            BackgroundImage = TexturePressed;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            BackgroundImage = TextureHover;
            base.OnMouseUp(mevent);
        }
    }
}
