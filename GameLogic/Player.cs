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
        private const int DefaultTankHealth = 100;

        public record AmmoCapacity(IProjectile Type, int Count);

        public string Name { get; init; }

        public int Money { get; set; }

        public int Score { get; set; } = 0;

        public int TankHealth { get; set; } = DefaultTankHealth;

        public Color TankColor { get; set; } = Color.White;

        public List<AmmoCapacity> Ammo { get; set; } = new();

        public int Selected { get; set; } = 0;

        public Tank Tank { get; set; } = null;

        public bool IsAlive() => Tank != null && !Tank.IsDestroyed();

        public Player() { }

        public Player(string name,
                      int money,
                      Color color,
                      AmmoCapacity defaultAmmo)
        {
            Name = name;
            Money = money;
            TankColor = color;
            Ammo.Add(defaultAmmo);
        }
    }
}
