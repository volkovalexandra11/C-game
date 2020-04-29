using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Game.Interfaces;

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
