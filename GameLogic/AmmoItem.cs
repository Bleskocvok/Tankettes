using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public class AmmoItem<T> : IShopItem where T : IProjectile, new()
    {
        public string Name { get; init; }

        public int Price { get; init; }

        public int BuyAmount { get; init; }

        public void Buy(Player buyer)
        {
            // TODO
            /*buyer.Ammo = buyer.Ammo
                .Select(ammo =>
                {
                    if (ammo.Type is T)
                    {
                        ammo.Count += BuyAmount;
                    }
                    return ammo;
                })
                .ToList();*/
            var bought = buyer.Ammo.SingleOrDefault(ammo => ammo.Type is T);
            if (bought != null)
            {
                bought = new(bought.Type, bought.Count + BuyAmount);
            }
            else
            {
                buyer.Ammo.Add(new Player.AmmoCapacity(new T(), BuyAmount));
            }
        }
    }
}
