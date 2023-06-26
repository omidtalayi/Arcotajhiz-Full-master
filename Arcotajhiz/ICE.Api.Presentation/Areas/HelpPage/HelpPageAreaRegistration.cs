using System;
using System.Web.Http;
using System.Web.Mvc;
namespace ICE.Api.Presentation.Areas.HelpPage
{
    public class HelpPageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HelpPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            Boolean isConverted = Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings.Get("showHelp"),out bool showResult);
            if (isConverted && showResult)
            {
                context.MapRoute(
                    "HelpPage_Default",
                    "Help/{action}/{apiId}",
                    new { controller = "Help", action = "Index", apiId = UrlParameter.Optional });
                HelpPageConfig.Register(GlobalConfiguration.Configuration);
            }
            
        }
    }
}