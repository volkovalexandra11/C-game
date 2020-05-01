using System.Collections.Generic;
using System.Windows.Forms;
using The_Game.Model;

namespace The_Game
{
    public static class MobActions
    {
        public static Dictionary<Keys, MobAction> ActionByKey
            = new Dictionary<Keys, MobAction>()
            {
                { Keys.A, MobAction.GoLeft },
                { Keys.D, MobAction.GoRight },
                { Keys.W, MobAction.GoUp },
                { Keys.S, MobAction.GoDown },
                { Keys.Space, MobAction.Jump },
                { Keys.E, MobAction.PickUp },
                { Keys.F, MobAction.Debug },
                { Keys.Right, MobAction.GoRight },
                { Keys.Left, MobAction.GoLeft }
            };

        public static Dictionary<MouseButtons, MobAction> ActionByButton
            = new Dictionary<MouseButtons, MobAction>()
            {
                { MouseButtons.Left, MobAction.AttackMelee },
                { MouseButtons.Right, MobAction.AttackRanged }
            };
    }
}
