using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using ARCO.Api.Common.Enums;
using ARCO.Api.Common.Models;
using ARCO.Api.Presentation.Models.User;


namespace ARCO.Api.Presentation.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class APIAuthorizeAttribute : AuthorizeAttribute
    {
        //public string[] roles { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                HandleUnauthorizedRequest(actionContext);
            var routeValues = actionContext.Request.GetRouteData();
            var user = routeValues.Values["CurrentUser"] as CurrentUser;
            bool isAuthorized = false;
            if (user != null && user.apis != null && user.apis.Any())
            {
                foreach (var apiItem in user.apis)
                {
                    if (base.Roles.Contains(apiItem.EnumName))
                    {
                        isAuthorized = true;
                        break;
                    }
                }
                if (isAuthorized)
                {
                    return;
                }
            }
            base.OnAuthorization(actionContext);

        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionUnauthorizedContext)
        {
            var actionResponse = new ActionResponse();
            actionResponse.State = ResponseStateEnum.NOTAUTHORIZED;
            actionResponse.Errors.Add("You are unauthorized to access this Api. call with manager if something is wrong.");
            var response = actionUnauthorizedContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                actionResponse, new JsonMediaTypeFormatter());
            actionUnauthorizedContext.Response = response;
        }
    }
}