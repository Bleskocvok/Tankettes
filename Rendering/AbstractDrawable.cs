using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tankettes
{
    public abstract class AbstractDrawable : IDrawable
    {
        public virtual string Texture { get; protected set; } = null;

        public virtual Rectangle Rectangle { get; set; }

        public virtual ICollection<IDrawable> Elements
        {
            get;
            protected set;
        } = null;

        public virtual Label Label { get; protected set; } = null;

        public virtual Color Color { get; set; } = Color.White;

        public virtual float Angle { get; set; } = 0;

        public virtual bool ChildrenRelative { get; protected set; } = true;
    }
}
