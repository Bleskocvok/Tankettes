using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tankettes.GameLogic
{
    public class Terrain : AbstractDrawable
    {
        private const int DefaultBlurAmount = 100;
        private const float Delta = 10f;

        /* (Again) Ignopring this property reduces the resulting
         * file-size by a lot. */
        [JsonIgnore]
        public override ICollection<IDrawable> Elements => base.Elements;
        

        [JsonProperty]
        private List<float> _heights = new();

        [JsonProperty]
        private readonly int _blockSize;

        [JsonProperty]
        private readonly string _blockTexture;

        [JsonConstructor]
        public Terrain([JsonProperty("_blockTexture")] string texture,
                       [JsonProperty("Rectangle")] Rectangle rectangle,
                       [JsonProperty("_blockSize")] int blockSize,
                       [JsonProperty("_heights")] List<float> heights)
        {
            _blockTexture = texture;
            Rectangle = rectangle;
            _blockSize = blockSize;
            _heights = heights;

            // reconstruct the elements, since they were ignored
            RecalculateSprites();
        }

        public Terrain(string texture,
                       Rectangle rectangle,
                       int seed,
                       int blockSize,
                       float amplitude,
                       int roughness)
        {
            _blockTexture = texture;
            Rectangle = rectangle;
            _blockSize = blockSize;

            if (_blockSize == 0)
            {
                throw new ArgumentException("Invalid blockSize");
            }

            GenerateHeights(seed, amplitude, roughness);
            RecalculateSprites();
        }

        public float Steepness(float x)
        {
            float h1 = Height(x);
            float h2 = Height(x + Delta);
            var toDeg = 180f * MathF.PI;
            return toDeg * MathF.Atan2(h2 - h1, Delta);
        }

        public float Height(float x)
        {
            int idx = CoordToIdx(x);
            int next = ClampIdx(idx + 1);

            float h1 = _heights[idx];
            float h2 = _heights[next];

            float between = (x - idx * _blockSize) / (float)_blockSize;

            float value = h1 * (1 - between) + h2 * between;

            return Rectangle.Bottom - value;
        }


        private int ClampIdx(int idx)
                => Math.Clamp(idx, 0, _heights.Count - 1);

        private int CoordToIdx(float x)
                => ClampIdx((int)Math.Floor(x / (float)_blockSize));

        private int IdxToCoord(int i)
                => i * _blockSize;

        public void Explode(Point pt, float radius)
        {
            int begin = CoordToIdx(pt.X - radius) + 1;
            int end = CoordToIdx(pt.X + radius) - 1;

            for (int i = begin; i <= end; i++)
            {
                float x = IdxToCoord(i);
                float dx = Math.Abs(x - pt.X);
                float value = MathF.Sqrt(MathF.Pow(radius, 2) - MathF.Pow(dx, 2));
                _heights[i] -= value;
                _heights[i] = Math.Max(0, _heights[i]);
            }

            RecalculateSprites();
        }

        private void GenerateHeights(int seed,
                                     float amplitude,
                                     int roughness)
        {
            var seeds = new int[roughness];
            var seedGen = new Random(seed);

            int count = Rectangle.Width / _blockSize;
            int blur = DefaultBlurAmount;

            var lists = new List<float>[roughness];

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

        private static List<float> Generate(int seed,
                                              int count,
                                              float amplitude,
                                              int flatness)
        {
            var result = new List<float>();

            var rand = new Random(seed);

            for (int x = 0; x < count; x++)
            {
                var val = (float)(rand.NextDouble() * 2 - 1) * amplitude;
                result.Add(val);
            }

            for (int i = 0; i < flatness; i++)
            {
                result = Blur(result);
            }

            return result;
        }

        private static List<float> Blur(List<float> list)
        {
            var result = new List<float>();

            var kernel = new float[]
            {
                0.06136f, 0.24477f, 0.38774f, 0.24477f, 0.06136f
            };

            float SafeAt(int idx)
                    => list[Math.Clamp(idx, 0, list.Count - 1)];

            for (int i = 0; i < list.Count; i++)
            {
                float value = 0;

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
                        list.Add(new Sprite(_blockTexture, rect));
                    }
                }
            }

            Elements = list;
        }
    }
}
