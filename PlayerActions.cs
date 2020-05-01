using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Game
{
    public static class PlayerActions
    {
        public static Dictionary<Keys, PlayerAction> ActionByKey
            = new Dictionary<Keys, PlayerAction>()
            {
                { Keys.A, PlayerAction.GoLeft },
                { Keys.D, PlayerAction.GoRight },
                { Keys.W, PlayerAction.GoUp },
                { Keys.S, PlayerAction.GoDown },
                { Keys.Space, PlayerAction.Jump },
                { Keys.E, PlayerAction.PickUp },
                { Keys.F, PlayerAction.Debug }
            };

        public static Dictionary<MouseButtons, PlayerAction> ActionByButton
            = new Dictionary<MouseButtons, PlayerAction>()
            {
                { MouseButtons.Left, PlayerAction.AttackMelee },
                { MouseButtons.Right, PlayerAction.AttackRanged }
            };
    }
}
