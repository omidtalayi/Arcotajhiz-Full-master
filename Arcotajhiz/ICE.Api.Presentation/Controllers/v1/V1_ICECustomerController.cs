using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using ICE.Api.Common.Enums;
using ICE.Api.Common.Models;
using ICE.Api.Presentation.Filters;
using ICE.Api.Presentation.Models;
using ICE.Api.Presentation.Models.Request.v1;
using ICE.Business;
using ICE.Business.Models;
using ICE.Entities.Enums;
using ICE.Presentation.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Web.Configuration;
using ResponseStateEnum = ICE.Api.Common.Enums.ResponseStateEnum;
using ICE.Api.Presentation.Models.Response;
using System.Xml;
using System.IO;
using System.Web.Script.Serialization;

namespace ICE.Api.Presentation.Controllers.v1
{

    [RoutePrefix("api/v1/customer")]
    public class V1_ICECustomerController : BaseApiPresentationController
    {

        [Route("IndividualReportExistence")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.INDIVIDUAL_REPORT_EXISTENCE))]
        public IHttpActionResult CheckIndividualReportExistence([FromBody] CheckIndividualReportExistenceRequest body)
        {
            HttpResponseMessage objResponse;
            var response = new ActionResponse<List<ReportSourceData>>();
            var request = new CheckIndividualReportExistenceRequest();
            try
            {
                var ApiIsDisabled = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("ApiIsDisabled");
                if (!string.IsNullOrEmpty(ApiIsDisabled))
                {
                    bool isBoolean = false;
                    if (bool.TryParse(ApiIsDisabled, out isBoolean) && isBoolean)
                    {
                        var responseData = new List<ReportSourceData>();
                        response.Data = responseData;
                        response.State = ResponseStateEnum.FAILED;
                        response.Errors.Add(ApiErrorCodeEnum.API_IS_DISABLED.ToString());
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        return ResponseMessage(objResponse);
                    }
                }

                var apiErrorEnum = body.Validation();
                if (apiErrorEnum != ApiErrorCodeEnum.NONE)
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(apiErrorEnum.ToString());
                }
                else
                {
                    var ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.UserHostAddress != null)
                    {
                        ip = HttpContext.Current.Request.UserHostAddress;
                    }
                    var responseData = ICE.Business.Engine.Instance.ApiBusiness.CheckIndividualReportExistence(userVCode: CurrentUser.VCode, nationalCode: body.NationalCode, cellphone: body.Cellphone, reportSources: body.ReportSources, ip: ip, receiverCellphone: body.ReceiverCellphone);
                    response.Data = responseData;
                    response.State = ResponseStateEnum.SUCCESS;
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
        [Route("FirmReportExistence")]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.INDIVIDUAL_REPORT_EXISTENCE))]
        public IHttpActionResult CheckFirmReportExistence([FromBody] CheckFirmReportExistenceRequest body)
        {
            HttpResponseMessage objResponse;
            var response = new ActionResponse<List<ReportSourceData>>();
            try
            {
                var ApiIsDisabled = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("ApiIsDisabled");
                if (!string.IsNullOrEmpty(ApiIsDisabled))
                {
                    bool isBoolean = false;
                    if (bool.TryParse(ApiIsDisabled, out isBoolean) && isBoolean)
                    {
                        var responseData = new List<ReportSourceData>();
                        response.Data = responseData;
                        response.State = ResponseStateEnum.FAILED;
                        response.Errors.Add(ApiErrorCodeEnum.API_IS_DISABLED.ToString());
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        return ResponseMessage(objResponse);
                    }
                }

                var apiErrorEnum = body.Validation();
                if (apiErrorEnum != ApiErrorCodeEnum.NONE)
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(apiErrorEnum.ToString());
                }
                else
                {
                    var responseData = ICE.Business.Engine.Instance.ApiBusiness.CheckFirmReportExistence(userVCode: CurrentUser.VCode, nationalCode: body.NationalCode, companyNationalID: body.CompanyNationalID, cellphone: body.Cellphone, reportSources: body.ReportSources, ip: HttpContext.Current.Request.UserHostAddress, receiverCellphone: body.ReceiverCellphone);
                    response.Data = responseData;
                    response.State = ResponseStateEnum.SUCCESS;
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
        [Route("GetIndividualInformation")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GET_INDIVIDUAL_INFORMATION))]
        public IHttpActionResult GetIndividualInformation([FromBody] GetIndividualInformationRequest body)
        {

            HttpResponseMessage objResponse = null;
            var response = new ActionResponse();
            try
            {
                var apiErrorEnum = body.Validation();
                if (apiErrorEnum != Entities.Enums.ApiErrorCodeEnum.NONE)
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(apiErrorEnum.ToString());
                    return Json(new { response = JsonConvert.SerializeObject(response) });
                }
                long identificationVCode = 0;
                IdentificationData identification;
                DateTime reportExpirationDate = DateTime.MinValue;
                UserData user = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                var reportLinkAndExpirationDate = Business.Engine.Instance.IdentificationBusiness.GetReportLinkAndExpirationDate(cellphone: body.Cellphone, nationalCode: body.NationalCode, UserVCode: user.VCode, receiverCellphone: body.ReceiverCellphone);
                if (reportLinkAndExpirationDate != null)
                {
                    reportExpirationDate = reportLinkAndExpirationDate.ReportExpirationDate;
                    identificationVCode = reportLinkAndExpirationDate.IdentificationVCode;
                }
                if (reportExpirationDate != DateTime.MinValue && reportExpirationDate > DateTime.Now)
                {
                    identification = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(vcode: identificationVCode);
                    var message = "";
                    if (identification != null)
                    {
                        if (!string.IsNullOrEmpty(identification.ReportLink))
                        {
                            message = "آیس : " + "\n" + "لینک گزارش اعتبارسنجی" + "\n" + "Https://iceScoring.com/" + identification.ReportLink;
                            if (reportLinkAndExpirationDate != null && (string.IsNullOrEmpty(reportLinkAndExpirationDate.ReportLink) || string.IsNullOrEmpty(identification.RedirectUrlICS24)))
                            {
                                if (!string.IsNullOrEmpty(body.Cellphone) && !string.IsNullOrEmpty(message))
                                {
                                    //sms change to ghasedak
                                    ICE.Business.Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: body.Cellphone, message: message, ip: HttpContext.Current.Request.UserHostAddress, identificationVCode: identification.VCode, typeVCode: SMSTypeEnum.REPORT_LINK, operatorType: OperatorTypeEnum.GHASEDAK);
                                }
                            }
                        }
                    }
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                    response.State = ResponseStateEnum.SUCCESS;
                }
                else
                {
                    bool state = false;
                    bool responseData = ICE.Business.Engine.Instance.ApiBusiness.GetCustomerInformation(userVCode: CurrentUser.VCode, nationalCode: body.NationalCode, cellphone: body.Cellphone, reportSources: body.ReportSources, ip: HttpContext.Current.Request.UserHostAddress, saleAmount: body.SaleAmount, saleRefID: body.SalesRefID, IdentificationVCode: ref identificationVCode, state: ref state);
                    if (state)
                    {
                        if (responseData && identificationVCode > 0)
                        {
                            identification = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(vcode: identificationVCode);
                            response.State = ResponseStateEnum.SUCCESS;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                        else
                        {
                            response.State = ResponseStateEnum.FAILED;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                    }
                    else
                    {
                        response.State = ResponseStateEnum.FAILED;
                        response.Errors.Add(ApiErrorCodeEnum.SALES_REFID_IS_INVALID.ToString());
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                    }

                }

            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                General.LogError(error: ex, request: (HttpContext.Current != null ? HttpContext.Current.Request : null));
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
        [Route("SendReportUrlToCustomer")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.SEND_REPORT_URL_TO_CUSTOMER))]
        public IHttpActionResult SendReportUrlToCustomer([FromBody] SendReportUrlToCustomerRequest body)
        {
            HttpResponseMessage objResponse;
            var response = new ActionResponse();
            var request = new SendReportUrlToCustomerRequest();
            string message = null;
            DateTime ReportDate = default(DateTime);
            try
            {
                var apiErrorEnum = Entities.Enums.ApiErrorCodeEnum.NONE;
                if (apiErrorEnum != Entities.Enums.ApiErrorCodeEnum.NONE)
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(apiErrorEnum.ToString());
                }
                else
                {
                    long CreditRiskReportVCode = 0;
                    string TrackingCode = "";
                    bool ICSNewApi = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("ICSNewApi") == "1";
                    if (ICSNewApi)
                    {
                        var addCreditRiskReportResult = ICE.Business.Engine.Instance.CreditRiskReportBusiness.AddCreditRiskReport(vcode: ref CreditRiskReportVCode, identificationVCode: body.IdentificationVCode, entryDate: ref ReportDate, trackingCode: ref TrackingCode, hasShahkarIdentified: body.HasShahkarIdentified, isLegalPerson: body.IsLegalPerson);
                    }
                    else
                    {
                        var addCreditRiskReportResult = ICE.Business.Engine.Instance.CreditRiskReportBusiness.AddCreditRiskReport(vcode: ref CreditRiskReportVCode, identificationVCode: body.IdentificationVCode, entryDate: ref ReportDate, trackingCode: ref TrackingCode);
                    }
                    IdentificationData identification =
                    ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(
                       vcode: body.IdentificationVCode);
                    UserData user = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: identification.UserVCode);
                    message = "آیس : " + "\n" + "لینک گزارش اعتبار سنجی" + "\n" + "Https://iceScoring.com" + identification.ReportLink;
                    if (user != null)
                    {
                        if (user.SendLinkUrlSms)
                        {
                            if (!string.IsNullOrEmpty(identification.Cellphone) && !string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(identification.ReportLink))
                            {
                                ICE.Business.Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: (!string.IsNullOrEmpty(identification.SecondCellphone) ? identification.SecondCellphone : identification.Cellphone), message: message, ip: HttpContext.Current.Request.UserHostAddress, identificationVCode: (identification != null ? identification.VCode : 0), typeVCode: SMSTypeEnum.REPORT_LINK);
                            }
                        }
                        if (user.WebHooks != null && user.WebHooks.Any())
                        {
                            WebHookCall(user: user, identification: identification, message: message);
                        }
                    }
                    response.State = ResponseStateEnum.SUCCESS;
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
        [Route("KycCheck")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.KYC_CHECK))]
        public IHttpActionResult KycCheck([FromBody] KycCheckRequest body)
        {
            HttpResponseMessage objResponse;
            var response = new ActionResponse<KycData>();
            var request = new KycCheckRequest();
            try
            {
                var apiErrorEnum = body.Validation();
                if (apiErrorEnum != ApiErrorCodeEnum.NONE)
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(apiErrorEnum.ToString());
                }
                else
                {
                    var responseData = ICE.Business.Engine.Instance.ApiBusiness.CheckKyc(userVCode: CurrentUser.VCode, nationalCode: body.NationalCode, cellphone: body.Cellphone, ip: HttpContext.Current.Request.UserHostAddress);
                    response.Data = responseData;
                    response.State = ResponseStateEnum.SUCCESS;
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
        [Route("SetIndividualInformationRequest")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.SET_INDIVIDUAL_INFORMATION_REQUEST))]
        public IHttpActionResult SetIndividualInformationRequest([FromBody] GetIndividualInformationRequest body)
        {
            HttpResponseMessage objResponse;
            var response = new ActionResponse<SetIndividualInformationResponse>();
            try
            {
                var apiErrorEnum = body.Validation();
                CryptographyBusiness cryptography = new CryptographyBusiness();
                if (apiErrorEnum != Entities.Enums.ApiErrorCodeEnum.NONE)
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(apiErrorEnum.ToString());
                    return Json(new { response = JsonConvert.SerializeObject(response) });
                }
                long identificationVCode = 0;
                IdentificationData identification;
                DateTime reportExpirationDate = DateTime.MinValue;
                UserData user = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                var reportLinkAndExpirationDate = Business.Engine.Instance.IdentificationBusiness.GetReportLinkAndExpirationDate(cellphone: body.Cellphone, nationalCode: body.NationalCode, UserVCode: user.VCode, receiverCellphone: body.ReceiverCellphone);
                if (reportLinkAndExpirationDate != null)
                {
                    reportExpirationDate = reportLinkAndExpirationDate.ReportExpirationDate;
                    identificationVCode = reportLinkAndExpirationDate.IdentificationVCode;
                }
                if (reportExpirationDate != DateTime.MinValue && reportExpirationDate > DateTime.Now)
                {
                    identification = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(vcode: identificationVCode);
                    var requestID = WebUtility.UrlEncode(cryptography.Encrypt(identificationVCode.ToString(), EncryptKeyEnum.Key1));
                    var message = "";
                    if (identification != null)
                    {
                        response.Data = new SetIndividualInformationResponse()
                        {
                            requestID = requestID,
                            reportLink = string.IsNullOrEmpty(identification.ReportLink) ? "" : "https://icescoring.com" + identification.ReportLink,
                            responseMessage = "گزارش فرد مورد نظر در ۲۴ گذشته گرفته شده و همچنان اعتبار دارد."
                        };
                    }
                    response.State = ResponseStateEnum.SUCCESS;
                }
                else
                {
                    bool state = false;
                    bool responseData = ICE.Business.Engine.Instance.ApiBusiness.GetCustomerInformation(userVCode: CurrentUser.VCode, nationalCode: body.NationalCode, cellphone: body.Cellphone, reportSources: body.ReportSources, ip: HttpContext.Current.Request.UserHostAddress, saleAmount: body.SaleAmount, saleRefID: body.SalesRefID, IdentificationVCode: ref identificationVCode, state: ref state);
                    if (state)
                    {
                        if (responseData && identificationVCode > 0)
                        {
                            identification = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(vcode: identificationVCode);

                            var requestID = WebUtility.UrlEncode(cryptography.Encrypt(identificationVCode.ToString(), EncryptKeyEnum.Key1));
                            var responseResult = ApiCall.Instance.AppIcs24Request(body.Cellphone, body.NationalCode);
                            if (responseResult != null && !(bool)responseResult["hasError"])
                            {
                                var appIcs24HashCode = (responseResult["data"] != null ? responseResult["data"].ToString() : "");
                                if (!string.IsNullOrEmpty(appIcs24HashCode))
                                {
                                    Business.Engine.Instance.IdentificationBusiness.SetIdentificationAppICS24HashCode(identificationVCode: identification.VCode, appIcs24HashCode: appIcs24HashCode);
                                    Business.Engine.Instance.IdentificationBusiness.UpdateExpirationDate(identification.VCode, DateTime.Now.AddSeconds(120));
                                }
                            }
                            response.Data = new SetIndividualInformationResponse()
                            {
                                requestID = requestID,
                                codeExpireTime = 90
                            };
                            response.State = ResponseStateEnum.SUCCESS;
                        }
                    }
                    else
                    {
                        response.State = ResponseStateEnum.FAILED;
                        response.Errors.Add(ApiErrorCodeEnum.SALES_REFID_IS_INVALID.ToString());
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                    }

                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(((int)ApiErrorCodeEnum.SERVER_ERROR).ToString());
                General.LogError(error: ex, request: (HttpContext.Current != null ? HttpContext.Current.Request : null));
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
        [Route("VerifyCellPhone")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.VERIFY_CELLPHONE))]
        public IHttpActionResult VerifyCellPhone([FromBody] VerifyCellPhoneResquest body)
        {
            HttpResponseMessage objResponse;
            var response = new ActionResponse<VerifyCellPhoneResponse>();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            object returnMessage;
            try
            {
                long identificationVCode = long.Parse(cryptography.Decrypt(WebUtility.UrlDecode(body.requestID), EncryptKeyEnum.Key1).ToString());
                {
                    var identification = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(vcode: identificationVCode);
                    if (identification.IdentificationState.VCode == 17)
                    {
                        var responseResult = ApiCall.Instance.AppIcs24Validate(hashcode: identification.appIcs24HashCode, code: body.verifyCode.ToString(), request: Request);
                        if (System.Convert.ToBoolean(responseResult["hasError"]))
                        {
                            if (responseResult["messages"] != null)
                            {
                                returnMessage = responseResult["messages"][0]["message"];
                                var reason = responseResult["messages"][0]["reason"].ToString();
                                switch (reason)
                                {
                                    case "ShahkarFailed":
                                        {
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationICS24Confirmed(identification.VCode);
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationHasShahkarIdentified(identificationVCode: identification.VCode, hasShahkarIdentified: false);
                                            response.Data = new VerifyCellPhoneResponse()
                                            {
                                                reportProcessingLink = "https://www.icescoring.com/identification/ReportInProcessing?id=" + body.requestID,
                                                verifyState = VerifyCellphoneStateApiEnum.SHAHKAR_FAILED,
                                                verifyStateMessage = responseResult["messages"][0]["message"].ToString()
                                            };
                                            break;
                                        }
                                    case "SherkathaFailed":
                                        {
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationICS24Confirmed(identification.VCode);
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationIsLegalPerson(identificationVCode: identification.VCode, isLegalPerson: false);
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationICS24Confirmed(identification.VCode);
                                            response.Data = new VerifyCellPhoneResponse()
                                            {
                                                reportProcessingLink = "https://www.icescoring.com/identification/ReportInProcessing?id=" + body.requestID,
                                                verifyState = VerifyCellphoneStateApiEnum.IS_LEGAL_PERSON_FAILED,
                                                verifyStateMessage = responseResult["messages"][0]["message"].ToString()
                                            };
                                            break;
                                        }
                                    case "IncorrectToken":
                                        {
                                            response.Data = new VerifyCellPhoneResponse()
                                            {
                                                reportProcessingLink = "",
                                                verifyState = VerifyCellphoneStateApiEnum.VERIFICATION_CODE_IS_NOT_VALID,
                                                verifyStateMessage = responseResult["messages"][0]["message"].ToString()
                                            };
                                            break;
                                        }
                                    case "TokenExpired":
                                        {
                                            response.Data = new VerifyCellPhoneResponse()
                                            {
                                                reportProcessingLink = "",
                                                verifyState = VerifyCellphoneStateApiEnum.VERIFICATION_CODE_IS_EXPIRED,
                                                verifyStateMessage = responseResult["messages"][0]["message"].ToString()
                                            };
                                            break;
                                        }
                                    case "OtpLocked":
                                        {
                                            Engine.Instance.IdentificationBusiness.SetIdentificationOtpLock(identification.VCode);
                                            response.Data = new VerifyCellPhoneResponse()
                                            {
                                                reportProcessingLink = "",
                                                verifyState = VerifyCellphoneStateApiEnum.REQUST_IS_LOCKED,
                                                verifyStateMessage = "به دلیل ورود بیش از حد کد اشتباه درخواست شما مسدود شده است."
                                            };
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        {
                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationICS24Confirmed(identification.VCode);
                            response.Data = new VerifyCellPhoneResponse()
                            {
                                reportProcessingLink = "https://www.icescoring.com/identification/ReportInProcessing?id=" + body.requestID,
                                verifyState = VerifyCellphoneStateApiEnum.SUCCESSFUL
                            };
                        }
                    }
                    else if (identification.IdentificationState.VCode == 19)
                    {
                        response.Data = new VerifyCellPhoneResponse()
                        {
                            reportProcessingLink = "",
                            verifyState = VerifyCellphoneStateApiEnum.REQUST_IS_LOCKED,
                            verifyStateMessage = "به دلیل ورود بیش از حد کد اشتباه درخواست شما مسدود شده است."
                        };
                    }
                    else
                    {
                        response.Data = new VerifyCellPhoneResponse()
                        {
                            reportProcessingLink = "",
                            verifyState = VerifyCellphoneStateApiEnum.REQUST_NOT_VALID_STATE,
                            verifyStateMessage = "درخواست شما امکان پذیر نیست ورودی های خود را بررسی نمایید."
                        };
                    }
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(((int)ApiErrorCodeEnum.SERVER_ERROR).ToString());
                General.LogError(error: ex, request: (HttpContext.Current != null ? HttpContext.Current.Request : null));
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
        [Route("ResendVerifyCode")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.VERIFY_CELLPHONE))]
        public IHttpActionResult ResendVerifyCode([FromBody] VerifyCellPhoneResquest body)
        {
            HttpResponseMessage objResponse;
            var response = new ActionResponse<ResendCodeIndevidualInformation>();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            object returnMessage;
            try
            {
                long identificationVCode = long.Parse(cryptography.Decrypt(WebUtility.UrlDecode(body.requestID), EncryptKeyEnum.Key1).ToString());
                var identification = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(vcode: identificationVCode);
                var responseResult = ApiCall.Instance.AppIcs24RenewToken(hashcode: identification.appIcs24HashCode, request: Request);
                if ((bool)responseResult["hasError"])
                {
                    switch (responseResult["messages"][0]["reason"].ToString().ToLower())
                    {
                        case "resendwait":
                            {
                                response.Data = new ResendCodeIndevidualInformation()
                                {
                                    requestID = body.requestID,
                                    codeExpireTime = (int)(identification.ExpirationDate > DateTime.Now ? Math.Round((identification.ExpirationDate - DateTime.Now).TotalSeconds) : 0),
                                    responseMessage = "برای این درخواست امکان دریافت توکن جدید وجود ندارد، لطفا چند لحظه دیگر مجددا تلاش کنید."
                                };
                                break;
                            }

                        case "requestproccessedbefore":
                            {
                                response.Data = new ResendCodeIndevidualInformation()
                                {
                                    requestID = body.requestID,
                                    codeExpireTime = 0,
                                    responseMessage = "این درخواست قبلا پردازش شده است، لذا انجام این عملیات امکان پذیر نیست."
                                };
                                break;
                            }
                        default:
                            {
                                response.Data = new ResendCodeIndevidualInformation()
                                {
                                    requestID = body.requestID,
                                    codeExpireTime = 0,
                                    responseMessage = responseResult["messages"] == null ? null : responseResult["messages"][0]["message"].ToString(),
                                    resendLocked = (responseResult["messages"] != null ? (responseResult["messages"][0] != null ? (responseResult["messages"][0]["reason"] != null ? responseResult["messages"][0]["reason"].ToString() == "ResendLocked" : false) : false): false) 
                                };
                                break;
                            }
                    }
                }
                else
                {
                    int expireSec = (int)responseResult["data"]["resendWait"];
                    response.Data = new ResendCodeIndevidualInformation()
                    {
                        requestID = body.requestID,
                        codeExpireTime = expireSec,
                        responseMessage = "ارسال مجدد کد با موفقیت انجام شد.",
                    };
                    Engine.Instance.IdentificationBusiness.UpdateExpirationDate(identification.VCode, DateTime.Now.AddSeconds(expireSec));
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(((int)ApiErrorCodeEnum.SERVER_ERROR).ToString());
                General.LogError(error: ex, request: (HttpContext.Current != null ? HttpContext.Current.Request : null));
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
        [Route("IndividualInformationRequestState")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.SET_INDIVIDUAL_INFORMATION_REQUEST))]
        public IHttpActionResult IndividualInformationRequestState([FromBody] IndividualInformationStateRequest body)
        {
            HttpResponseMessage objResponse;
            var response = new ActionResponse<IndividualInformationStateResponse>();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            try
            {
                long identificationVCode = long.Parse(cryptography.Decrypt(WebUtility.UrlDecode(body.requestID), EncryptKeyEnum.Key1).ToString());
                var identification = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(vcode: identificationVCode);
                if (!string.IsNullOrEmpty(identification.ReportLink))
                {
                    if (identification.ReportExpirationDate >= DateTime.Now)
                    {
                        if (identification.HasShahkarIdentified)
                        {
                            string IndividualReport = "";
                            string EmptyReport = "";
                            List<PersonageData> personageInfo = null;
                            EmptyReportData emptyInfo = null;

                            personageInfo = Engine.Instance.ICSBusiness.GetAdvancedIndividualReport(nationalCode: identification.NationalCode, identificationVCode: identification.VCode, IsNewCoreConnection: true);

                            if (personageInfo != null && personageInfo.Any())
                            {
                                string jobj = JsonConvert.SerializeObject(personageInfo);
                                //var jobj = JSObj.FromObject();
                                IndividualReport = jobj.ToString().Replace("\"", "'");
                            }
                            else
                            {
                                emptyInfo = Engine.Instance.ICSBusiness.GetEmptyCompanyReport(companyNationalID: identification.CompanyNationalID, identificationVCode: identificationVCode, IsNewCoreConnection: true);
                                string jobj = JsonConvert.SerializeObject(personageInfo);
                                EmptyReport = jobj.ToString().Replace("\"", "'");
                            }
                            response.Data = new IndividualInformationStateResponse()
                            {
                                requestID = body.requestID,
                                reportLink = "https://icescoring.com" + identification.ReportLink,
                                requestState = (IdentificationStateEnum)identification.IdentificationState.VCode,
                                reportData = IndividualReport,
                                emptyReportData = EmptyReport
                            };
                        }
                        else
                        {
                            response.Data = new IndividualInformationStateResponse()
                            {
                                requestID = body.requestID,
                                reportLink = "https://icescoring.com" + identification.ReportLink,
                                requestState = IdentificationStateEnum.SHAHKAR_REJECTED,
                                reportData = null,
                                emptyReportData = null
                            };
                        }
                    }
                    else
                    {
                        response.Data = new IndividualInformationStateResponse()
                        {
                            requestID = body.requestID,
                            reportLink = "https://icescoring.com" + identification.ReportLink,
                            requestState = IdentificationStateEnum.EXPIRED,
                            reportData = null,
                            emptyReportData = null
                        };
                    }
                }
                else
                {
                    response.Data = new IndividualInformationStateResponse()
                    {
                        requestID = body.requestID,
                        reportLink = null,
                        requestState = (IdentificationStateEnum)identification.IdentificationState.VCode,
                        reportData = null,
                        emptyReportData = null
                    };
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message) && ex.Message.ToLower() == "Invalid length for a Base-64 char array or string.".ToLower())
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add((ApiErrorCodeEnum.REPORT_ID_IS_INVALID).ToString());
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(((int)ApiErrorCodeEnum.SERVER_ERROR).ToString());
                    General.LogError(error: ex, request: (HttpContext.Current != null ? HttpContext.Current.Request : null));
                    objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
                }
            }
            return ResponseMessage(objResponse);
        }
        private void WebHookCall(UserData user, IdentificationData identification, String message)
        {
            WebHookData webhook = user.WebHooks.FirstOrDefault(x =>
                x.WebHookType == WebHookTypeEnum.NOTIFY_REPORT_URL_HAS_BEEN_SENT);
            if (webhook != null)
            {
                string url = webhook.WebHook;
                var client = new RestClient(url + "?nationalCode=" + identification.NationalCode + "&cellphone=" + identification.Cellphone + "&trackingCode=" + identification.TrackingCode + "&reportLink=https://iceScoring.com" + identification.ReportLink);
                var webHookRequest = new RestRequest(Method.GET);
                webHookRequest.AddHeader("cache-control", "no-cache");
                webHookRequest.AddHeader("Content-Type", "application/json");
                IRestResponse webHookResponse = client.Execute(webHookRequest);
                if (webHookResponse.StatusCode != HttpStatusCode.OK)
                {
                    //Exception ex = new Exception(message: "Partner webhook call has error identificationVCode=" + identification.VCode + " enum=" + webhook.WebHookType.ToString());
                    //General.LogError(error: ex, request: Request);
                }
                else
                {
                    ICE.Business.Engine.Instance.UserBusiness.UserWebHookCallIns(userVcode: user.VCode, userWebHookVCode: webhook.VCode, identificationVCode: identification.VCode);
                }
            }
        }

        [Route("GetIdentificationVerifyCodeRemainTime")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.VERIFY_CELLPHONE))]
        public IHttpActionResult GetIdentificationExpireTime(string id)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            try
            {
                object data = null;
                if (!string.IsNullOrEmpty(id))
                {
                    string decodedID = "";
                    if (!id.Contains("=") && !id.Contains("+"))
                    {
                        decodedID = WebUtility.UrlDecode(id);
                    }
                    else
                    {
                        decodedID = id;
                    }
                    var identificationVCode = System.Convert.ToInt64(cryptography.Decrypt(decodedID, EncryptKeyEnum.Key1));
                    var identification = Engine.Instance.IdentificationBusiness.GetIdentification(identificationVCode);
                    int totalSec = (int)(identification.ExpirationDate > DateTime.Now ? Math.Round((identification.ExpirationDate - DateTime.Now).TotalSeconds) : 0);
                    data = (new { success = true, expire = totalSec });

                }
                else
                {
                    data = (new { success = false, expire = 0 });
                }
                response.Data = JsonConvert.SerializeObject(data);
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
        private void CheckAvailability(string nationalCode, string cellphone, int identificationVCode)
        {
            List<ReportSourceData> reportSources = null;
            var IndividualAvailablity = false;
            var username = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("ApiUsername");
            var password = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("ApiPassword");
            long userVCode = 0;
            var token = ApiCall.Instance.GetToken(username: username, password: password, request: Request, userVCode: ref userVCode);
            var reportExistenceJObject = ApiCall.Instance.GetIndividualReportExistence(token: token, nationalCode: nationalCode, cellphone: cellphone, request: Request, username: username, password: password);
            var reportExistenceResponseState = (int)reportExistenceJObject["State"];
            if (reportExistenceResponseState == 1)
            {
                reportSources = JsonConvert.DeserializeObject<List<ReportSourceData>>(reportExistenceJObject["Data"].ToString());
                foreach (var reportSource in reportSources)
                {
                    switch (reportSource.ReportSource)
                    {
                        case (int)ReportSourceEnum.ICS:
                            Engine.Instance.IdentificationBusiness.SetIdentificationAvailabilityCheck(identificationVCode: identificationVCode, availabilityCheck: IndividualAvailablity);
                            break;
                    }
                }
            }
            else
            {
                var ex = new Exception(message: "API https://api.icescoring.com/api/v1/customer/IndividualReportExistence Error:" + (String)reportExistenceJObject["Errors"]);
                General.LogError(error: ex, request: Request);
            }
        }
    }
}