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
        private const float TankRelativeSize = 0.066f;

        public Terrain Terrain { get; set; }

        public Rectangle Rectangle { get; set; }

        public float Wind { get; set; }

        public Player CurrentPlayer { get => Players[_current]; }

        public List<Player> Players { get; init; }

        private int _current = 0;

        private List<IProjectile> _projectiles = new();

        private Random _random;

        public State()
        {
            // TODO
            /*// use random seed
            _random = new Random();*/
        }

        public State(int seed,
                     List<Player> players,
                     Rectangle rectangle,
                     string tankTexture,
                     string cannonTexture,
                     string terrainTexture,
                     int terrainBlockSize)
        {
            _random = new Random(seed);
            Rectangle = rectangle;
            Players = players;

            var terrainSeed = _random.Next();
            var amplitude = (float)(_random.NextDouble()
                                    * rectangle.Height * 0.4);
            int roughness = _random.Next(1, 5);
            Terrain = new(terrainTexture,
                          rectangle,
                          terrainSeed,
                          terrainBlockSize,
                          amplitude,
                          roughness);

            InitializeTanks(tankTexture, cannonTexture);
        }

        private void InitializeTanks(string tankTexture, string cannonTexture)
        {
            int size = (int)(Rectangle.Height * TankRelativeSize);

            foreach (var pl in Players)
            {
                int x = _random.Next(Rectangle.Left, Rectangle.Right);
                int y = (int)Terrain.Height(x + size / 2) - size;

                pl.Tank = new Tank(tankTexture,
                                   cannonTexture,
                                   new Rectangle(x, y, size, size),
                                   pl.TankColor,
                                   pl.TankHealth);
            }
        }

        public void Shoot()
        {
            CurrentPlayer.SelectedAmmo;
        }

        public void NextPlayer()
        {
            if (IsRoundOver())
                return;

            _current = (_current + 1) % Players.Count;

            if (!CurrentPlayer.IsAlive())
                NextPlayer();
        }

        public bool IsRoundOver()
        {
            return Players.Count(p => p.IsAlive()) < 2;
        }

        public void Update(float delta)
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

            // update tanks
            var tanks = Players
                .Where(p => p != null)
                .Select(p => p.Tank);

            foreach (var tank in tanks)
            {
                tank.Update(this, delta);
            }
        }

        public void Explode(Point pos, float radius)
        {
            Terrain.Explode(pos, radius);
            // TODO: add a particle effect
        }

        public bool IsEquilibrium()
        {
            return _projectiles.Count == 0;
        }
    }
}
