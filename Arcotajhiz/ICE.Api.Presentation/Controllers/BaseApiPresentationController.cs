using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using ICE.Api.Common.Controller;
using ICE.Api.Presentation.Models.User;

namespace ICE.Api.Presentation.Controllers
{
    public class BaseApiPresentationController : BaseApiController
    {

        public bool OTPFromICS24 = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["OTPFromICS24"]);
        public Models.User.CurrentUser CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;
                var routeValues = Request.GetRouteData();
                var user = routeValues.Values["CurrentUser"] as CurrentUser;
                return user;
            }
        }
    }
}