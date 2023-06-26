using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ICE.Api.Common.Models;
using ICE.Api.Firm.Filters;
using ICE.Entities.Enums;
using ICE.Presentation.Common;
using ResponseStateEnum = ICE.Api.Common.Enums.ResponseStateEnum;
using System.Web.Script.Serialization;

namespace ICE.Api.Firm.Controllers.v1
{
    [RoutePrefix("api/v1/general")]
    public class V1_GeneralController : BaseApiPresentationController
    {

        [System.Web.Http.Route("GetProvinceChildrens")]
        [HttpGet]
        public IHttpActionResult GetProvinceChildrens(int vcode,LocationTypeEnum  type = LocationTypeEnum.NONE)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                var places = ICE.Business.Engine.Instance.LocationBusiness.GetLocationChildren(vcode, type);
                var jsonStr = new JavaScriptSerializer().Serialize(places);
                response.Data = jsonStr;
                response.State = ResponseStateEnum.SUCCESS;
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }


        [System.Web.Http.Route("AddMonth")]
        [HttpGet]
        public IHttpActionResult AddMonth(int jDate ,int addMonthCount = 1)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                var result = Business.Engine.Instance.ApplicationBusiness.AddMonth(Jdate: jDate.ToString(), month: addMonthCount);
                response.Data = result;
                response.State = ResponseStateEnum.SUCCESS;
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }  
        
        [System.Web.Http.Route("GetServerDate")]
        [HttpGet]
        public IHttpActionResult GetServerDate()
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                var result = DateTime.Now.ToString();
                response.Data = result;
                response.State = ResponseStateEnum.SUCCESS;
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [System.Web.Http.Route("GetSystemIsDisabled")]
        [HttpGet]
        public IHttpActionResult GetSystemIsDisabled()
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                string ShowSystemIsDisabledStr = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("ShowSystemIsDisabled");
                string DisableReportStr = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("DisableReport");
                bool ShowSystemIsDisabled = (ShowSystemIsDisabledStr == "1");
                bool DisableReport = (DisableReportStr == "1");
                var result = new { ShowSystemIsDisabled = ShowSystemIsDisabled, DisableReport = DisableReport };
                var jsonStr = new JavaScriptSerializer().Serialize(result);
                response.Data = jsonStr;
                response.State = ResponseStateEnum.SUCCESS;
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
    }
}