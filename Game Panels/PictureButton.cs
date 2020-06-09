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

        public PictureButton(string img, string imgHover)
        {
            TextureDefault = TextureLoader.LoadImage(img);
            TextureHover = TextureLoader.LoadImage(imgHover);
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
    }
}
