using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public interface IGameElement : IDrawable
    {
        void Update(State state, float delta);

        bool IsDestroyed();
    }
}
