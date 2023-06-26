using ICE.Presentation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ICE.Api.Presentation.Models
{
    public class ApiLogHandler: DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // log request body
                string requestBody = await request.Content.ReadAsStringAsync();
                //Trace.WriteLine(requestBody);

                ICE.Business.Models.ApiLogData objApiLogData = new Business.Models.ApiLogData();
                objApiLogData.Request = requestBody;
                objApiLogData.IP = HttpContext.Current.Request.UserHostAddress;
                objApiLogData.UserVCode = 0;
                objApiLogData.MethodName = HttpContext.Current.Request.RawUrl;
                // let other handlers process the request
                var result = await base.SendAsync(request, cancellationToken);
                objApiLogData.State = result.StatusCode.ToString();
                if (result.Content != null)
                {
                    // once response body is ready, log it
                    var responseBody = await result.Content.ReadAsStringAsync();
                    objApiLogData.Response = responseBody;
                    //Trace.WriteLine(responseBody);
                }
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    objApiLogData.UserVCode = long.Parse((((System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity).Claims).ToList()[1].Value);
                }

                ICE.Business.Engine.Instance.LogBusiness.ApiLogIns(apilog: objApiLogData);
                return result;
            }
            catch (Exception ex)
            {
                General.LogError(error: ex, request: HttpContext.Current.Request);
                return null;
            }
            
        }
    }
}