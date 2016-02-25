using System.Collections.Specialized;

namespace BaseFour.ConfigurationHelpers
{
    public interface IConfigurationManagerWrapper
    {
        NameValueCollection AppSettings { get; }
    }
}