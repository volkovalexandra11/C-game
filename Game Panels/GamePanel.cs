using System;
using System.Drawing;
using System.Windows.Forms;

namespace The_Game.Game_Panels
{
    [System.ComponentModel.DesignerCategory("")]
    public abstract class GamePanel : Panel
    {
        public virtual void Draw(Graphics g)
        {
            throw new NotImplementedException();
        }
    }
}
