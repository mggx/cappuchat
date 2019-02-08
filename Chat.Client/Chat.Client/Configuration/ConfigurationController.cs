using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ConfigurationFile = Chat.Models.Configuration;

namespace Chat.Client.Configuration
{
    public class ConfigurationController : IConfigurationController
    {
        private const string CONFIGFILE = "config.json";

        public void CreateConfigurationFile()
        {
            if (!File.Exists(CONFIGFILE))
            {
                ConfigurationFile configurationFile = new ConfigurationFile
                {
                    Host = "localhost",
                    Port = "1232"
                };
                //File.Create(CONFIGFILE);
                string json = JsonConvert.SerializeObject(configurationFile);
                File.WriteAllText(CONFIGFILE, json);
            }
        }

        public ConfigurationFile ReadConfiguration()
        {
            ConfigurationFile configurationFile = new ConfigurationFile();
            if (File.Exists(CONFIGFILE))
            {
                using (StreamReader file = File.OpenText(CONFIGFILE))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o2 = (JObject)JToken.ReadFrom(reader);
                    configurationFile = o2.ToObject<ConfigurationFile>();
                }
            }
            return configurationFile;
        }

        public void WriteConfiguration(ConfigurationFile configurationFile)
        {
            if (File.Exists(CONFIGFILE))
            {
                string json = File.ReadAllText(CONFIGFILE);
                dynamic jsonObject = JsonConvert.DeserializeObject(json);
                jsonObject["Host"] = configurationFile.Host;
                jsonObject["Port"] = configurationFile.Port;
                string output = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                File.WriteAllText(CONFIGFILE, output);
            }
            else
            {
                //File.Create(CONFIGFILE);
                string json = JsonConvert.SerializeObject(configurationFile);
                File.WriteAllText(CONFIGFILE, json);
            }
        }
    }
}
