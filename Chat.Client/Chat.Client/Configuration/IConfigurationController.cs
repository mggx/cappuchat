namespace Chat.Client.Configuration
{
    public interface IConfigurationController<T>
    {
        void CreateConfigurationFile(T classToWrite = default(T));
        void WriteConfiguration(T classToWrite);
        T ReadConfiguration();
        T ReadConfiguration(T fallback);
    }
}
