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
        string Texture { get; }

        Color Color { get; }

        Rectangle Rectangle { get; set; }

        ICollection<IDrawable> Elements { get; }

        Label Label { get; }

        float Angle { get; set; }

        bool ChildrenRelative { get; }
    }
}
