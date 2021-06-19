using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public class State
    {
        public Terrain Terrain { get; set; }

        public decimal Wind { get; set; }

        public List<Player> Players;

        private List<IProjectile> _projectiles = new();

        private int _current = 0;

        public State() { }

        public State(int seed) { }


        public void NextPlayer()
        {

        }

        public bool IsRoundOver()
        {
            return false;
        }

        public void Update(decimal delta)
        {
            // update projectiles
            foreach (var proj in _projectiles)
            {
                proj.Update(this, delta);
            }

            // delete exploded projectiles
            _projectiles = _projectiles
                .Where(p => !p.IsDestroyed())
                .ToList();
        }

        public void Explode(Point pos, float radius)
        {
            Terrain.Explode(pos, radius);
            // TODO: add a particle effect
        }

        public bool IsEquilibrium() => _projectiles.Count == 0;
    }
}
