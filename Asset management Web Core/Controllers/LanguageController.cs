using Asset_management_Web_Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Asset_management_Web_Core.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult ChangeLanguage(string lang)
        {
            lang = CultureHelper.GetImplementedCulture(lang);

            Response.Cookies.Append("lang", lang, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = false,
                IsEssential = true
            });

            var returnUrl = Request.Headers["Referer"].ToString();

            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Url.Action("Index", "Home") ?? "/";

            return Redirect(returnUrl);
        }
    }
}