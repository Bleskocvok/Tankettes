using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.UI
{
    interface IMenuScreen : IDrawable, IElementUI
    {
        void UpdateControls(KeyboardState keyboard);
    }
}
