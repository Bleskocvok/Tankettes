using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public interface IShopItem
    {
        string Name { get; }

        int Price { get; }

        int BuyAmount { get; }

        void Buy(GameLogic.Player buyer);
    }
}
