using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using ARCO.Api.Presentation.Models.User;
using ARCO.Api.Presentation.Modules;
using ARCO.Business.Models;
using ARCO.Entities.Enums;
using Newtonsoft.Json;

namespace ARCO.Api.Presentation.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing JWT Token", request);
                return;
            }
            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing JWT Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token, context);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid Token",request);
                return;
            }
            else
            {
                context.Principal = principal;

                var identity = principal?.Identity as ClaimsIdentity;
                var usernameClaim = identity?.FindFirst(ClaimTypes.Name);
                var userVCodeClaim = identity?.FindFirst("VCode");
                //string[] userApisClaim = identity?.FindFirst("apis").Value.Split(',');
                //var authenticatedUser = new CurrentUser()
                //{
                //    UserName = usernameClaim?.Value,
                //    VCode = long.Parse(userVCodeClaim?.Value),
                //    apis = userApisClaim
                //};
                //context.ActionContext.RequestContext.RouteData.Values.Add("CurrentUser", authenticatedUser);
            }
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token, HttpAuthenticationContext context)
        {
            var simplePrinciple = JwtManager.GetPrincipal(token,context);
            return Task.FromResult<IPrincipal>(simplePrinciple);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            //string parameter = null;

            //if (!string.IsNullOrEmpty(Realm))
            //    parameter = "realm=\"" + Realm + "\"";

            //context.ChallengeWith("Bearer", parameter);
        }
    }
}