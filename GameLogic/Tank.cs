using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes
{
    class Tank : AbstractDrawable
    {
        public float CannonAngle
        {
            // TODO
            get;
            set;
        } = 0;

        public int Health { get; set; }

        private Sprite _cannon;

        public Tank(string tank,
                    string cannon,
                    Rectangle rectangle,
                    Color color,
                    int startHealth)
        {
            Texture = tank;
            Rectangle = rectangle;
            Color = color;
            Health = startHealth;
            _cannon = new Sprite(cannon,
                    new Rectangle(Point.Zero, Rectangle.Size));
        }

        public override IEnumerable<IDrawable> Elements
        {
            get => base.Elements;
            protected set => base.Elements = value;
        }
    }
}
