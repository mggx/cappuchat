using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Formatting = Newtonsoft.Json.Formatting;

namespace Chat.Configurations
{
    public class ConfigurationController<T> : IConfigurationController<T>
    {
        private const string ConfigurationDirectoryName = "Configurations";

        private readonly string _filePath;

        public ConfigurationController()
        {
            Type theType = typeof(T);

            var constructor = theType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
                throw new ArgumentException(
                    $"Could not create ConfigurationController. Given {typeof(T)} has no parameterless constructor.");

            if (!Directory.Exists(ConfigurationDirectoryName))
                Directory.CreateDirectory(ConfigurationDirectoryName);

            _filePath = GetJsonFileName<T>();
        }

        public void CreateConfigurationFile(T classToWrite = default(T))
        {
            var fileName = _filePath;
            if (File.Exists(fileName)) return;
            var instance = Activator.CreateInstance<T>();
            string json = JsonConvert.SerializeObject(instance);
            File.WriteAllText(fileName, json);
            if (classToWrite != null) WriteConfiguration(classToWrite);
        }

        public void WriteConfiguration(T classToWrite)
        {
            string fileName = _filePath;
            if (!File.Exists(fileName)) CreateConfigurationFile();
            string output = JsonConvert.SerializeObject(classToWrite, Formatting.Indented);
            File.WriteAllText(fileName, output);
        }

        public T ReadConfiguration()
        {
            string fileName = _filePath;
            T config;

            if (!File.Exists(fileName))
                return default(T);

            using (StreamReader file = File.OpenText(fileName))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o2 = (JObject) JToken.ReadFrom(reader);
                    config = o2.ToObject<T>();
                }
            }

            return config;
        }

        public T ReadConfiguration(T fallback)
        {
            var retrievedInstance = ReadConfiguration();
            if (retrievedInstance != null) return retrievedInstance;
            WriteConfiguration(fallback);
            return fallback;
        }

        private string GetJsonFileName<T>()
        {
            return $"{ConfigurationDirectoryName}\\{typeof(T).Name}.json";
        }
    }
}
