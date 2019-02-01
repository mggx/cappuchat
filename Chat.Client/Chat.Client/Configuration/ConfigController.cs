using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chat.Client.Configuration
{
    class ConfigController : IConfigController
    {
        private const string CONFIGFILE = "config.json";

        public void CreateConfigFile()
        {
            if (!File.Exists(CONFIGFILE))
            {
                Models.Config config = new Models.Config
                {
                    Host = "localhost",
                    Port = "1232"
                };
                //File.Create(CONFIGFILE);
                string json = JsonConvert.SerializeObject(config);
                File.WriteAllText(CONFIGFILE, json);
            }
        }

        public Models.Config ReadConfig()
        {
            Models.Config config = new Models.Config();
            if (File.Exists(CONFIGFILE))
            {
                using (StreamReader file = File.OpenText(CONFIGFILE))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o2 = (JObject)JToken.ReadFrom(reader);
                    config = o2.ToObject<Models.Config>();
                }
            }
            return config;
        }

        public void WriteConfig(Models.Config config)
        {
            if (File.Exists(CONFIGFILE))
            {
                string json = File.ReadAllText(CONFIGFILE);
                dynamic jsonObject = JsonConvert.DeserializeObject(json);
                jsonObject["Host"] = config.Host;
                jsonObject["Port"] = config.Port;
                string output = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                File.WriteAllText(CONFIGFILE, output);
            }
            else
            {
                //File.Create(CONFIGFILE);
                string json = JsonConvert.SerializeObject(config);
                File.WriteAllText(CONFIGFILE, json);
            }
        }
    }
}
