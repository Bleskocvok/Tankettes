using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes
{
    class Tank
    {
        public Color Color { get; set; }

        public Point Position { get; set; }

        public float CannonAngle { get; set; }

        public int Health { get; set; }
    }
}
