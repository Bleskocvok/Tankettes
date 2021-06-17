using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tankettes.UI;

namespace Tankettes
{
    class GameLoop : AbstractDrawable, UI.IMenuScreen
    {
        public GameLoop()
        {
            // TODO
        }

        public void UpdateControls(KeyboardState keyboard) { }

        public void UpdateMouse(Point pos)
        {
            throw new NotImplementedException();
        }

        public void Click(bool ended = false)
        {
            throw new NotImplementedException();
        }

        void Update(GameTime gameTime) { }

        void IMenuScreen.Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
