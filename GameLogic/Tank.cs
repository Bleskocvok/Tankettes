using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    class Tank : AbstractDrawable
    {
        public decimal CannonAngle
        {
            get => _cannon.Angle;
            set => _cannon.Angle = Math.Clamp(value, 0, 180);
        }

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
            Elements = new List<IDrawable> { _cannon };
            CannonAngle = 0;
        }

        public override IEnumerable<IDrawable> Elements
        {
            get => base.Elements;
            protected set => base.Elements = value;
        }
    }
}
