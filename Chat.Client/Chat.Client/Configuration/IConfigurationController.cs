using ConfigurationFile = Chat.Models.Configuration;

namespace Chat.Client.Configuration
{
    public interface IConfigurationController
    {
        void WriteConfiguration(ConfigurationFile configurationFile);
        void CreateConfigurationFile();
        ConfigurationFile ReadConfiguration();
    }
}
