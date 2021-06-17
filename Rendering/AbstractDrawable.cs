using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes
{
    public abstract class AbstractDrawable : IDrawable
    {
        public virtual string Texture { get; protected set; }

        public virtual Rectangle Rectangle { get; set; }

        public virtual IEnumerable<IDrawable> Elements { get; protected set; }

        public virtual Label Label { get; protected set; }
    }
}
