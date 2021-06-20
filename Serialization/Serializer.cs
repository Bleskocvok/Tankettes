using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tankettes
{
    public class Serializer
    {
        public static GameLogic.State LoadGame(string filename)
        {
            string fileContent = File.ReadAllText(filename);

            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
            };

            GameLogic.State result = JsonConvert
                .DeserializeObject<GameLogic.State>(
                    fileContent,
                    settings);

            return result;
        }

        public static void SaveGame(string filename, GameLogic.State state)
        {
            var converters = new JsonConverter[] { };

            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Converters = converters
            };

            var resultString = JsonConvert.SerializeObject(
                    state,
                    Formatting.Indented,
                    settings);

            File.WriteAllText(filename, resultString);
        }
    }
}
