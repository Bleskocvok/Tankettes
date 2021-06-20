using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes
{
    public class Renderer
    {
        public SpriteBatch SpriteBatch { get; init; }
        public GraphicsDeviceManager Graphics { get; init; }

        public Dictionary<string, Texture2D> Textures { get; } = new();

        public SpriteFont Font { get; private set; }

        public void LoadAssets(ContentManager content,
                               string font,
                               string[] textures)
        {
            foreach (var name in textures)
            {
                Textures[name] = content.Load<Texture2D>(name);
            }

            Font = content.Load<SpriteFont>(font);
        }

        public void Draw(IDrawable obj, Point origin)
        {
            Rectangle rect = new(obj.Rectangle.Location + origin,
                                 obj.Rectangle.Size);

            // draw the current texture
            if (obj.Texture != null)
            {
                float angle = -obj.Angle * MathF.PI / 180;

                float sin = MathF.Sin(angle);
                float cos = MathF.Cos(angle);

                float x = rect.Width / 2;
                float y = rect.Height / 2;

                var correction = new Vector2(x - (x * cos - y * sin),
                                             y - (x * sin + y * cos));

                rect.Offset(correction);

                SpriteBatch.Draw(
                    Textures[obj.Texture], // 🡸 texture
                    rect,                   // 🡸 destination rectangle
                    null,                   // 🡸 source rectangle
                    obj.Color,              // 🡸 how to color the texture
                    angle,                  // 🡸 rotation angle
                    Vector2.Zero,           // 🡸 rotation origin
                    SpriteEffects.None,     // 🡸 some dumb sprite effects
                    1);                     // 🡸 some dumb layer depth thing

                rect.Offset(-correction);
            }

            // recursively draw all its children
            if (obj.Elements != null)
            {
                var moved = obj.ChildrenRelative
                            ? origin + obj.Rectangle.Location
                            : Point.Zero;

                foreach (var el in obj.Elements)
                {
                    Draw(el, moved);
                }
            }

            // finally draw the text label
            if (obj.Label != null)
            {
                // nice lime green:
                //     new Color(112, 133, 53);
                // but white is better
                var color = Color.White;

                SpriteBatch.DrawString(
                        Font,                       // 🡸 the font to use
                        obj.Label.Text,             // 🡸 string to draw
                        rect.Center.ToVector2(),    // 🡸 origin
                        color,                      // 🡸 text color
                        0,                          // 🡸 rotation
                                                    // 🡿 offset to origin
                                                    // so that it's centered
                        Font.MeasureString(obj.Label.Text) / 2,
                        1,                          // 🡸 scale, TODO
                        SpriteEffects.None,         // 🡸 useless
                        1);                         // 🡸 some dumb layer
                                                    // depth thing
            }
        }
    }
}
