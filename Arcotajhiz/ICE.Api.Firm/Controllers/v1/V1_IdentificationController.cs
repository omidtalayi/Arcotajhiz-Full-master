using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using ICE.Api.Common.Enums;
using ICE.Api.Common.Models;
using ICE.Api.Firm.Filters;
using ICE.Api.Firm.Models.Request.v1;
using ICE.Api.Firm.Models.Response;
using ICE.Api.Presentation.Models.Request.v1;
using ICE.Business;
using ICE.Business.Models;
using ICE.Entities.Enums;
using ICE.Presentation.Common;
using Newtonsoft.Json;
using ResponseStateEnum = ICE.Api.Common.Enums.ResponseStateEnum;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web.Script.Serialization;
using ICE.Entities.Modules;

namespace ICE.Api.Firm.Controllers.v1
{
    [RoutePrefix("api/v1/identification")]
    public class V1_IdentificationController : BaseApiPresentationController
    {
        [Route("IndividualReportExistence")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.INDIVIDUAL_REPORT_EXISTENCE))]
        public IHttpActionResult CheckIndividualReportExistence([FromBody] CheckIndividualReportExistenceRequest body)
        {
            HttpResponseMessage objResponse;
            var response = new ActionResponse<Models.Response.CheckIndividualReportExistenceResponse>();
            var request = new CheckIndividualReportExistenceRequest();
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
                    var ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.UserHostAddress != null)
                    {
                        ip = HttpContext.Current.Request.UserHostAddress;
                    }
                    var responseData = ICE.Business.Engine.Instance.ApiBusiness.CheckIndividualReportExistence(userVCode: CurrentUser.VCode, nationalCode: body.NationalCode, cellphone: body.Cellphone, reportSources: body.ReportSources, ip: ip, receiverCellphone: body.ReceiverCellphone);
                    response.Data = new Models.Response.CheckIndividualReportExistenceResponse
                    {
                        ReportIsGeneratedPast24Hours = responseData[0].ReportIsGeneratedPast24Hours
                    };
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
        [Route("AddIdentificationFromFirm")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult AddIdentificationFromFirm([FromBody] AddIdentificationFirmRequestBody body)
        {
            string token = CurrentUser.token;
            long VCode = 0;
            int VerificationCode = 0;
            bool sendVerificationLink = false;
            string LinkToken = null;
            IdentificationData identificationItem = new IdentificationData();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            string reportID = null;
            List<ReportSourceData> reportSources = null;
            var ReportIsGeneratedPast24Hours = false;
            DateTime identificationExpirationDate = DateTime.MinValue;
            UserData objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            var response = new ActionResponse<AddIdentificationFirmResponse>();
            var objResponse = new HttpResponseMessage();
            UserData user = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            //چک کردن غیر فعال بودن گزارش حقوقی
            string disableReportStr = Engine.Instance.ApplicationBusiness.GetSetting("DisableFirmSubmit");
            bool disableFirmReport = (disableReportStr == "1");
            if (!string.IsNullOrEmpty(body.CompanyNationalID) && disableFirmReport)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(("خطا سیستم دریافت گزراش حقوقی غیر فعال میباشد.").ToString());
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            //چک کردن پرداخت کاربران استفاده کننده از خدمات apiFirm
            if (!ApiUserPaymentCheck(user))
            {
                var errorEnumCode = ((int)ApiErrorCodeEnum.USER_IS_NOT_APPROVED).ToString();
                response.Errors.Add("خطا : اعتبار قرارداد شما به پایان رسیده است لطفا با شماره 021-88177300 تماس حاصل فرمایید.");
                response.Data = new AddIdentificationFirmResponse()
                {
                    ResponseMessage = "خطا : اعتبار قرارداد شما به پایان رسیده است لطفا با شماره 021-88177300 تماس حاصل فرمایید."
                };
                response.State = ResponseStateEnum.NOTAUTHORIZED;
                response.Errors.Add(errorEnumCode);
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            decimal remainCredit = 0;
            bool hasChequeReport = false;
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var apiErrorEnum = body.Validation();
                    if (apiErrorEnum != ApiErrorCodeEnum.NONE)
                    {
                        response.State = ResponseStateEnum.FAILED;
                        response.Errors.Add(((int)apiErrorEnum).ToString());
                        var responseData = new AddIdentificationFirmResponse
                        {
                            ResponseMessage = "لطفا ورودی های خود را چک کنیید و مجددا تلاش نمایید."
                        };
                        response.Data = responseData;
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else
                    {
                        var identificationLinkAndExpirationDate = Business.Engine.Instance.IdentificationBusiness.GetIdentificationLinkAndExpirationDateFirmPanel(cellphone: body.CellPhone, nationalCode: body.NationalCode, UserVCode: CurrentUser.VCode, CompanyNationalID: body.CompanyNationalID);
                        if (identificationLinkAndExpirationDate != null)
                            identificationExpirationDate = identificationLinkAndExpirationDate.ReportExpirationDate;
                        var identificationStateEnums = new List<IdentificationStateEnum>
                            {
                                IdentificationStateEnum.EXPIRED,
                                IdentificationStateEnum.SHAHKAR_REJECTED,
                                IdentificationStateEnum.CONFIRMATION_DENIED,
                                IdentificationStateEnum.REPORT_EXPIRED,
                                IdentificationStateEnum.CONFIRMATION_NOT_RESPONDED,
                                IdentificationStateEnum.NOT_PAID,
                                IdentificationStateEnum.CONFIRMATION_DENIED_INCORRECT_NATIONAL_CODE
                            };
                        if (identificationLinkAndExpirationDate != null && identificationExpirationDate > DateTime.Now &&
                            !identificationStateEnums.Contains(identificationLinkAndExpirationDate.IdentificationState))
                        {
                            var reportExistenceJObject = ApiCall.Instance.GetIndividualReportExistence(token: token, nationalCode: body.NationalCode, cellphone: body.CellPhone, request: Request);
                            int reportExistenceResponseState = Convert.ToInt32(reportExistenceJObject["State"]);
                            if (reportExistenceResponseState == 1)
                            {
                                reportSources = JsonConvert.DeserializeObject<List<ReportSourceData>>(reportExistenceJObject["Data"].ToString());
                                foreach (ReportSourceData reportSource in reportSources)
                                {
                                    switch (reportSource.ReportSource)
                                    {
                                        case (int)ReportSourceEnum.ICS:
                                            {
                                                bool IndividualAvailability = reportSource.HasReport;
                                                Engine.Instance.IdentificationBusiness.SetIdentificationAvailabilityCheck(identificationVCode: VCode, availabilityCheck: IndividualAvailability);
                                                ReportIsGeneratedPast24Hours = reportSource.ReportIsGeneratedPast24Hours;
                                                break;
                                            }
                                    }
                                }
                            }
                            if (!ReportIsGeneratedPast24Hours)
                            {
                                if (identificationLinkAndExpirationDate != null)
                                {
                                    identificationItem = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(identificationLinkAndExpirationDate.IdentificationVCode);
                                }
                                reportID = WebUtility.UrlEncode(cryptography.Encrypt(identificationItem.VCode.ToString(), EncryptKeyEnum.Key1));
                                response.State = ResponseStateEnum.FAILED;
                                var responseData = new AddIdentificationFirmResponse
                                {
                                    ExpireDate = identificationLinkAndExpirationDate.ReportExpirationDate,
                                    ReportUrl = null,
                                    ReportID = Regex.Replace(reportID, "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()),
                                    NationalCode = identificationItem.NationalCode,
                                    VerifyUrl = MainURL + "identification/verifyCode?id=" + reportID,
                                    ResponseMessage = "رکورد فرد مورد نظر قبلا ثبت شده است و منتظر تایید میباشد.",
                                    ResponseState = identificationLinkAndExpirationDate.IdentificationState
                                };
                                response.Data = responseData;
                                response.Errors.Add(((int)(ApiErrorCodeEnum.REPORT_REQUEST_ALREADY_SUBMITTED)).ToString());
                                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                            }
                            else
                            {
                                identificationItem = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(identificationLinkAndExpirationDate.IdentificationVCode);
                                if (identificationItem != null)
                                {
                                    reportID = WebUtility.UrlEncode(cryptography.Encrypt(identificationItem.VCode.ToString(), EncryptKeyEnum.Key1));
                                    response.State = ResponseStateEnum.FAILED;
                                    var responseData = new AddIdentificationFirmResponse
                                    {
                                        ExpireDate = identificationLinkAndExpirationDate.ReportExpirationDate,
                                        ReportUrl = MainURL + identificationItem.ReportLink,
                                        ReportID = Regex.Replace(reportID, "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()),
                                        NationalCode = identificationItem.NationalCode,
                                        ResponseMessage = "گزارش فرد مورد در ۲۴ ساعت گذشته گرفته شده و اعتبار دارد.",
                                        ResponseState = identificationLinkAndExpirationDate.IdentificationState
                                    };
                                    response.Data = responseData;
                                    response.Errors.Add(((int)(ApiErrorCodeEnum.REPORT_ALREADY_EXIST_IN_LAST_24HOURS)).ToString());
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                }
                                else
                                {
                                    response.State = ResponseStateEnum.FAILED;
                                    response.Errors.Add(((int)(ApiErrorCodeEnum.SERVER_ERROR)).ToString());
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                }

                            }
                        }
                        else
                        {
                            var finnotechChequeIsActiveStr = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("FinnotechChequeIsActive");
                            bool finnotechChequeIsActive = finnotechChequeIsActiveStr == "1";
                            if (finnotechChequeIsActive)
                            {
                                hasChequeReport = body.hasChequeReport;
                            }
                            int receiverCellphoneVerificationCode = 0;
                            //تستی داتین
                            var isFanUpAuthorize = false;
                            if (user.VCode == 2887)
                            {
                                if (
                                    body.NationalCode == "1290635821" ||
                                    body.NationalCode == "0082276196" ||
                                    body.NationalCode == "0839933800" ||
                                    body.NationalCode == "6249745564" ||
                                    body.NationalCode == "0080280242" ||
                                    body.NationalCode == "4879758000" ||
                                    body.NationalCode == "0068421257" ||
                                    body.NationalCode == "1950647791" ||
                                    body.NationalCode == "0017400570" ||
                                    body.NationalCode == "0440053331" ||
                                    body.NationalCode == "0083975071" ||
                                    body.NationalCode == "0015313751" ||
                                    body.NationalCode == "0076800644" ||
                                    body.NationalCode == "4073489194" ||
                                    body.NationalCode == "0453733931" ||
                                    body.NationalCode == "0016575709" ||
                                    body.NationalCode == "0061753361" ||
                                    body.NationalCode == "0083613900" ||
                                    body.NationalCode == "0078908906" ||
                                    body.NationalCode == "0453482791" ||
                                    body.NationalCode == "0020947186" ||
                                    body.NationalCode == "0492932085" ||
                                    body.NationalCode == "0491979665" ||
                                    body.NationalCode == "0023367083" ||
                                    body.NationalCode == "6649861413" ||
                                    body.NationalCode == "0052492524" ||
                                    body.NationalCode == "2121780904" ||
                                    body.NationalCode == "0532997662" ||
                                    body.NationalCode == "6249716033" ||
                                    body.NationalCode == "0082190674" ||
                                    body.NationalCode == "0014480001" ||
                                    body.NationalCode == "0012734144" ||
                                    body.NationalCode == "3255931193"
                                    )
                                {
                                    isFanUpAuthorize = true;
                                }
                            }
                            if (user.VCode == 2887 && !isFanUpAuthorize)
                            {
                                response.State = ResponseStateEnum.NOTAUTHORIZED;
                                response.Errors.Add(((int)(ApiErrorCodeEnum.USER_IS_NOT_APPROVED)).ToString());
                                response.Data = new AddIdentificationFirmResponse()
                                {
                                    ResponseMessage = "خطا : درسترسی شما برای این درخواست محدود است."
                                };
                                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                            }
                            else
                            {
                                //چک کردن دسترسی نوع پرداخت گزارش
                                if (user.AvailableUserPaymentTypeVCode == UserPaymentTypeEnum.NONE || user.AvailableUserPaymentTypeVCode == body.UserPaymentTypeEnum)
                                {
                                    IdentificationInsStateEnum identificationInsState = Engine.Instance.IdentificationBusiness.AddIdentification(userVCode: CurrentUser.VCode, vcode: ref VCode, cellphone: body.CellPhone, nationalCode: body.NationalCode, companyNationalID: body.CompanyNationalID, verificationCode: ref VerificationCode, LinkToken: ref LinkToken, userPaymentTypeVCode: body.UserPaymentTypeEnum, fromFirmPanel: true, receiverCellphoneVerificationCode: ref receiverCellphoneVerificationCode, hasChequeReport: hasChequeReport, identificationTypeVCode: (string.IsNullOrEmpty(body.CompanyNationalID) ? IdentificationTypeEnum.INDIVIDUAL : IdentificationTypeEnum.FIRM));
                                    switch (identificationInsState)
                                    {
                                        case IdentificationInsStateEnum.SUCCESSFUL:
                                            identificationItem = Engine.Instance.IdentificationBusiness.GetIdentification(VCode);
                                            if (identificationItem.UserPaymentTypeVCode == UserPaymentTypeEnum.PAY_FROM_CREDIT)
                                            {
                                                var PayIdentificationByCreditState = ICE.Business.Engine.Instance.PaymentBusiness.PayIdentificationByCredit(user.VCode, identificationItem.VCode);
                                                switch (PayIdentificationByCreditState)
                                                {
                                                    case PayIdentificationByCreditStateEnum.SUCCESSFUL:
                                                        response.State = ResponseStateEnum.SUCCESS;
                                                        reportID = WebUtility.UrlEncode(cryptography.Encrypt(identificationItem.VCode.ToString(), EncryptKeyEnum.Key1));
                                                        var responseResult = ApiCall.Instance.AppIcs24Request(identificationItem.Cellphone, identificationItem.NationalCode, companyNationalID: identificationItem.CompanyNationalID);
                                                        if (!(bool)responseResult["hasError"])
                                                        {
                                                            var appIcs24HashCode = responseResult["data"] != null ? responseResult["data"].ToString() : "";
                                                            if (!string.IsNullOrEmpty(appIcs24HashCode))
                                                            {
                                                                Engine.Instance.IdentificationBusiness.SetIdentificationAppICS24HashCode(identificationVCode: identificationItem.VCode, appIcs24HashCode: appIcs24HashCode);
                                                                Engine.Instance.IdentificationBusiness.UpdateExpirationDate(identificationItem.VCode, DateTime.Now.AddSeconds(240));
                                                            }
                                                        }
                                                        remainCredit = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode).Credit;
                                                        var responseData3 = new AddIdentificationFirmResponse
                                                        {
                                                            ExpireDate = identificationItem.ReportExpirationDate,
                                                            ReportUrl = "",
                                                            VerifyUrl = MainURL + "identification/verifyCode?id=" + reportID,
                                                            RemainCredit = remainCredit,
                                                            ReportID = Regex.Replace(reportID, "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()),
                                                            NationalCode = identificationItem.NationalCode,
                                                            ResponseMessage = "درخواست شما با موفقیت ثبت شد.",
                                                            ResponseState = (IdentificationStateEnum)identificationItem.IdentificationState.VCode
                                                        };
                                                        response.Data = responseData3;
                                                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                                        if (!user.SelfOtp)
                                                        {
                                                            sendVerificationLink = true;
                                                        }
                                                        break;
                                                    case PayIdentificationByCreditStateEnum.CREDIT_IS_NOT_ENOUGH:
                                                        response.Errors.Add(((int)(ApiErrorCodeEnum.CREDIT_IS_NOT_ENOUGH)).ToString());
                                                        response.Data = new AddIdentificationFirmResponse()
                                                        {
                                                            ResponseMessage = "خطا : اعتبار شما برای انجام این درخواست کافی نیست. ابتدا حساب کاربری خود را از طریق پنل حقوقی firmpanel.icescoring.com شارژ نمایید و مجددا اقدام به دریافت گزارش نمایید."
                                                        };
                                                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                                        break;
                                                }
                                            }
                                            else if (identificationItem.UserPaymentTypeVCode == UserPaymentTypeEnum.PAY_BY_CUSTOMER)
                                            {
                                                remainCredit = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode).Credit;
                                                response.State = ResponseStateEnum.SUCCESS;
                                                reportID = WebUtility.UrlEncode(cryptography.Encrypt(identificationItem.VCode.ToString(), EncryptKeyEnum.Key1));
                                                var responseData2 = new AddIdentificationFirmResponse
                                                {
                                                    ExpireDate = identificationItem.ReportExpirationDate,
                                                    ReportUrl = "",
                                                    VerifyUrl = MainURL + "Identification/PayIndex?id=" + reportID,
                                                    RemainCredit = remainCredit,
                                                    ReportID = Regex.Replace(reportID, "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()),
                                                    NationalCode = identificationItem.NationalCode,
                                                    ResponseMessage = "درخواست شما با موفقیت ثبت شد.",
                                                    ResponseState = (IdentificationStateEnum)identificationItem.IdentificationState.VCode
                                                };
                                                response.Data = responseData2;
                                                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                                if (!user.SelfOtp)
                                                {
                                                    sendVerificationLink = true;
                                                }
                                            }
                                            break;
                                        case IdentificationInsStateEnum.CREDIT_IS_NOT_ENOUGH:
                                            remainCredit = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode).Credit;
                                            var responseData = new AddIdentificationFirmResponse
                                            {
                                                ExpireDate = DateTime.Now,
                                                ReportUrl = "",
                                                ReportID = "",
                                                RemainCredit = remainCredit,
                                                NationalCode = body.NationalCode,
                                                ResponseMessage = "اعتبار شما برای دریافت این گذارش کافی نیست به منو شارژ اعتبار مراجعه نمایید.",
                                                ResponseState = (IdentificationStateEnum)IdentificationStateEnum.NONE
                                            };
                                            response.Data = responseData;
                                            response.State = ResponseStateEnum.FAILED;
                                            response.Errors.Add(((int)(ApiErrorCodeEnum.CREDIT_IS_NOT_ENOUGH)).ToString());
                                            break;
                                        case IdentificationInsStateEnum.NONE:
                                            response.State = ResponseStateEnum.FAILED;
                                            response.Errors.Add(((int)(ApiErrorCodeEnum.SERVER_ERROR)).ToString());
                                            break;
                                    }
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                }
                                else
                                {
                                    response.State = ResponseStateEnum.NOTAUTHORIZED;
                                    response.Errors.Add(((int)(ApiErrorCodeEnum.USER_IS_NOT_APPROVED)).ToString());
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                }
                            }


                        }
                    }
                }
                else
                {
                    response.State = ResponseStateEnum.NOTAUTHORIZED;
                    response.Errors.Add(((int)(ApiErrorCodeEnum.USER_IS_NOT_APPROVED)).ToString());
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(((int)(ApiErrorCodeEnum.SERVER_ERROR)).ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            if (sendVerificationLink && identificationItem != null)
            {
                var objUser = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                string msg = "آیس:" + "\n" + (objUser.Name != null ? objUser.Name : "") + " درخواست مشاهده رتبه اعتباری شما را دارد." + "\n" + " لینک تایید:" + "\n";
                msg += MainURL + identificationItem.VerificationLink;
                Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: (!string.IsNullOrEmpty(identificationItem.SecondCellphone) ? identificationItem.SecondCellphone : identificationItem.Cellphone), message: msg, ip: "", identificationVCode: identificationItem.VCode, typeVCode: SMSTypeEnum.CONFIRMATION, operatorType: OperatorTypeEnum.ICS_IRANSMS);
            }
            return ResponseMessage(objResponse);
            //}

        }
        public static string UpperCaseUrlEncode(string s)
        {
            char[] temp = HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp);
        }

        [Route("GetReportData")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GET_REPORT_DATA))]
        public IHttpActionResult GetReportData(string reportID)
        {
            string token = CurrentUser.token;
            string LinkToken = null;
            IdentificationData identificationItem = new IdentificationData();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            UserData objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: 4);
            var response = new ActionResponse<GetReportDataResponse>();
            HttpResponseMessage objResponse = null;
            UserData user = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: 4);
            string responseData = null;
            CreditRiskReportEnum reportState = CreditRiskReportEnum.NONE;
            try
            {
                //if (testEnabled)
                //{

                //}
                if (!reportID.Contains("=") && !reportID.Contains("+"))
                {
                    reportID = WebUtility.UrlDecode(reportID);
                }
                var identificationVCode = long.Parse(cryptography.Decrypt(reportID, EncryptKeyEnum.Key1));

                identificationItem = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(identificationVCode);
                if (identificationItem != null)
                {
                    if (CurrentUser != null && identificationItem.UserVCode == CurrentUser.VCode)
                    {
                        if (identificationItem.ReportExpirationDate >= DateTime.Now)
                        {
                            if (!string.IsNullOrEmpty(identificationItem.ReportLink))
                            {
                                string IndividualReport = "";
                                string EmptyReport = "";
                                if (identificationItem.HasShahkarIdentified)
                                {

                                    List<PersonageData> personageInfo = null;
                                    List<CompanyData> companyInfo = null;
                                    EmptyReportData emptyInfo = null;

                                    var reportTypes = Engine.Instance.IdentificationBusiness.GetIdentificationReportTypes(identificationItem.TrackingCode);
                                    if (identificationItem.IdentificationType.VCode == (int)IdentificationTypeEnum.INDIVIDUAL)
                                    {
                                        personageInfo = Engine.Instance.ICSBusiness.GetAdvancedIndividualReport(nationalCode: identificationItem.NationalCode, identificationVCode: identificationItem.VCode, IsNewCoreConnection: true);

                                        if (personageInfo != null && personageInfo.Any())
                                        {
                                            string jobj = JsonConvert.SerializeObject(personageInfo);
                                            IndividualReport = jobj.ToString().Replace("\"", "'");
                                        }
                                        else
                                        {
                                            emptyInfo = Engine.Instance.ICSBusiness.GetEmptyIndividualReport(nationalCode: identificationItem.NationalCode, identificationVCode: identificationVCode, IsNewCoreConnection: true);
                                            string jobj = JsonConvert.SerializeObject(emptyInfo);
                                            EmptyReport = jobj.ToString().Replace("\"", "'");
                                        }

                                        responseData = IndividualReport;
                                        reportState = CreditRiskReportEnum.INDIVIDUAL_REPORT;
                                    }
                                    else
                                    {
                                        if (identificationItem.IsLegalPerson)
                                        {
                                            companyInfo = Engine.Instance.ICSBusiness.GetAdvancedCompanyReport(companyNationalID: identificationItem.CompanyNationalID, identificationVCode: identificationItem.VCode, IsNewCoreConnection: true);

                                            if (companyInfo != null && companyInfo.Any())
                                            {
                                                string jobj = JsonConvert.SerializeObject(companyInfo);
                                                IndividualReport = jobj.ToString().Replace("\"", "'");
                                            }
                                            else
                                            {
                                                emptyInfo = Engine.Instance.ICSBusiness.GetEmptyCompanyReport(companyNationalID: identificationItem.CompanyNationalID, identificationVCode: identificationVCode, IsNewCoreConnection: true);
                                                string jobj = JsonConvert.SerializeObject(emptyInfo);
                                                EmptyReport = jobj.ToString().Replace("\"", "'");
                                            }
                                            responseData = IndividualReport;
                                            reportState = CreditRiskReportEnum.INDIVIDUAL_REPORT;
                                        }
                                        else
                                        {
                                            reportState = CreditRiskReportEnum.IS_LEGAL_PERSON_FAILD;
                                        }
                                    }
                                }
                                else
                                {
                                    reportState = CreditRiskReportEnum.KYC_FAILD;
                                }
                                string encryptedTrackingCode = null;
                                if (identificationItem.TrackingCode != 0)
                                {
                                    encryptedTrackingCode = StringToURLCustom(cryptography.Encrypt(identificationItem.TrackingCode.ToString(), EncryptKeyEnum.Key1));
                                }
                                GetReportDataResponse data = new GetReportDataResponse()
                                {
                                    EmptyReportData = EmptyReport,
                                    ReportData = responseData,
                                    ReportState = reportState,
                                    ReportLink = MainURL + identificationItem.ReportLink,
                                    PDFLink = string.IsNullOrEmpty(encryptedTrackingCode) ? "" : MainURL + "Report/GetPdf?id=" + encryptedTrackingCode + "&printPage=false"
                                };
                                response.State = ResponseStateEnum.SUCCESS;
                                response.Data = data;
                                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                            }
                            else
                            {
                                GetReportDataResponse data = new GetReportDataResponse()
                                {
                                    ReportData = responseData,
                                    ReportState = CreditRiskReportEnum.EMPTY_REPORT

                                };
                                response.State = ResponseStateEnum.FAILED;
                                response.Data = data;
                                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                            }
                        }
                        else
                        {
                            GetReportDataResponse data = new GetReportDataResponse()
                            {
                                ReportData = responseData,
                                ReportState = CreditRiskReportEnum.IS_EXPIRED

                            };
                            response.State = ResponseStateEnum.FAILED;
                            response.Data = data;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                    }
                    else
                    {
                        GetReportDataResponse data = new GetReportDataResponse()
                        {
                            ReportData = null,
                            ReportState = CreditRiskReportEnum.INVALID_USER

                        };
                        response.State = ResponseStateEnum.FAILED;
                        response.Data = data;
                        objResponse = Request.CreateResponse(HttpStatusCode.Unauthorized, response);
                    }
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(((int)(ApiErrorCodeEnum.REPORT_ID_IS_INVALID)).ToString());
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }

            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(((int)(ApiErrorCodeEnum.SERVER_ERROR)).ToString());
                response.Data = null;
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("GetReportStatus")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GET_REPORT_STATUS))]
        public IHttpActionResult GetReportStatus(string reportID)
        {
            string token = CurrentUser.token;
            string LinkToken = null;
            IdentificationData identificationItem = new IdentificationData();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            UserData objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            var response = new ActionResponse<GetReportDataResponse>();
            HttpResponseMessage objResponse = null;
            UserData user = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            BatchResultXmlData batchResultXml = null;
            EmptyReportData emptyInfo = null;
            CreditRiskReportEnum reportState = CreditRiskReportEnum.NONE;
            try
            {
                //if (!testEnabled)
                //{
                if (!reportID.Contains("=") && !reportID.Contains("+"))
                {
                    reportID = WebUtility.UrlDecode(reportID);
                }
                var identificationVCode = long.Parse(cryptography.Decrypt(reportID, EncryptKeyEnum.Key1));
                identificationItem = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(identificationVCode);
                if (identificationItem != null)
                {
                    if (identificationItem.ReportExpirationDate >= DateTime.Now)
                    {
                        if (!string.IsNullOrEmpty(identificationItem.ReportLink))
                        {
                            if (identificationItem.HasShahkarIdentified)
                            {
                                var reportTypes = Engine.Instance.IdentificationBusiness.GetIdentificationReportTypes(identificationItem.TrackingCode);
                                if (identificationItem.IdentificationType.VCode == (int)IdentificationTypeEnum.INDIVIDUAL)
                                {
                                    reportState = CreditRiskReportEnum.INDIVIDUAL_REPORT;
                                }
                                else
                                {
                                    if (identificationItem.IsLegalPerson)
                                    {
                                        reportState = CreditRiskReportEnum.INDIVIDUAL_REPORT;
                                    }
                                    else
                                    {
                                        reportState = CreditRiskReportEnum.IS_LEGAL_PERSON_FAILD;
                                    }
                                }
                            }
                            else
                            {
                                reportState = CreditRiskReportEnum.KYC_FAILD;
                            }
                            string encryptedTrackingCode = null;
                            if (identificationItem.TrackingCode != 0)
                            {
                                encryptedTrackingCode = StringToURLCustom(cryptography.Encrypt(identificationItem.TrackingCode.ToString(), EncryptKeyEnum.Key1));
                            }
                            GetReportDataResponse data = new GetReportDataResponse()
                            {
                                ReportState = reportState,
                                ReportLink = MainURL + identificationItem.ReportLink,
                                PDFLink = string.IsNullOrEmpty(encryptedTrackingCode) ? "" : MainURL + "Report/GetPdf?id=" + encryptedTrackingCode + "&printPage=false"
                            };
                            response.State = ResponseStateEnum.SUCCESS;
                            response.Data = data;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                        else
                        {
                            GetReportDataResponse data = new GetReportDataResponse()
                            {
                                ReportState = CreditRiskReportEnum.EMPTY_REPORT

                            };
                            response.State = ResponseStateEnum.FAILED;
                            response.Data = data;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                    }
                    else
                    {
                        GetReportDataResponse data = new GetReportDataResponse()
                        {
                            ReportState = CreditRiskReportEnum.IS_EXPIRED

                        };
                        response.State = ResponseStateEnum.FAILED;
                        response.Data = data;
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(((int)ApiErrorCodeEnum.REPORT_ID_IS_INVALID).ToString());
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                //}
                //else
                //{
                //    GetReportDataResponse data = new GetReportDataResponse()
                //    {
                //        ReportState = CreditRiskReportEnum.INDIVIDUAL_REPORT,
                //        ReportLink = MainURL + "Report/TestReport.html",
                //        PDFLink = MainURL + "Report/pdf_sample.pdf"
                //    };
                //    response.State = ResponseStateEnum.SUCCESS;
                //    response.Data = data;
                //    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                //}

            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(((int)ApiErrorCodeEnum.SERVER_ERROR).ToString());
                response.Data = null;
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("CancelIdentification")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult CancelIdentification([FromBody] CancelIdentificationFirmRequest data)
        {
            UserData objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            IdentificationData identificationItem = new IdentificationData();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            var response = new ActionResponse<CancelIdentificationResponse>();
            HttpResponseMessage objResponse = null;
            bool success = false;
            string reportIDObj = null;
            try
            {
                long identificationVCode = 0;
                if (data.vcode != 0)
                {
                    identificationVCode = data.vcode;
                }
                else
                {
                    reportIDObj = data.reportID;
                    if (!data.reportID.Contains("=") && !data.reportID.Contains("+"))
                    {
                        reportIDObj = WebUtility.UrlDecode(data.reportID);
                    }
                    identificationVCode = long.Parse(cryptography.Decrypt(reportIDObj, EncryptKeyEnum.Key1));
                }
                identificationItem = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(identificationVCode);
                if (identificationItem != null)
                {
                    if (identificationItem.ReportExpirationDate >= DateTime.Now)
                    {
                        if (identificationItem.IdentificationState.VCode == (int)IdentificationStateEnum.WAITING_INDIVIDUAL_CONFIRMATION ||
                            identificationItem.IdentificationState.VCode == (int)IdentificationStateEnum.CONFIRMED ||
                            identificationItem.IdentificationState.VCode == (int)IdentificationStateEnum.ICS24_OTP_LINK_RECEIVED ||
                            (identificationItem.IdentificationState.VCode == (int)IdentificationStateEnum.PAID && identificationItem.UserPaymentTypeVCode == UserPaymentTypeEnum.PAY_FROM_CREDIT))
                        {
                            if (identificationItem.UserPaymentTypeVCode == UserPaymentTypeEnum.PAY_FROM_CREDIT)
                            {
                                success = ICE.Business.Engine.Instance.PaymentBusiness.ReturnIdentificationCredit(identificationVCode: identificationItem.VCode, userVCode: objCurrentUser.VCode);
                            }
                            else
                            {
                                success = ICE.Business.Engine.Instance.IdentificationBusiness.CancelIdentifiction(identificationVCode: identificationItem.VCode, userVCode: objCurrentUser.VCode);
                            }

                            response.State = success ? ResponseStateEnum.SUCCESS : ResponseStateEnum.FAILED;
                        }
                        else
                        {
                            response.State = ResponseStateEnum.FAILED;
                            response.Errors.Add(((int)ApiErrorCodeEnum.REPORT_IS_NOT_IN_RIGHT_STATE).ToString());
                        }
                    }
                    else
                    {
                        response.State = ResponseStateEnum.FAILED;
                        response.Errors.Add(((int)ApiErrorCodeEnum.REPORT_IS_EXPIRED).ToString());
                    }
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(((int)ApiErrorCodeEnum.REPORT_ID_IS_INVALID).ToString());

                }
                decimal remainCredit = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode).Credit;
                var responseData = new CancelIdentificationResponse
                {
                    remainCredit = remainCredit
                };
                response.Data = responseData;
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(((int)ApiErrorCodeEnum.SERVER_ERROR).ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("ResendIdentificationCode")]
        [Route("ResendIdentificationVerifyCode")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult ResendIdentificationCode([FromBody] ResendVerifyCodeData data)
        {
            UserData objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);

            IdentificationData identificationItem = new IdentificationData();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            var response = new ActionResponse<ResendIdentificationCodeResponse>();
            HttpResponseMessage objResponse = null;
            long identificationVCode = 0;
            string reportID = "";

            try
            {
                if (string.IsNullOrEmpty(data.reportId))
                {
                    reportID = data.firmCode;
                    if (!data.firmCode.Contains("=") && !data.firmCode.Contains("+"))
                    {
                        reportID = WebUtility.UrlDecode(data.firmCode);
                    }
                }
                else
                {
                    reportID = data.reportId;
                    if (!data.reportId.Contains("=") && !data.reportId.Contains("+"))
                    {
                        reportID = WebUtility.UrlDecode(data.reportId);
                    }
                }
                identificationVCode = System.Convert.ToInt64(cryptography.Decrypt(reportID, EncryptKeyEnum.Key1));
                var identification = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(vcode: identificationVCode);
                var result = ApiCall.Instance.AppIcs24RenewToken(hashcode: identification.appIcs24HashCode, request: Request);
                response.State = ResponseStateEnum.SUCCESS;
                if (System.Convert.ToBoolean(result["hasError"]))
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Data = new ResendIdentificationCodeResponse { message = (result["messages"] == null ? null : result["messages"][0]["message"].ToString()), status = (result["messages"] == null ? null : result["messages"][0]["reason"].ToString()), hasError = (bool)result["hasError"], resendWait = 0 };
                }
                else
                {
                    int expire = System.Convert.ToInt32(result["data"]["resendWait"]);
                    Engine.Instance.IdentificationBusiness.UpdateExpirationDate(identification.VCode, DateTime.Now.AddSeconds(expire));
                    bool resendLocked = false;
                    if (result["messages"] != null)
                    {
                        var res = result["messages"].ToString();
                        if (!string.IsNullOrEmpty(res))
                        {
                            if (result["messages"][0] != null)
                            {
                                if (result["messages"][0]["reason"] != null)
                                {
                                    resendLocked = result["messages"][0]["reason"].ToString() == "ResendLocked";
                                }
                            }
                        }
                    }
                    response.Data = new ResendIdentificationCodeResponse { resendWait = (int)result["data"]["resendWait"], resendLocked = resendLocked, hasError = (bool)result["hasError"], message = (result["messages"] == null ? null : result["messages"].ToString()) };
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(((int)ApiErrorCodeEnum.SERVER_ERROR).ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("PanelCodeConfirmation")]
        [Route("IdentificationVerify")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult PanelCodeConfirmation([FromBody] PanelCodeConfirmationRequest data)
        {
            CryptographyBusiness cryptography = new CryptographyBusiness();
            var response = new ActionResponse<IdentificationVerifyResponse>();
            HttpResponseMessage objResponse = null;
            long identificationVCode;
            string message = "";
            bool success = false;
            string status = "";
            int expire = 90;
            string reportID = data.reportID;
            try
            {
                //if (testEnabled)
                //{
                //    response.State = ResponseStateEnum.SUCCESS;
                //    response.Data = new IdentificationVerifyResponse { success = true, message = "", status = "InProcessing", expire = 120 };
                //    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                //}
                //else
                //{
                if (!data.reportID.Contains("=") && !data.reportID.Contains("+"))
                {
                    reportID = WebUtility.UrlDecode(data.reportID);
                }
                identificationVCode = System.Convert.ToInt64(cryptography.Decrypt(reportID, EncryptKeyEnum.Key1));
                var identification = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(vcode: identificationVCode);
                if (identification.IdentificationState.VCode == (int)IdentificationStateEnum.ICS24_OTP_LINK_RECEIVED && !string.IsNullOrEmpty(identification.appIcs24HashCode))
                {
                    if (data.code > 0)
                    {
                        var responseResult = ApiCall.Instance.AppIcs24Validate(hashcode: identification.appIcs24HashCode, code: data.code.ToString(), request: Request);
                        if (System.Convert.ToBoolean(responseResult["hasError"]))
                        {
                            status = "HasError";
                            if (responseResult["messages"] != null)
                            {
                                switch (responseResult["messages"][0]["reason"].ToString())
                                {
                                    case "ShahkarFailed":
                                        {
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationICS24Confirmed(identification.VCode);
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationHasShahkarIdentified(identificationVCode: identification.VCode, hasShahkarIdentified: false);
                                            message = "شماره همراه وارد شده با کدملی همخوانی ندارد.";
                                            break;
                                        }

                                    case "SherkathaFailed":
                                        {
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationICS24Confirmed(identification.VCode);
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationIsLegalPerson(identificationVCode: identification.VCode, isLegalPerson: false);
                                            message = " کدملی وارد شده متعلق به مدیر شرکت نمیباشد.";
                                            break;
                                        }

                                    case "IncorrectToken":
                                        {
                                            message = responseResult["messages"][0]["message"].ToString();
                                            break;
                                        }

                                    case "TokenExpired":
                                        {
                                            message = "اعتبار رمز به پایان رسیده رمز جدید به همراه مورد نظر ارسال گردید.";
                                            var result = ApiCall.Instance.AppIcs24RenewToken(hashcode: identification.appIcs24HashCode, request: Request);
                                            if (System.Convert.ToBoolean(result["hasError"]))
                                            {
                                                success = false;
                                                message = result["messages"][0]["message"].ToString();
                                            }
                                            else
                                            {
                                                expire = System.Convert.ToInt32(result["data"]["resendWait"]);
                                                Engine.Instance.IdentificationBusiness.UpdateExpirationDate(identification.VCode, DateTime.Now.AddSeconds(expire));
                                            }
                                            break;
                                        }

                                    case "OtpLocked":
                                        {
                                            message = "درخواست مورد نظر به دلیل بیش از حد وارد کردن کد اشتباه قفل شده است لطفا با پشتیبانی تماس بگیرید.";
                                            break;
                                        }

                                    default:
                                        {
                                            {
                                                message = responseResult["messages"][0]["message"].ToString();
                                                break;
                                            }

                                        }
                                }
                                status = responseResult["messages"][0]["reason"].ToString();
                            }
                            else
                            {
                                message = "خطایی پیش آمده لطفا با پشتیبانی تماس بگیرید.";
                            }

                        }
                        else
                        {
                            status = responseResult["data"].ToString();
                            switch (responseResult["data"].ToString())
                            {
                                case "ReportGenerated":
                                case "InProcessing":
                                    {
                                        message = "گزارش شما تایید شد و در حال پردازش است و بعد از پردازش میتوانید آن را در تب قابل مشاهده مشاهده نمایید.";
                                        break;
                                    }

                                default:
                                    {
                                        message = "لطفا مجددا تلاش نمایید و یا با پشتیبانی تماس بگیرید.";
                                        break;
                                    }
                            }
                        }
                    }
                    else
                    {
                        message = "رمز جدید به همراه مورد نظر ارسال گردید.";
                        var result = ApiCall.Instance.AppIcs24RenewToken(hashcode: identification.appIcs24HashCode, request: Request);
                        if (System.Convert.ToBoolean(result["hasError"]))
                        {
                            success = false;
                            message = result["messages"][0]["message"].ToString();
                        }
                        else
                        {
                            expire = System.Convert.ToInt32(result["data"]["resendWait"]);
                            Engine.Instance.IdentificationBusiness.UpdateExpirationDate(identification.VCode, DateTime.Now.AddSeconds(expire));
                        }
                        status = "TokenExpired";
                    }
                    response.Data = new IdentificationVerifyResponse { success = success, message = message, status = status, expire = expire };
                    response.State = ResponseStateEnum.SUCCESS;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    status = "RequestProccessedBefore";
                    response.Data = new IdentificationVerifyResponse { success = success, message = "این درخواست قبلا پردازش شده است.", status = status, expire = 0 };
                    response.State = ResponseStateEnum.SUCCESS;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(((int)ApiErrorCodeEnum.SERVER_ERROR).ToString());
                General.LogError(error: ex, request: HttpContext.Current.Request);
                General.LogError(error: new Exception("خطا تایید : reportID=" + reportID), request: HttpContext.Current.Request);
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("AddIdentificationFromFirmBatch")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult AddIdentificationFromFirmBatch([FromBody] List<AddIdentificationFirmRequestBody> bodyList)
        {
            string token = CurrentUser.token;
            long VCode = 0;
            int VerificationCode = 0;
            string LinkToken = null;
            IdentificationData identificationItem = new IdentificationData();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            string reportID = null;
            List<ReportSourceData> reportSources = null;
            var ReportIsGeneratedPast24Hours = false;
            DateTime identificationExpirationDate = DateTime.MinValue;
            UserData objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            var response = new ActionResponse<List<AddIdentificationFirmResponse>>();
            response.Data = new List<AddIdentificationFirmResponse>();
            HttpResponseMessage objResponse = null;
            UserData user = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            try
            {
                //if (!string.IsNullOrEmpty(token))
                //{
                //    bool sendVerificationLink = false;
                //    foreach (var body in bodyList)
                //    {
                //        var identificationLinkAndExpirationDate = Business.Engine.Instance.IdentificationBusiness.GetIdentificationLinkAndExpirationDateFirmPanel(cellphone: body.CellPhone, nationalCode: body.NationalCode, UserVCode: CurrentUser.VCode, CompanyNationalID: body.CompanyNationalID);
                //        if (identificationLinkAndExpirationDate != null)
                //            identificationExpirationDate = identificationLinkAndExpirationDate.ReportExpirationDate;
                //        var identificationStateEnums = new List<IdentificationStateEnum>
                //    {
                //         IdentificationStateEnum.EXPIRED,
                //        IdentificationStateEnum.SHAHKAR_REJECTED,
                //        IdentificationStateEnum.CONFIRMATION_DENIED,
                //        IdentificationStateEnum.REPORT_EXPIRED,
                //        IdentificationStateEnum.CONFIRMATION_NOT_RESPONDED,
                //        IdentificationStateEnum.NOT_PAID,
                //        IdentificationStateEnum.CONFIRMATION_DENIED_INCORRECT_NATIONAL_CODE
                //    };
                //        if (identificationLinkAndExpirationDate != null && identificationExpirationDate > DateTime.Now &&
                //            !identificationStateEnums.Contains(identificationLinkAndExpirationDate.IdentificationState))
                //        {
                //            var reportExistenceJObject = ApiCall.Instance.GetIndividualReportExistence(token: token, nationalCode: body.NationalCode, cellphone: body.CellPhone, request: Request);
                //            int reportExistenceResponseState = Convert.ToInt32(reportExistenceJObject["State"]);
                //            if (reportExistenceResponseState == 1)
                //            {
                //                reportSources = JsonConvert.DeserializeObject<List<ReportSourceData>>(reportExistenceJObject["Data"].ToString());
                //                foreach (ReportSourceData reportSource in reportSources)
                //                {
                //                    switch (reportSource.ReportSource)
                //                    {
                //                        case (int)ReportSourceEnum.ICS:
                //                            {
                //                                bool IndividualAvailability = reportSource.HasReport;
                //                                Engine.Instance.IdentificationBusiness.SetIdentificationAvailabilityCheck(identificationVCode: VCode, availabilityCheck: IndividualAvailability);
                //                                ReportIsGeneratedPast24Hours = reportSource.ReportIsGeneratedPast24Hours;
                //                                break;
                //                            }
                //                    }
                //                }
                //            }
                //            if (!ReportIsGeneratedPast24Hours)
                //            {
                //                reportID = WebUtility.UrlEncode(cryptography.Encrypt(identificationLinkAndExpirationDate.IdentificationVCode.ToString(), EncryptKeyEnum.Key1));
                //                response.State = ResponseStateEnum.SUCCESS;
                //                response.Data.Add(new AddIdentificationFirmResponse()
                //                {
                //                    ExpireDate = identificationLinkAndExpirationDate.ReportExpirationDate,
                //                    ReportUrl = null,
                //                    ReportID = Regex.Replace(reportID, "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()),
                //                    NationalCode = body.NationalCode,
                //                    ResponseMessage = "رکورد فرد مورد نظر قبلا ثبت شده است و منتظر تایید میباشد.",
                //                    ResponseState = identificationLinkAndExpirationDate.IdentificationState
                //                });
                //            }
                //            else
                //            {
                //                identificationItem = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentification(identificationLinkAndExpirationDate.IdentificationVCode);
                //                reportID = WebUtility.UrlEncode(cryptography.Encrypt(identificationItem.VCode.ToString(), EncryptKeyEnum.Key1));
                //                response.State = ResponseStateEnum.SUCCESS;
                //                response.Data.Add(new AddIdentificationFirmResponse
                //                {
                //                    ExpireDate = identificationLinkAndExpirationDate.ReportExpirationDate,
                //                    ReportUrl = "https://icescoring.com" + identificationItem.ReportLink,
                //                    ReportID = Regex.Replace(reportID, "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()),
                //                    NationalCode = body.NationalCode,
                //                    ResponseMessage = "رکورد فرد مورد نظر قبلا ثبت شده است.",
                //                    ResponseState = identificationLinkAndExpirationDate.IdentificationState
                //                });
                //            }
                //        }
                //        else
                //        {
                //            int receiverCellphoneVerificationCode = 0;
                //            IdentificationInsStateEnum identificationInsState = Engine.Instance.IdentificationBusiness.AddIdentification(userVCode: CurrentUser.VCode, vcode: ref VCode, cellphone: body.CellPhone, nationalCode: body.NationalCode, verificationCode: ref VerificationCode, LinkToken: ref LinkToken, userPaymentTypeVCode: body.UserPaymentTypeEnum, fromFirmPanel: true, receiverCellphoneVerificationCode: ref receiverCellphoneVerificationCode);
                //            switch (identificationInsState)
                //            {
                //                case IdentificationInsStateEnum.SUCCESSFUL:
                //                    identificationItem = Engine.Instance.IdentificationBusiness.GetIdentification(VCode);
                //                    if (identificationItem.UserPaymentTypeVCode == UserPaymentTypeEnum.PAY_FROM_CREDIT)
                //                    {
                //                        var PayIdentificationByCreditState = ICE.Business.Engine.Instance.PaymentBusiness.PayIdentificationByCredit(user.VCode, identificationItem.VCode);
                //                        switch (PayIdentificationByCreditState)
                //                        {
                //                            case PayIdentificationByCreditStateEnum.SUCCESSFUL:
                //                                response.State = ResponseStateEnum.SUCCESS;
                //                                reportID = cryptography.Encrypt(identificationItem.VCode.ToString(), EncryptKeyEnum.Key1);
                //                                response.Data.Add(new AddIdentificationFirmResponse
                //                                {
                //                                    ExpireDate = identificationItem.ReportExpirationDate,
                //                                    ReportUrl = "",
                //                                    ReportID = reportID,
                //                                    NationalCode = body.NationalCode,
                //                                    ResponseMessage = "درخواست شما با موفقیت ثبت شد.",
                //                                    ResponseState = (IdentificationStateEnum)identificationItem.IdentificationState.VCode
                //                                });
                //                                sendVerificationLink = true;
                //                                break;
                //                            case PayIdentificationByCreditStateEnum.CREDIT_IS_NOT_ENOUGH:
                //                                response.Data.Add(new AddIdentificationFirmResponse
                //                                {
                //                                    ExpireDate = identificationItem.ReportExpirationDate,
                //                                    ReportUrl = "",
                //                                    ReportID = reportID,
                //                                    NationalCode = body.NationalCode,
                //                                    ResponseMessage = "اعتبار شما برای دریافت این گذارش کافی نیست به منو شارژ اعتبار مراجعه نمایید.",
                //                                    ResponseState = (IdentificationStateEnum)identificationItem.IdentificationState.VCode
                //                                });
                //                                break;

                //                        }
                //                    }
                //                    else if (identificationItem.UserPaymentTypeVCode == UserPaymentTypeEnum.PAY_BY_CUSTOMER)
                //                    {
                //                        response.State = ResponseStateEnum.SUCCESS;
                //                        reportID = WebUtility.UrlEncode(cryptography.Encrypt(identificationItem.VCode.ToString(), EncryptKeyEnum.Key1));
                //                        response.Data.Add(new AddIdentificationFirmResponse
                //                        {
                //                            ExpireDate = identificationItem.ReportExpirationDate,
                //                            ReportUrl = "",
                //                            ReportID = Regex.Replace(reportID, "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()),
                //                            NationalCode = body.NationalCode,
                //                            ResponseMessage = "درخواست شما با موفقیت ثبت شد.",
                //                            ResponseState = (IdentificationStateEnum)identificationItem.IdentificationState.VCode
                //                        });
                //                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                //                    }
                //                    sendVerificationLink = true;
                //                    break;
                //                case IdentificationInsStateEnum.CREDIT_IS_NOT_ENOUGH:
                //                    var errorEnumCode = ((int)ApiErrorCodeEnum.CREDIT_IS_NOT_ENOUGH).ToString();
                //                    response.State = ResponseStateEnum.FAILED;
                //                    response.Data.Add(new AddIdentificationFirmResponse
                //                    {
                //                        ExpireDate = DateTime.Now,
                //                        ReportUrl = "",
                //                        ReportID = "",
                //                        NationalCode = body.NationalCode,
                //                        ResponseMessage = "اعتبار شما برای دریافت این گذارش کافی نیست به منو شارژ اعتبار مراجعه نمایید.",
                //                        ResponseState = (IdentificationStateEnum)IdentificationStateEnum.NONE
                //                    });
                //                    response.Errors.Add(errorEnumCode);
                //                    break;
                //                case IdentificationInsStateEnum.NONE:
                //                    response.State = ResponseStateEnum.FAILED;
                //                    response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //                    break;
                //            }
                //            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                //        }
                //    }
                //    if (sendVerificationLink && identificationItem != null)
                //    {
                //        var objUser = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                //        string msg = "آیس:" + "\n" + (objUser.Name != null ? objUser.Name : "") + " درخواست مشاهده رتبه اعتباری شما را دارد." + "\n" + " لینک تایید:" + "\n";
                //        msg += "https://icescoring.com" + identificationItem.VerificationLink;
                //        Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: (!string.IsNullOrEmpty(identificationItem.SecondCellphone) ? identificationItem.SecondCellphone : identificationItem.Cellphone), message: msg, ip: "", identificationVCode: identificationItem.VCode, typeVCode: SMSTypeEnum.CONFIRMATION, operatorType: OperatorTypeEnum.ICS_IRANSMS);
                //    }
                //}
                //else
                //{
                //    var errorEnumCode = ((int)ApiErrorCodeEnum.USER_IS_NOT_APPROVED).ToString();
                //    response.State = ResponseStateEnum.NOTAUTHORIZED;
                //    response.Errors.Add(errorEnumCode);
                //    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                //}
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

        [Route("GetReadyForSeeIdentification")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetReadyForSeeIdentification(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<IdentificationFrontData>>();
            try
            {
                //if (isFirstInitial)
                //{
                //    PersianCalendar objPersianCalendar = new PersianCalendar();
                //    //var currentDate = DateTime.Now;
                //    //var month = objPersianCalendar.GetMonth(currentDate).ToString().Length == 1 ? "0" + objPersianCalendar.GetMonth(currentDate).ToString() : objPersianCalendar.GetMonth(currentDate).ToString();
                //    //var day = objPersianCalendar.GetDayOfMonth(currentDate).ToString().Length == 1 ? "0" + objPersianCalendar.GetDayOfMonth(currentDate).ToString() : objPersianCalendar.GetDayOfMonth(currentDate).ToString();
                //    //var year = objPersianCalendar.GetYear(currentDate).ToString();
                //    //var day2 = objPersianCalendar.GetDayOfMonth(currentDate.AddDays(-2)).ToString().Length == 1 ? "0" + objPersianCalendar.GetDayOfMonth(currentDate.AddDays(-2)).ToString() : objPersianCalendar.GetDayOfMonth(currentDate.AddDays(-2)).ToString();
                //    //var month2 = objPersianCalendar.GetMonth(currentDate.AddDays(-2)).ToString().Length == 1 ? "0" + objPersianCalendar.GetMonth(currentDate.AddDays(-2)).ToString() : objPersianCalendar.GetMonth(currentDate.AddDays(-2)).ToString();
                //    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                //    toDate = DateTime.Now.ToJalali().ToString();
                //}
                var identificationStates = new List<IdentificationStateData>();
                //identificationStates.Add(new IdentificationStateData() { VCode = 3 });
                identificationStates.Add(new IdentificationStateData() { VCode = 4 });
                identificationStates.Add(new IdentificationStateData() { VCode = 13 });
                //identificationStates.Add(new IdentificationStateData() { VCode = 18 });
                var identificationList = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentifications(userVCode: CurrentUser.VCode, identificationStates: identificationStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate, withouExpired: true);
                var identificationList2 = identificationList.Where(x => string.IsNullOrEmpty(x.ReportLink));
                identificationList.RemoveAll(i => identificationList2.Contains(i));
                var orderdList = identificationList.OrderByDescending(o => o.VCode).ToList();
                response.Data = orderdList.Select(t => new IdentificationFrontData { VCode = t.VCode, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, IdentificationState = t.IdentificationState, UserPaymentTypeName = t.UserPaymentTypeName, UserPaymentTypeEnum = t.UserPaymentTypeVCode, ReportLink = MainURL + t.ReportLink }).ToList();
                var errorEnumCode = ((int)ApiErrorCodeEnum.USER_IS_NOT_APPROVED).ToString();
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


        [Route("GetWaitingConfirmIdentification")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetWaitingConfirmIdentification(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            CryptographyBusiness cryptography = new CryptographyBusiness();
            var response = new ActionResponse<List<IdentificationFrontData>>();
            try
            {
                var identificationStates = new List<IdentificationStateData>();
                identificationStates.Add(new IdentificationStateData() { VCode = 1 });
                identificationStates.Add(new IdentificationStateData() { VCode = 2 });
                identificationStates.Add(new IdentificationStateData() { VCode = 3 });
                identificationStates.Add(new IdentificationStateData() { VCode = 11 });
                identificationStates.Add(new IdentificationStateData() { VCode = 12 });
                identificationStates.Add(new IdentificationStateData() { VCode = 17 });
                identificationStates.Add(new IdentificationStateData() { VCode = 18 });
                identificationStates.Add(new IdentificationStateData() { VCode = 19 });
                var identificationList = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentifications(userVCode: CurrentUser.VCode, identificationStates: identificationStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: Unify(fromDate), toDate: Unify(toDate), withouExpired: true);
                var identificationList2 = identificationList.Where(x => !string.IsNullOrEmpty(x.ReportLink) & x.IdentificationState.VCode == (int)IdentificationStateEnum.ICS24_CONFIRMED);
                identificationList.RemoveAll(i => identificationList2.Contains(i));

                response.Data = identificationList.Select(t => new IdentificationFrontData { VCode = t.VCode, Id = cryptography.Encrypt(t.VCode.ToString(), EncryptKeyEnum.Key1), EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, IdentificationState = t.IdentificationState, UserPaymentTypeName = t.UserPaymentTypeName, UserPaymentTypeEnum = t.UserPaymentTypeVCode }).ToList();
                var errorEnumCode = ((int)ApiErrorCodeEnum.USER_IS_NOT_APPROVED).ToString();
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

        [Route("GetRejectedIdentification")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetRejectedIdentification(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<IdentificationFrontData>>();
            try
            {
                var identificationStates = new List<IdentificationStateData>();
                identificationStates.Add(new IdentificationStateData() { VCode = 7 });
                var identificationList = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentifications(userVCode: CurrentUser.VCode, identificationStates: identificationStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: Unify(fromDate), toDate: Unify(toDate), withouExpired: true);
                var identificationList2 = identificationList.Where(x => !string.IsNullOrEmpty(x.ReportLink) & x.IdentificationState.VCode == (int)IdentificationStateEnum.ICS24_CONFIRMED);
                identificationList.RemoveAll(i => identificationList2.Contains(i));

                response.Data = identificationList.Select(t => new IdentificationFrontData { VCode = t.VCode, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, IdentificationState = t.IdentificationState, UserPaymentTypeName = t.UserPaymentTypeName, UserPaymentTypeEnum = t.UserPaymentTypeVCode }).ToList();
                var errorEnumCode = ((int)ApiErrorCodeEnum.USER_IS_NOT_APPROVED).ToString();
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
        [Route("GetArchiveIdentification")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetArchiveIdentification(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<IdentificationFrontData>>();
            try
            {
                var identificationStates = new List<IdentificationStateData>();
                identificationStates.Add(new IdentificationStateData() { VCode = 4 });
                identificationStates.Add(new IdentificationStateData() { VCode = 5 });
                identificationStates.Add(new IdentificationStateData() { VCode = 6 });
                identificationStates.Add(new IdentificationStateData() { VCode = 8 });
                identificationStates.Add(new IdentificationStateData() { VCode = 10 });
                identificationStates.Add(new IdentificationStateData() { VCode = 14 });
                identificationStates.Add(new IdentificationStateData() { VCode = 15 });
                identificationStates.Add(new IdentificationStateData() { VCode = 16 });
                identificationStates.Add(new IdentificationStateData() { VCode = 17 });
                var identificationList = ICE.Business.Engine.Instance.IdentificationBusiness.GetIdentifications(userVCode: CurrentUser.VCode, identificationStates: identificationStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: Unify(fromDate), toDate: Unify(toDate), withouExpired: false);
                var identificationList2 = identificationList.Where(x => !string.IsNullOrEmpty(x.ReportLink) & x.IdentificationState.VCode == (int)IdentificationStateEnum.ICS24_CONFIRMED);
                identificationList.RemoveAll(i => identificationList2.Contains(i));

                response.Data = identificationList.Select(t => new IdentificationFrontData { VCode = t.VCode, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, IdentificationState = t.IdentificationState, UserPaymentTypeName = t.UserPaymentTypeName, UserPaymentTypeEnum = t.UserPaymentTypeVCode }).ToList().OrderByDescending(o => o.VCode).ToList(); ;
                var errorEnumCode = ((int)ApiErrorCodeEnum.USER_IS_NOT_APPROVED).ToString();
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

        [Route("GetIdentificationVerifyCodeRemainTime")]
        [Route("GetIdentificationExpireTime")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
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
                    //if (testEnabled)
                    //{
                    //    DateTime d = new DateTime();
                    //    var totalSec = d.AddSeconds(240) > DateTime.Now ? Math.Round((d.AddSeconds(240) - DateTime.Now).TotalSeconds) : 0;
                    //    data = (new { success = true, expire = totalSec });
                    //}
                    //else
                    //{
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
                    var totalSec = identification.ExpirationDate > DateTime.Now ? Math.Round((identification.ExpirationDate - DateTime.Now).TotalSeconds) : 0;
                    data = (new { success = true, expire = totalSec });
                    //}

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
        [Route("GetFirmRequestIsDisabled")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetFirmRequestIsDisabled()
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                object data = null;
                string disableReportStr = Engine.Instance.ApplicationBusiness.GetSetting("DisableFirmSubmit");
                bool disableFirmReport = (disableReportStr == "1");
                data = (new { disableFirmSubmit = disableFirmReport });
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
        private bool ApiUserPaymentCheck(UserData user)
        {
            bool isAuthorize = true;
            try
            {
                if (user.IsApiFree)
                {
                    if (user.ApiExpirationDate != DateTime.MinValue && user.ApiExpirationDate < DateTime.Now)
                    {
                        isAuthorize = false;
                    }
                }
                else
                {
                    if (user.APIs.Find(x => x.VCode == 10 || x.VCode == 13) != null)
                    {
                        var firmExpreData = ICE.Business.Engine.Instance.FirmBusiness.IsFirmApiExpired(user.VCode);
                        if (firmExpreData != null && firmExpreData.IsFirmApiExpired)
                        {
                            isAuthorize = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogError(error: ex, request: HttpContext.Current.Request);
                throw;
            }
            return isAuthorize;
        }


    }
}
