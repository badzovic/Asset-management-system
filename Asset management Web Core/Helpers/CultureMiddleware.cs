using System.Globalization;
using System.Threading.Tasks;

namespace Asset_management_Web_Core.Helpers
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var lang = context.Request.Cookies["lang"];

            lang = CultureHelper.GetImplementedCulture(lang);

            var cultureInfo = new CultureInfo(lang);

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            context.Items["lang"] = lang;

            await _next(context);
        }
    }
}