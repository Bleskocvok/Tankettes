using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public class Tank : AbstractDrawable, IGameElement
    {
        private const float Gravity = 100f;
        private const int Epsilon = 3;

        public float CannonAngle
        {
            get => _cannon.Angle;
            set => _cannon.Angle = Math.Clamp(value, 0, 180);
        }

        [JsonIgnore]
        public override ICollection<IDrawable> Elements => base.Elements;

        [JsonProperty]
        private readonly Sprite _cannon;

        public int Health { get; set; }

        [JsonConstructor]
        public Tank(string texture,
                    [JsonProperty("_cannon")] Sprite cannon,
                    Rectangle rectangle,
                    Color color,
                    int health,
                    float cannonAngle)
        {
            Texture = texture;
            Rectangle = rectangle;
            Color = color;
            Health = health;

            _cannon = cannon;
            Elements = new List<IDrawable> { _cannon };

            CannonAngle = cannonAngle;
        }

        public Tank(string tank,
                    string cannon,
                    Rectangle rectangle,
                    Color color,
                    int startHealth)
        {
            Texture = tank;
            Rectangle = rectangle;
            Color = color;
            Health = startHealth;

            _cannon = new Sprite(cannon,
                    new Rectangle(Point.Zero, Rectangle.Size));
            Elements = new List<IDrawable> { _cannon };
            
            CannonAngle = 0;
        }

        public void Update(State state, float delta)
        {
            Move(0, Math.Min((int)(Gravity * delta), 1));

            var height = state.Terrain.Height(Rectangle.Center.X);
            if (Rectangle.Bottom > height)
            {
                Move(0, -(int)MathF.Ceiling((float)(Rectangle.Bottom - height)));
            }

            if (Rectangle.Left < state.Terrain.Rectangle.Left)
            {
                Move(state.Terrain.Rectangle.Left - Rectangle.Left, 0);
            }

            if (Rectangle.Right > state.Terrain.Rectangle.Right)
            {
                Move(state.Terrain.Rectangle.Right - Rectangle.Right, 0);
            }

            // TODO
            // Angle = state.Terrain.Steepness(Rectangle.X);
        }

        public void Move(int dx, int dy)
        {
            Rectangle = new(Rectangle.X + dx,
                            Rectangle.Y + dy,
                            Rectangle.Width,
                            Rectangle.Height);
        }

        public bool IsEquilibrium(State state)
        {
            float height = state.Terrain.Height(Rectangle.Center.X);
            return Rectangle.Bottom - height < Epsilon;
        }

        public bool IsDestroyed() => Health <= 0;
    }
}
