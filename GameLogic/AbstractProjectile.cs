using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public abstract class AbstractProjectile : IProjectile
    {
        private const float Drag = 0.13f;
        private const float Gravity = 0.1f;

        public Vector2 Position { get; protected set; }

        public Vector2 Previous { get; protected set; }

        public virtual float ExplosionRadius => 10f;

        protected bool _destroyed = false;

        public virtual bool IsDestroyed() => _destroyed;

        public abstract void Shoot(
                        Point pos,
                        Vector2 vec,
                        ref List<IProjectile> projectiles);

        protected void Advance(float delta)
        {
            var vec = Position - Previous;
            var next = Position + vec * (1 - Drag);
            next.Y += (Gravity * delta * delta);
            Previous = Position;
            Position = next;
        }

        public void Update(State state, float delta)
        {
            Advance(delta);

            if ((float)Position.Y >= state.Terrain.Height((float)Position.X))
            {
                _destroyed = true;
                state.Terrain.Explode(Position.ToPoint(), ExplosionRadius);
            }
        }
    }
}
