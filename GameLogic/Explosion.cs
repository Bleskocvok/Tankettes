using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    class Explosion : AbstractDrawable, IGameElement
    {
        public static float TankExplosionRadius = 40f;

        private const float EffectiveRadius = 2.5f;
        private const float Duration = 1500f;

        private float _time = 0;

        public override string Texture => "explosion";

        private static float Damage(float radius)
                => MathF.Round(20f * radius);

        public static void Hit(Tank tank, Point pt, float radius)
        {
            float dist2 = (pt - tank.Rectangle.Center)
                    .ToVector2().LengthSquared();

            if (dist2 <= Math.Pow(radius * EffectiveRadius, 2))
            {
                float dist = MathF.Sqrt(dist2);
                float coeff = 1 - (dist / radius);
                int damage = (int)MathF.Round(Damage(radius) * coeff);
                damage = Math.Abs(damage);
                tank.Health -= damage;
            }
        }

        public void Update(State state, float delta)
        {
            _time += delta;
            int size = (int)((1 - (_time / Duration)) * Rectangle.Width);
            int diff = (Rectangle.Width - size) / 2;
            Rectangle = new((int)(Rectangle.X + diff),
                            (int)(Rectangle.Y + diff),
                            size, size);
            Color = Color.Lerp(Color, new Color(Color, 0), delta / 1000f);
        }

        public Explosion(Point pt, float radius)
        {
            int size = (int)(radius * 2f);
            Rectangle = new(pt.X - size / 2, pt.Y - size / 2, size, size);
        }

        public bool IsDestroyed() => _time >= Duration;
    }
}
