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
        public static int MaxPower = 100;
        private const int DefaultTankHealth = 100;

        public record AmmoCapacity(IProjectile Type, int Count);

        public string Name { get; init; }

        public int Money { get; set; }

        public int Score { get; set; } = 0;

        public int TankHealth { get; set; } = DefaultTankHealth;

        private int _power = MaxPower / 2;

        public int Power
        {
            get => _power;
            set => _power = Math.Clamp(value, 0, MaxPower);
        }

        public Color TankColor { get; set; } = Color.White;

        public List<AmmoCapacity> Ammo { get; set; } = new();

        public AmmoCapacity SelectedAmmo => Ammo[_selected];

        private int _selected = 0;

        public Tank Tank { get; set; } = null;

        public bool IsAlive()
                => Tank != null && !Tank.IsDestroyed();

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

        public void SpendAmmo()
        {
            Ammo[_selected] = new(SelectedAmmo.Type, SelectedAmmo.Count -1);
            if (SelectedAmmo.Count <= 0)
            {
                Ammo.Remove(SelectedAmmo);
            }
        }

        public void NextAmmo()
                => _selected = (_selected + 1) % Ammo.Count;

        public void PrevAmmo()
        {
            _selected -= 1;

            if (_selected < 0)
                _selected += Ammo.Count;
        }
    }
}
