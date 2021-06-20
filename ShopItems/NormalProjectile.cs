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
        public override string Name => "Normal";

        public override string Texture => "ball";

        public override float ExplosionRadius => 20f;

        public override void Shoot(Point pos,
                                   Vector2 vec,
                                   ref List<IProjectile> projectiles)
        {
            projectiles.Add(new NormalProjectile(pos, vec));
        }

        public NormalProjectile()
            : base() {}

        public NormalProjectile(Point pos, Vector2 vec)
            : base(pos, vec)
        {}
    }
}
