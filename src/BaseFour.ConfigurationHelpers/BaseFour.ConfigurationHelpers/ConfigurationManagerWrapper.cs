using System.Collections.Specialized;
using System.Configuration;

namespace BaseFour.ConfigurationHelpers
{
    internal class ConfigurationManagerWrapper : IConfigurationManagerWrapper
    {
        public NameValueCollection AppSettings => ConfigurationManager.AppSettings;
    }
}