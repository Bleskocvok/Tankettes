using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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


        public void Explode(Point pt, float radius)
        {

        }

        public override IEnumerable<IDrawable> Elements
        {
            get
            {
                int bit = 4;

                var list = new List<IDrawable>();

                var tex = "terrain";

                var rand = new Random(16);
                
                for (int x = 0; x < 100; x++)
                {
                    for (int y = 0; y < rand.Next(80, 100); y++)
                    {
                        list.Add(new Sprite(tex,
                                new Rectangle(x * bit, y * bit, bit, bit),
                                new Color(255, 128, 200)));
                    }
                }
                
                return list;
            }
        }
    }
}
