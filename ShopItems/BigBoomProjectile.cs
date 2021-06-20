using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tankettes.GameLogic;

namespace Tankettes.Shop
{
    public class BigBoomProjectile : AbstractProjectile
    {
        public override string Name => "BigBoom";

        public override string Texture => "ball";

        public override float ExplosionRadius => 60f;

        public override void Shoot(Point pos,
                                   Vector2 vec,
                                   ref List<IProjectile> projectiles)
        {
            projectiles.Add(new BigBoomProjectile(pos, vec));
        }

        public BigBoomProjectile()
            : base() { }

        public BigBoomProjectile(Point pos, Vector2 vec)
            : base(pos, vec)
        { }
    }
}
