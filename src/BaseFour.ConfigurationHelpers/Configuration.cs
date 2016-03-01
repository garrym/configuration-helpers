using System.ComponentModel;
using System.Configuration;

namespace BaseFour.ConfigurationHelpers
{
    public class Configuration : IConfiguration
    {
        private readonly IConfigurationManagerWrapper _configurationManagerWrapper;

        public Configuration(IConfigurationManagerWrapper configurationManagerWrapper = null)
        {
            _configurationManagerWrapper = configurationManagerWrapper ?? new ConfigurationManagerWrapper();
        }

        public string GetAppSetting(string key, bool required = true)
        {
            var value = _configurationManagerWrapper.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                if (required)
                    throw new ConfigurationErrorsException($"Required AppSetting '{key}' is missing.");
                return string.Empty;
            }

            return value;
        }

        public string GetAppSettingOrDefault(string key, string defaultValue)
        {
            return GetAppSettingOrDefaultAs(key, defaultValue);
        }

        public T GetAppSettingAs<T>(string key, bool required = true)
        {
            var value = GetAppSetting(key, required);

            var typeConverter = GetTypeConverter<T>();
            if (CanConvert<T>(typeConverter, value))
                return Convert<T>(typeConverter, value);

            if (required)
                throw new ConfigurationErrorsException($"Required AppSetting '{key}' is invalid. Unable to convert to type '{typeof(T).FullName}'"); ;

            return default(T);
        }

        public T GetAppSettingOrDefaultAs<T>(string key, T defaultValue)
        {
            var value = GetAppSetting(key, false);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            var typeConverter = GetTypeConverter<T>();
            if (CanConvert<T>(typeConverter, value))
                return Convert<T>(typeConverter, value);

            return defaultValue;
        }

        public ConnectionStringSettings GetConnectionString(string name)
        {
            var connectionString = _configurationManagerWrapper.ConnectionStrings[name];
            if (connectionString == null)
                throw new ConfigurationErrorsException($"Required connection string '{name}' is missing.");
            
            if (string.IsNullOrWhiteSpace(connectionString.ConnectionString))
                throw new ConfigurationErrorsException($"Required connection string '{name}' is empty.");

            return connectionString;
        }

        private TypeConverter GetTypeConverter<T>()
        {
            return TypeDescriptor.GetConverter(typeof(T));
        }

        private static T Convert<T>(TypeConverter typeConverter, string value)
        {
            return (T)typeConverter.ConvertFromInvariantString(value);
        }

        private static bool CanConvert<T>(TypeConverter typeConverter, string value)
        {
            return typeConverter.IsValid(value);
        }
    }
}
