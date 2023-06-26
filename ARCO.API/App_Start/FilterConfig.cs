using ARCO.Api.Common.Filters;
using System.Web;
using System.Web.Http.Filters;

namespace ARCO.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());
        }
    }
}
