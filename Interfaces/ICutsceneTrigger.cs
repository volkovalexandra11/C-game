using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game.Interfaces
{
    public interface ITrigger
    {
        bool Active { get; set; }

        void Trigger();
    }
}
