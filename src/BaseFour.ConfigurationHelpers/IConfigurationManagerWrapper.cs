using System.Collections.Specialized;
using System.Configuration;

namespace BaseFour.ConfigurationHelpers
{
    public interface IConfigurationManagerWrapper
    {
        NameValueCollection AppSettings { get; }

        ConnectionStringSettingsCollection ConnectionStrings { get; }
    }
}