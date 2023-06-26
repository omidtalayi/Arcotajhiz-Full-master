using ICE.Api.Common.Models;
using ICE.Api.Firm.Filters;
using ICE.Api.Firm.Models.Request.v1;
using ICE.Business;
using ICE.Business.Models;
using ICE.Entities.Enums;
using ICE.Presentation.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ICE.Api.Firm.Controllers.v1
{
    [RoutePrefix("api/v1/firmRequest")]
    public class V1_FirmRequestController : BaseApiPresentationController
    {
        [Route("GetUserfirmRequests")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetUserfirmRequests()
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                var objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                var firmRequestList = Engine.Instance.FirmBusiness.GetRequestRegistartions(firmRegistrationVCode: objCurrentUser.FirmRegistrationVCode,RequestregistrationTypeVCode : 1);
                var jsonStr = new JavaScriptSerializer().Serialize(new { firmRequestList = firmRequestList });
                response.Data = jsonStr;
                response.State = Common.Enums.ResponseStateEnum.SUCCESS;
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = Common.Enums.ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("AddFirmRequest")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult AddFirmRequest([FromBody] FirmRequestChargeRequest body)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            string strResponse = "";
            try
            {
                int RequestVCode = 0;
                int vcode = 0;
                var objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                long cellPhoneVerifyCode = 0;
                var objFirmRegisterationData = new RequestRegistrationData();
                var objFirmData = Engine.Instance.FirmBusiness.GetFirmRegistration(vcode: objCurrentUser.FirmRegistrationVCode);
                objFirmRegisterationData.FirmRegistrationVCode = objFirmData.VCode;
                objFirmRegisterationData.Address = body.Address;
                objFirmRegisterationData.ContactPointFamilyName = body.ContactPointFamilyName;
                objFirmRegisterationData.ContactPointName = body.ContactPointName;
                objFirmRegisterationData.FirmName = body.FirmName;
                objFirmRegisterationData.ContactPointCellphone = objFirmData.ContactPointCellphone;
                objFirmRegisterationData.Email = objFirmData.Email;
                objFirmRegisterationData.LocationVCode = (int)body.cityVCode;
                objFirmRegisterationData.PostalCode = body.FirmPostalCode;
                objFirmRegisterationData.RegistrationNo = body.RegistrationNo;
                objFirmRegisterationData.RegisteredIdentificationNo = body.RegisteredIdentificationNo;
                objFirmRegisterationData.Telephone = body.Tel;
                objFirmRegisterationData.RequestRegistrationStateVCode = RequestRegistrationStateEnum.PENDING;
                var result = Engine.Instance.FirmBusiness.RequestRegistrationIns(VCode: ref RequestVCode, requestRegistration: objFirmRegisterationData);
                if (result == RequestRegistrationInsStateEnum.SUCCESSFUL && !string.IsNullOrEmpty(body.officialPaperFileName) && !string.IsNullOrEmpty(body.idCardFileName))
                {
                    var officialPaperFile = new RequestRegistrationFileData();
                    officialPaperFile.FileName = body.officialPaperFileName;
                    officialPaperFile.RequestRegistrationFileTypeVCode = RequestRegistrationFileTypeEnum.OFFICIAL_PAPER_OR_WORKING_PERMIT;
                    officialPaperFile.RequestRegistrationVCode = RequestVCode;
                    var fileInsResult = Engine.Instance.FirmBusiness.RequestRegistrationFileIns(VCode: ref vcode, requestRegistrationFile: officialPaperFile);
                    var idCardFile = new RequestRegistrationFileData();
                    idCardFile.FileName = body.idCardFileName;
                    idCardFile.RequestRegistrationFileTypeVCode = RequestRegistrationFileTypeEnum.NATIONAL_CARD;
                    idCardFile.RequestRegistrationVCode = RequestVCode;
                    fileInsResult = Engine.Instance.FirmBusiness.RequestRegistrationFileIns(VCode: ref vcode, requestRegistrationFile: idCardFile);
                    response.State = Common.Enums.ResponseStateEnum.SUCCESS;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }

            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = Common.Enums.ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("upload")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public async Task<IHttpActionResult> Upload(RequestRegistrationFileTypeEnum fileType)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            string strResponse = "";
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    response.State = Common.Enums.ResponseStateEnum.FAILED;
                    var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                    //response.Data = "";
                    response.Errors.Add(errorEnumCode);
                    response.Data = "";
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    string fileNameInst = "";
                    var provider = new MultipartMemoryStreamProvider();
                    await Request.Content.ReadAsMultipartAsync(provider);
                    foreach (var file in provider.Contents)
                    {
                        if (string.Compare(file.Headers.ContentType.MediaType, "image/jpg", true) != 0
                               && string.Compare(file.Headers.ContentType.MediaType, "image/jpeg", true) != 0
                               && string.Compare(file.Headers.ContentType.MediaType, "image/pjpeg", true) != 0
                               && string.Compare(file.Headers.ContentType.MediaType, "image/x-png", true) != 0
                               && string.Compare(file.Headers.ContentType.MediaType, "image/png", true) != 0
                               && string.Compare(file.Headers.ContentType.MediaType, "application/pdf", true) != 0)
                        {
                            response.Data = "";
                            response.Errors.Add("پسوند فایل درست نیست.");
                            objResponse = Request.CreateResponse(HttpStatusCode.BadRequest, response);
                        }
                        else
                        {
                            var random = new Random();
                            if (fileType == RequestRegistrationFileTypeEnum.NATIONAL_CARD)
                            {
                                fileNameInst = "idCard-" + (char)(65m + Math.Round((decimal)random.Next(0, 10), 0)) + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                            }
                            else
                            {
                                fileNameInst = "officialPaper-" + (char)(65m + Math.Round((decimal)random.Next(0, 10), 0)) + DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                            }
                            var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                            var buffer = await file.ReadAsByteArrayAsync();
                            string mimeType = "." + file.Headers.ContentType.MediaType.Split('/')[1];
                            using (FileStream srReadFile = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/Attachments/" + fileNameInst + mimeType), FileMode.Create))
                            {
                                using (BinaryWriter writer = new BinaryWriter(srReadFile))
                                {
                                    writer.Write(buffer);
                                    writer.Close();
                                    response.Data = fileNameInst + mimeType;
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = Common.Enums.ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }

            return ResponseMessage(objResponse);

        }

        [Route("AddFirmApiRequest")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult AddFirmApiRequest([FromBody] RequestRegistrationData body)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                int RequestVCode = 0;
                var objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                body.RequestRegistrationStateVCode = RequestRegistrationStateEnum.PENDING;
                var userData = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                var firmRegisterationData = Engine.Instance.FirmBusiness.GetFirmRegistration(userData.FirmRegistrationVCode);
                body.ContactPointCellphone = firmRegisterationData.ContactPointCellphone;
                body.FirmRegistrationVCode = firmRegisterationData.VCode;
                body.RequestRegistrationTypeVCode = RequestRegistrationTypeEnum.REQUEST_API_PANEL;
                body.EstablishedJDate.Replace("/", "");
                body.EstablishedJDateNewspaperJDate.Replace("/", "");
                var result = Engine.Instance.FirmBusiness.RequestRegistrationIns(VCode: ref RequestVCode, requestRegistration: body);
                if (result == RequestRegistrationInsStateEnum.SUCCESSFUL)
                {
                    response.State = Common.Enums.ResponseStateEnum.SUCCESS;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = Common.Enums.ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("GetUserfirmApiRequest")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetUserfirmApiRequest()
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                var objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                List<RequestRegistrationData> firmRequestList = Engine.Instance.FirmBusiness.GetRequestRegistartions(firmRegistrationVCode: objCurrentUser.FirmRegistrationVCode, RequestregistrationTypeVCode: (short)RequestRegistrationTypeEnum.REQUEST_API_PANEL);
                if (firmRequestList != null && firmRequestList.Count > 0)
                {
                    firmRequestList.RemoveAll(s => (int)s.RequestRegistrationStateVCode == 3);
                    if (firmRequestList != null && firmRequestList.Count > 0)
                    {
                        firmRequestList.Sort((t1, t2) => t2.VCode.CompareTo(t1.VCode));
                        var firmExpireData = ICE.Business.Engine.Instance.FirmBusiness.IsFirmApiExpired(userVCode: CurrentUser.VCode);
                        var jsonStr = new JavaScriptSerializer().Serialize(new { firmRequestList = firmRequestList[0],accessToSubmit = true, firmExpireData = firmExpireData });
                        response.Data = jsonStr;
                    }
                    else
                    {
                        var jsonStr = new JavaScriptSerializer().Serialize(new { firmRequestList = "", accessToSubmit = false });
                        response.Data = jsonStr;
                    }
                }
                else
                {
                    firmRequestList = Engine.Instance.FirmBusiness.GetRequestRegistartions(firmRegistrationVCode: objCurrentUser.FirmRegistrationVCode, RequestregistrationTypeVCode: (short)RequestRegistrationTypeEnum.REQUEST_CHANGE);
                    if (firmRequestList != null && firmRequestList.Count > 0)
                    {
                        var firmRequestAccepted = firmRequestList.Find(s => (int)s.RequestRegistrationStateVCode == 2);
                        if (firmRequestAccepted != null && firmRequestAccepted.VCode > 0)
                        {
                            var jsonStr = new JavaScriptSerializer().Serialize(new { firmRequestList = "", accessToSubmit = true });
                            response.Data = jsonStr;
                        }
                        else
                        {
                            var jsonStr = new JavaScriptSerializer().Serialize(new { firmRequestList = "", accessToSubmit = false });
                            response.Data = jsonStr;
                        }
                    }
                    else
                    {
                        var jsonStr = new JavaScriptSerializer().Serialize(new { firmRequestList = "", accessToSubmit = false });
                        response.Data = jsonStr;
                    }
                }
                response.State = Common.Enums.ResponseStateEnum.SUCCESS;
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = Common.Enums.ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("SetUserfirmApiRequestNotPaid")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult SetUserfirmApiRequestNotPaid(long VCode)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                var objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                var result = Engine.Instance.FirmBusiness.RequestRegistrationStateNotPaid(vcode: VCode);
                if (result)
                {
                    response.Data = "قراردادها با موافقت خودتان تایید شد.";
                    response.State = Common.Enums.ResponseStateEnum.SUCCESS;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.Data = "خطایی پیش آمده لطفا ورودی های خود را بررسی نمایید.";
                    response.State = Common.Enums.ResponseStateEnum.FAILED;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }

            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = Common.Enums.ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("UpdateUserWebhookAndIps")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult UpdateUserWebhookAndIps([FromBody] RequestUpdateUserWebhookAndIps body)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                var result = Engine.Instance.UserBusiness.UpdateUserWebhook(userVCode: CurrentUser.VCode,body.WebHook);
               
                if (result)
                {
                    if (!string.IsNullOrEmpty(body.Ips))
                    {
                        Engine.Instance.FirmBusiness.UpdateRequestRegistrationApiIPList(vcode: body.RequestRegistrationVCode, body.Ips);
                    }
                    response.Data = "اطلاعات با موفقیت ثبت شد.";
                    response.State = Common.Enums.ResponseStateEnum.SUCCESS;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.Data = "خطایی پیش آمده لطفا ورودی های خود را بررسی نمایید.";
                    response.State = Common.Enums.ResponseStateEnum.FAILED;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }

            }
            catch (Exception ex)
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.SERVER_ERROR).ToString();
                response.State = Common.Enums.ResponseStateEnum.FAILED;
                response.Errors.Add(errorEnumCode);
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
    }
}