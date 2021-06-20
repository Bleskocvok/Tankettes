using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes
{
    class BackgroundSaver
    {
        public string Filename { get; init; } = "save";

        public float IntervalMs { get; init; } = 5000;

        private float _accumulated = 0;

        public void Update(GameLogic.State state, GameTime gameTime)
        {
            _accumulated += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_accumulated >= IntervalMs)
            {
                _accumulated -= IntervalMs;

                Task.Run(() =>
                {
                    try
                    {
                        ForceSave(state);
                    }
                    catch
                    {
                        // let's just silently ignore any errors in
                        // order to not interrupt the rest of the program
                    }
                });
            }
        }

        public void ForceSave(GameLogic.State state)
        {
            lock (Filename)
            {
                 Serializer.SaveGame(Filename, state);
            }
        }
    }
}
