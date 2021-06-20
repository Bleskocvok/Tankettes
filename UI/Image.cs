using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.UI
{
    public class Image : AbstractDrawable, IElementUI
    {
        public void Click(bool ended = false) { }

        public void Update(GameTime gameTime) { }

        public void UpdateMouse(Point pos) { }


        public Image(string texture, Rectangle rectangle)
            : this(texture, rectangle, Color.White) { }

        public Image(string texture, Rectangle rectangle, Color color)
        {
            Texture = texture;
            Rectangle = rectangle;
            Color = color;
        }
    }
}
