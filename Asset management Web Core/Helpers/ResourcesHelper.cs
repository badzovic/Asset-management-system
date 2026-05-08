using System.Resources;

namespace Asset_management_Web_Core.Helpers
{
    public static class ResourcesHelper
    {
        public static string Translate(string key, string? lang)
        {
            lang = CultureHelper.GetImplementedCulture(lang);

            ResourceManager resourceManager = lang.ToLower() switch
            {
                "en" => Asset_management_Web_Core.Resources.en.ResourceManager,
                "bs" => Asset_management_Web_Core.Resources.bs.ResourceManager,
                _ => Asset_management_Web_Core.Resources.bs.ResourceManager
            };

            var translation = resourceManager.GetString(key);

            return string.IsNullOrWhiteSpace(translation)
                ? key
                : translation;
        }
    }
}