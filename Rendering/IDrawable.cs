using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes
{
    public interface IDrawable
    {
        public string Texture { get; }

        public Color Color { get; }

        public Rectangle Rectangle { get; set; }

        public IEnumerable<IDrawable> Elements { get; }

        public Label Label { get; }
    }
}
