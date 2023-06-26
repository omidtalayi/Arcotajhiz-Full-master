
namespace ARCO.API.Controllers
{
    public class ApplicationController : BaseApiPresentationController
    {
        //[Route("ApiIsDisabled")]
        //[HttpGet]
        //public IHttpActionResult ApiIsDisabled()
        //{
        //    bool isDisabled = false;
        //    var response = new ActionResponse<ApiIsDisabledResponse>();
        //    try
        //    {
        //        var ApiIsDisabled = ARCO.Business.Engine.Instance.ApplicationBusiness.GetSetting("ApiIsDisabled");
        //        if (!string.IsNullOrEmpty(ApiIsDisabled))
        //        {
        //            bool isBoolean = false;
        //            if (bool.TryParse(ApiIsDisabled, out isBoolean) && isBoolean)
        //            {
        //                isDisabled = true;
        //            }
        //        }
        //        ApiIsDisabledResponse objApiIsDisabledResponse = new ApiIsDisabledResponse();
        //        objApiIsDisabledResponse.IsDisabled = isDisabled;
        //        response.Data = objApiIsDisabledResponse;
        //        response.State = ResponseStateEnum.SUCCESS;
        //        var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //        return ResponseMessage(objResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.State = ResponseStateEnum.FAILED;
        //        response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
        //        General.LogError(error: ex, request: HttpContext.Current.Request);
        //        var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //        return ResponseMessage(objResponse);
        //    }


        //}

        //[Route("sendsms")]
        //[HttpPost]
        //[JwtAuthentication]
        //public IHttpActionResult SendSms([FromBody] SendSmsRequest body)
        //{
        //    var response = new ActionResponse();
        //    var request = new CheckIndividualReportExistenceRequest();
        //    try
        //    {
        //        var apiErrorEnum = body.Validation();
        //        if (apiErrorEnum != Entities.Enums.ApiErrorCodeEnum.NONE)
        //        {
        //            response.State = ResponseStateEnum.FAILED;
        //            response.Errors.Add(apiErrorEnum.ToString());

        //        }
        //        else
        //        {
        //            var responseData = ARCO.Business.Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: body.Cellphone, message: body.Message, ip: HttpContext.Current.Request.UserHostAddress);
        //            response.State = ResponseStateEnum.SUCCESS;
        //        }
        //        var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //        return ResponseMessage(objResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.State = ResponseStateEnum.FAILED;
        //        response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
        //        General.LogError(error: ex, request: HttpContext.Current.Request);
        //        var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //        return ResponseMessage(objResponse);
        //    }
        //}
        //[Route("geticsreportsources")]
        //[HttpPost]
        //public IHttpActionResult GetICSReportSources()
        //{
        //    var response = new ActionResponse<List<ICSReportSourceData>>();
        //    var request = new CheckIndividualReportExistenceRequest();
        //    try
        //    {
        //        var responseData = ARCO.Business.Engine.Instance.ApiBusiness.GetICSReportSources();
        //        response.Data = responseData;
        //        response.State = ResponseStateEnum.SUCCESS;
        //        var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //        return ResponseMessage(objResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.State = ResponseStateEnum.FAILED;
        //        response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
        //        General.LogError(error: ex, request: HttpContext.Current.Request);
        //        var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //        return ResponseMessage(objResponse);
        //    }
        //}
    }
}