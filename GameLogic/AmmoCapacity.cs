using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public class AmmoCapacity
    {
        public IProjectile Type { get; init; }
        public int Count { get; init; }

        private AmmoCapacity() { }

        public AmmoCapacity(IProjectile type, int count)
        {
            Type = type;
            Count = count;
        }
    }
}
