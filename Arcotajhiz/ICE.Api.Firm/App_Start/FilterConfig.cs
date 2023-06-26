
using System.Web.Http.Filters;
using ICE.Api.Common.Filters;

namespace ICE.Api.Firm
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());
        }
    }
}
