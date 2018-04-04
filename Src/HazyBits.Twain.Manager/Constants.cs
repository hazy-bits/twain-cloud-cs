using System.Configuration;

namespace HazyBits.Twain.Manager
{
    public static class Constants
    {
        private const string ApiRootKey = "ApiRoot";

        public static string ApiRoot => ConfigurationManager.AppSettings[ApiRootKey];
    }
}
