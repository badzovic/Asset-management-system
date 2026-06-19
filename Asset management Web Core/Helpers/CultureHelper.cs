using System.Collections.Generic;

namespace Asset_management_Web_Core.Helpers
{
    public static class CultureHelper
    {
        private static readonly List<string> _cultures = new()
        {
            "bs",
            "en",
            "de",
            "it"
        };

        public static bool IsCultureAvailable(string cultureName)
        {
            return _cultures.Contains(cultureName);
        }

        public static string GetImplementedCulture(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return GetDefaultCulture();

            if (_cultures.Any(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                return name.ToLower();

            return GetDefaultCulture();
        }

        public static string GetDefaultCulture()
        {
            return "bs";
        }
    }
}