using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public interface IProjectile : IDrawable
    {
        float ExplosionRadius { get; }

        void Shoot(Point pos,
                   Vector2 vec,
                   ref List<IProjectile> projectiles);

        void Update(State state, float delta);

        bool IsDestroyed();
    }
}
