using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public class Player
    {
        public record AmmoCapacity(IProjectile Type, int Count);

        public string Name { get; init; }

        public int Money { get; set; }

        public int Score { get; set; }

        public int TankHealth { get; set; }

        public Color TankColor { get; set; }

        public List<AmmoCapacity> Ammo { get; set; }

        public int Selected { get; set; }

    }
}
