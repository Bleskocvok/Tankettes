using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public abstract class AbstractProjectile : AbstractDrawable, IProjectile
    {
        private const float Drag = 0.013f;
        private const float Gravity = 0.0001f;

        public virtual float ExplosionRadius => 10f;
        public virtual float Speed => 15f;
        public virtual int Size => 10;

        private Vector2 _position;

        public Vector2 Position
        {
            get => _position;
            protected set
            {
                _position = value;
                Rectangle = new(_position.ToPoint(), Rectangle.Size);
            }
        }

        public Vector2 Previous { get; protected set; }

        public AbstractProjectile()
        {
            Rectangle = new(0, 0, Size, Size);
        }

        public AbstractProjectile(Point pos, Vector2 vec)
            : this()
        {
            Position = pos.ToVector2();
            Previous = Position - vec * Speed;
        }

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
            next.Y += Gravity * delta * delta;
            Previous = Position;
            Position = next;
        }

        public void Update(State state, float delta)
        {
            // TODO wind
            Advance(delta);

            if ((float)Position.Y >= state.Terrain.Height((float)Position.X))
            {
                _destroyed = true;
                state.Explode(Position.ToPoint(), ExplosionRadius);
            }
        }
    }
}
