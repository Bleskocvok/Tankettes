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
        private const decimal Drag = 0.13M;
        private const decimal Gravity = 0.1M;

        public Vector2 Position { get; protected set; }

        public Vector2 Previous { get; protected set; }

        public virtual float ExplosionRadius => 10f;

        protected bool _destroyed = false;

        public virtual bool IsDestroyed() => _destroyed;

        public abstract void Shoot(
                        Point pos,
                        Vector2 vec,
                        ref List<IProjectile> projectiles);

        protected void Advance(decimal delta)
        {
            var vec = Position - Previous;
            var next = Position + vec * (float)(1 - Drag);
            next.Y += (float)(Gravity * delta * delta);
            Previous = Position;
            Position = next;
        }

        public void Update(State state, decimal delta)
        {
            Advance(delta);

            if ((decimal)Position.Y >= state.Terrain.Height((decimal)Position.X))
            {
                _destroyed = true;
                state.Terrain.Explode(Position.ToPoint(), ExplosionRadius);
            }
        }
    }
}
