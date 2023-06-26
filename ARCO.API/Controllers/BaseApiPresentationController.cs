using ARCO.Api.Common.Controller;
using System.Net.Http;
using ARCO.Api.Presentation.Models.User;

namespace ARCO.API.Controllers
{
    public class BaseApiPresentationController : BaseApiController
    {

        public Api.Presentation.Models.User.CurrentUser CurrentUser
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