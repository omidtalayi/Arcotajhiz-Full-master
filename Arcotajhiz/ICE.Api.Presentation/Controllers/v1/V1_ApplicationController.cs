using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using ICE.Entities.Enums;
using ICE.Presentation.Common;
using ICE.Api.Common;
using ICE.Api.Common.Controller;
using ICE.Api.Common.Models;
using ICE.Api.Presentation.Filters;
using Newtonsoft.Json;
using ICE.Api.Presentation.Models.Request.v1;
using ResponseStateEnum = ICE.Api.Common.Enums.ResponseStateEnum;
using ICE.Business.Models;
using System.Collections.Generic;
using ICE.Api.Presentation.Models.Response;

namespace ICE.Api.Presentation.Controllers.v1
{
    [RoutePrefix("api/v1/application")]
    public class v1_ApplicationController : BaseApiPresentationController
    {

        [Route("ApiIsDisabled")]
        [HttpGet]
        public IHttpActionResult ApiIsDisabled()
        {
            bool isDisabled = false;
            var response = new ActionResponse<ApiIsDisabledResponse>();
            try
            {
                var ApiIsDisabled = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("ApiIsDisabled");
                if (!string.IsNullOrEmpty(ApiIsDisabled))
                {
                    bool isBoolean = false;
                    if (bool.TryParse(ApiIsDisabled, out isBoolean) && isBoolean)
                    {
                        isDisabled = true;
                    }
                }
                ApiIsDisabledResponse objApiIsDisabledResponse = new ApiIsDisabledResponse();
                objApiIsDisabledResponse.IsDisabled = isDisabled;
                response.Data = objApiIsDisabledResponse;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }


        }

        [Route("sendsms")]
        [HttpPost]
        [JwtAuthentication]
        public IHttpActionResult SendSms([FromBody]SendSmsRequest body)
        {
            var response = new ActionResponse();
            var request = new CheckIndividualReportExistenceRequest();
            try
            {
                var apiErrorEnum = body.Validation();
                if (apiErrorEnum != Entities.Enums.ApiErrorCodeEnum.NONE)
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(apiErrorEnum.ToString());
                  
                }
                else
                {
                    var responseData = ICE.Business.Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: body.Cellphone, message: body.Message, ip: HttpContext.Current.Request.UserHostAddress);
                    response.State = ResponseStateEnum.SUCCESS;
                }
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
        [Route("geticsreportsources")]
        [HttpPost]
        public IHttpActionResult GetICSReportSources()
        {
            var response = new ActionResponse<List<ICSReportSourceData>>();
            var request = new CheckIndividualReportExistenceRequest();
            try
            {
                var responseData = ICE.Business.Engine.Instance.ApiBusiness.GetICSReportSources();
                response.Data = responseData;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
    }
}
