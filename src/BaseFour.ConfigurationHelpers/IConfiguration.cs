using System.Configuration;

namespace BaseFour.ConfigurationHelpers
{
    public interface IConfiguration
    {
        string GetAppSetting(string key, bool required = true);

        string GetAppSettingOrDefault(string key, string defaultValue);

        T GetAppSettingAs<T>(string key, bool required = true);

        T GetAppSettingOrDefaultAs<T>(string key, T defaultValue);

        ConnectionStringSettings GetConnectionString(string name);
    }
}