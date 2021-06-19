using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tankettes.GameLogic;

namespace Tankettes.Shop
{
    public class NormalProjectile : GameLogic.AbstractProjectile
    {
        public override float ExplosionRadius => 10f;

        public override void Shoot(Point pos,
                                   Vector2 vec,
                                   ref List<IProjectile> projectiles)
        {
            projectiles.Add(new NormalProjectile(pos, vec));
        }

        public NormalProjectile() { }

        public NormalProjectile(Point pos, Vector2 vec)
        {
            Position = pos.ToVector2();
            Previous = pos.ToVector2() - vec;
        }
    }
}
