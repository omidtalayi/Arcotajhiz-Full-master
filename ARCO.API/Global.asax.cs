using ARCO.Api.Presentation;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ARCO.API
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalConfiguration.Configuration.Filters);
        }
    }
}
