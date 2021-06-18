using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tankettes
{
    class Terrain : AbstractDrawable
    {
        private List<decimal> _heights = new();

        private readonly int _blockSize;

        private readonly string _texture;

        public Terrain(string texture,
                       Rectangle rectangle,
                       int seed,
                       int blockSize,
                       decimal amplitude,
                       int roughness)
        {
            _texture = texture;
            Rectangle = rectangle;
            _blockSize = blockSize;

            GenerateHeights(seed, amplitude, roughness);
            RecalculateSprites();
        }

        public decimal Height(decimal x)
        {
            int idx = (int)Math.Floor(x / (decimal)_blockSize);
            int next = Math.Min(idx + 1, _heights.Count - 1);

            decimal h1 = _heights[idx];
            decimal h2 = _heights[next];

            decimal between = (x - idx * _blockSize) / (decimal)_blockSize;

            return h1 * (1 - between) + h2 * between;
        }


        public void Explode(Point pt, float radius)
        {

        }

        private void GenerateHeights(int seed,
                                     decimal amplitude,
                                     int roughness)
        {
            var seeds = new int[roughness];
            var seedGen = new Random(seed);

            int count = Rectangle.Width / _blockSize;
            int blur = 4;

            var lists = new List<decimal>[roughness];

            for (int i = 0; i < roughness; i++)
            {
                seeds[i] = seedGen.Next();
            }

            /* Note: this code could be easily executed in parallel */

            for (int i = 0; i < roughness; i++)
            {
                lists[i] = Generate(seeds[i], count, amplitude, blur);
                amplitude /= 2;
                blur /= 2;
            }

            _heights = Enumerable
                    .Range(0, count)
                    // sum all lists
                    .Select(i => lists.Select(list => list.ElementAt(i)).Sum())
                    // elevate the result so that it doesn't contain any
                    // negative values
                    .Select(v =>
                        Math.Clamp(v + Rectangle.Height / 2,
                            Rectangle.Height / 10,
                            Rectangle.Height))
                    .ToList();
        }

        private static List<decimal> Generate(int seed,
                                              int count,
                                              decimal amplitude,
                                              int flatness)
        {
            var result = new List<decimal>();

            var rand = new Random(seed);

            for (int x = 0; x < count; x++)
            {
                var val = (decimal)(rand.NextDouble() * 2 - 1) * amplitude;
                result.Add(val);
            }

            for (int i = 0; i < flatness; i++)
            {
                result = Blur(result);
            }

            return result;
        }

        private static List<decimal> Blur(List<decimal> list)
        {
            var result = new List<decimal>();

            var kernel = new decimal[]
            {
                0.06136M, 0.24477M, 0.38774M, 0.24477M, 0.06136M
            };

            decimal SafeAt(int idx)
                    => list[Math.Clamp(idx, 0, list.Count - 1)];

            for (int i = 0; i < list.Count; i++)
            {
                decimal value = 0;

                for (int k = 0; k < kernel.Length; k++)
                {
                    int d = kernel.Length / 2;
                    value += kernel[k] * SafeAt(i - d + k);
                }
                
                result.Add(value);
            }

            return result;
        }

        private void RecalculateSprites()
        {
            var list = new List<IDrawable>();

            for (int i = 0; i < _heights.Count; i++)
            {
                for (int y = 0; y < Rectangle.Height; y += _blockSize)
                {
                    if (y >= Rectangle.Height - _heights[i])
                    {
                        var rect = new Rectangle(
                                Rectangle.X + i * _blockSize,
                                Rectangle.Y + y,
                                _blockSize,
                                _blockSize);
                        list.Add(new Sprite(_texture, rect));
                    }
                }
            }

            Elements = list;
        }
    }
}
