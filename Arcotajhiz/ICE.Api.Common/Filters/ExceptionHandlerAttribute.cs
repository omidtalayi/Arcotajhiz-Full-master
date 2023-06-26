using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;
using ARCO.Api.Common.Class;
using ARCO.Api.Common.Enums;
using ARCO.Api.Common.Models;

namespace ARCO.Api.Common.Filters
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var actionResponse = new ActionResponse();
            actionResponse.State = ResponseStateEnum.FAILED;
            var response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError,
                actionResponse, new JsonMediaTypeFormatter());
            base.OnException(actionExecutedContext);
            General.LogError(actionExecutedContext.Exception,Guid.NewGuid(),Ip:HttpContext.Current.Request.UserHostAddress,RequestUrl: HttpContext.Current.Request.Url.AbsoluteUri);
            actionExecutedContext.Response = response;
        }
    }
}