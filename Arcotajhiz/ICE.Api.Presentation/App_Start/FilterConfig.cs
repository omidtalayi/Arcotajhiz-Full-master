using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using ICE.Api.Common.Filters;
using ICE.Api.Presentation.Filters;

namespace ICE.Api.Presentation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());
            //filters.Add(new APIAuthorizeAttribute());
            //filters.Add(new JwtAuthenticationAttribute());
        }
    }
}