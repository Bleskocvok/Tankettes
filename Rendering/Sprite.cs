﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes
{
    class Sprite : AbstractDrawable
    {
        public Sprite(string texture, Rectangle rectangle)
                : this(texture, rectangle, Color.White) { }

        public Sprite(string texture, Rectangle rectangle, Color color)
        {
            Texture = texture;
            Rectangle = rectangle;
            Color = color;
        }
    }
}
