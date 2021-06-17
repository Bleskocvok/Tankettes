using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes
{
    class Terrain : AbstractDrawable
    {
        public Terrain(Rectangle rectangle,
                       int seed,
                       decimal amplitude,
                       decimal roughness)
        {
            Rectangle = rectangle;
        }

        public override IEnumerable<IDrawable> Elements
        {
            get
            {
                var list = new List<IDrawable>();
                list.Add(new Sprite("button_normal", new Rectangle(300, 300, 50, 50)));
                return list;
            }
        }
    }
}
