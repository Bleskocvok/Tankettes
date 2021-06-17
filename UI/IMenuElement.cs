using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.UI
{
    interface IMenuElement
    {
        void UpdateMouse(Point pos);

        void Click(bool ended = false);
    }
}
