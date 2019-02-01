namespace Chat.Client.Configuration
{
    public interface IConfigController
    {
        void WriteConfig(Models.Config config);
        void CreateConfigFile();
        Models.Config ReadConfig();
    }
}
