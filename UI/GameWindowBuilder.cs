using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.UI
{
    class GameWindowBuilder
    {
        public ButtonTexture ButtonTexture { get; set; }

        public Point ButtonSize { get; set; } = new(200, 50);

        private Window _result = new();

        public void Reset()
                => _result = new();

        public Window Build()
                => _result;

        public MenuFrame AddMenuFrame(string tag)
        {
            var frame = new UI.MenuFrame();
            _result.Add(tag, frame);
            return frame;
        }

        public Button AddButtonTransition(
                                string frame,
                                string buttonText,
                                int x, int y,
                                string target)
        {
            return AddButton(frame, buttonText, x, y, () =>
            {
                _result.MakeCurrent(target);
            });
        }

        public Button AddButton(string frame,
                                string buttonText,
                                int x, int y,
                                Action action)
        {
            var button = new Button(buttonText,
                    new Rectangle(new Point(x, y), ButtonSize),
                    ButtonTexture);
            button.EventOnRelease += (s, a) => action();
            (_result.MenuScreens[frame] as MenuFrame).Add(button);
            return button;
        }
    }
}
