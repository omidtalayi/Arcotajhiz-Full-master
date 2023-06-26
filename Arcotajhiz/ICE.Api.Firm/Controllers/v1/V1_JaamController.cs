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
using ICE.Core.Business.Models;
using RestSharp;

namespace ICE.Api.Firm.Controllers.v1
{
    [RoutePrefix("api/v1/jaam")]
    public class V1_JaamController : BaseApiPresentationController
    {
        [Route("AddJaamFromFirm")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult AddJaamFromFirm([FromBody] AddJaamFirmRequestBody body)
        {
            string token = CurrentUser.token;
            bool sendVerificationLink = false;
            bool sendPayLink = false;
            string reportID;
            decimal remainCredit = 0;
            JaamData jaamItem = new JaamData();
            var response = new ActionResponse<AddJaamFirmResponse>();
            var responseData = new AddJaamFirmResponse();
            var objResponse = new HttpResponseMessage();
            UserData user = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            var disableEmtaSubmitStr = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("DisableEmtaGraphSubmit");
            bool disableEmtaSubmit = disableEmtaSubmitStr == "1";
            try
            {
                if (!string.IsNullOrEmpty(token) && !disableEmtaSubmit)
                {
                    var apiErrorEnum = body.Validation();
                    if (apiErrorEnum != ApiErrorCodeEnum.NONE)
                    {
                        response.State = ResponseStateEnum.FAILED;
                        response.Errors.Add(((int)apiErrorEnum).ToString());
                        responseData.ResponseMessage = "لطفا ورودی های خود را چک کنیید و مجددا تلاش نمایید.";
                        response.Data = responseData;
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else
                    {
                        var jaamLinkAndExpirationDate = Engine.Instance.JaamBusiness.GetJaamReportLinkAndExpirationDate(cellphone: body.CellPhone, nationalCode: body.NationalCode, UserVCode: CurrentUser.VCode, companyNationalId: body.CompanyNationalID);
                        if (jaamLinkAndExpirationDate != null)
                        {
                            switch (jaamLinkAndExpirationDate.JaamState)
                            {
                                case JaamStateEnum.PAID:
                                    responseData.ExpireDate = jaamLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = jaamLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "jaam/PayComplete?id=" + jaamLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر قبلا ثبت شده است و منتظر تایید می باشد.";
                                    responseData.ResponseState = jaamLinkAndExpirationDate.JaamState;
                                    sendVerificationLink = true;
                                    break;
                                case JaamStateEnum.ACCEPTED:
                                    sendPayLink = true;
                                    responseData.ExpireDate = jaamLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = jaamLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "jaam/PayComplete?id=" + jaamLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر تایید شده ثبت شده و در انتظار دریافت اطلاعات از مرکز می باشد.";
                                    responseData.ResponseState = jaamLinkAndExpirationDate.JaamState;
                                    break;
                                case JaamStateEnum.READY_FOR_SEE:
                                case JaamStateEnum.SEEN:
                                    responseData.ExpireDate = jaamLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = MainURL + "jam/" + jaamLinkAndExpirationDate.GUID;
                                    responseData.ReportID = jaamLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "jaam/PayComplete?id=" + jaamLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "اطلاعات رکورد فرد مورد نظر دریافت شده و قابل مشاهده می باشد..";
                                    responseData.ResponseState = jaamLinkAndExpirationDate.JaamState;
                                    break;
                            }
                        }
                        else
                        {
                            long JaamVCode = 0;
                            string JaamGUID = "";
                            string name = user.Name;
                            string jaamRequestNo = "";
                            var resultData = ApiCall.Instance.JamAddNewRequest(NationalID: body.CompanyNationalID, MaliYear: body.fiscalYear, requesterNationalID: body.NationalCode, requesterName: name);
                            if (resultData != null && bool.Parse(resultData["state"].ToString()))
                            {
                                jaamRequestNo = resultData["RequestNo"].ToString();
                                var result = Engine.Instance.JaamBusiness.AddJaam(vcode: ref JaamVCode, cellphone: user.CellPhone, name: name, nationalCode: body.NationalCode, userVCode: CurrentUser.VCode, companyNationalId: body.CompanyNationalID, fiscalYear: body.fiscalYear.ToString(), userPaymentTypeVCode: body.UserPaymentTypeEnum, fromApp: false, guid: ref JaamGUID);
                                if (result == JaamInsStateEnum.SUCCESSFUL)
                                {
                                    jaamItem = Engine.Instance.JaamBusiness.GetJaam(vcode: JaamVCode);
                                    //pay by credit of firm
                                    if (jaamItem.UserPaymentTypeVCode == (int)UserPaymentTypeEnum.PAY_FROM_CREDIT)
                                    {
                                        var PayJaamByCreditState = Engine.Instance.PaymentBusiness.PayJaamByCredit(userVCode: user.VCode, jaamVCode: jaamItem.VCode);
                                        switch (PayJaamByCreditState)
                                        {
                                            case PayJaamByCreditStateEnum.SUCCESSFUL:
                                                response.State = ResponseStateEnum.SUCCESS;
                                                reportID = jaamItem.GUID;
                                                remainCredit = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode).Credit;
                                                var responseData3 = new AddJaamFirmResponse
                                                {
                                                    ExpireDate = jaamItem.ReportExpirationDate,
                                                    ReportUrl = "",
                                                    VerifyUrl = "",
                                                    RemainCredit = remainCredit,
                                                    ReportID = reportID,
                                                    NationalCode = jaamItem.NationalCode,
                                                    ResponseMessage = "درخواست شما با موفقیت  ثبت شد، لطفا منتظر تایید مدیر عامل شرکت مورد نظر باشد.",
                                                    ResponseState = jaamItem.JaamStateVCode
                                                };
                                                Engine.Instance.JaamBusiness.SetJaamRequestNumber(jaamVCode: JaamVCode, requestNumber: jaamRequestNo, requestStatus: 1);
                                                responseData = responseData3;
                                                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                                if (!user.SelfOtp)
                                                {
                                                    sendVerificationLink = true;
                                                }
                                                break;
                                            case PayJaamByCreditStateEnum.CREDIT_IS_NOT_ENOUGH:
                                                response.Errors.Add(((int)(ApiErrorCodeEnum.CREDIT_IS_NOT_ENOUGH)).ToString());
                                                response.Data = new AddJaamFirmResponse()
                                                {
                                                    ResponseMessage = "خطا : اعتبار شما برای انجام این درخواست کافی نیست. ابتدا حساب کاربری خود را از طریق پنل حقوقی firmpanel.icescoring.com شارژ نمایید و مجددا اقدام به دریافت گزارش نمایید."
                                                };
                                                Engine.Instance.JaamBusiness.CancelJaam(JaamVCode: jaamItem.VCode, userVCode: CurrentUser.VCode, JaamStateVCode: JaamStateEnum.DELETED);
                                                break;
                                        }
                                    }
                                    //pay by customer
                                    else
                                    {
                                        Engine.Instance.JaamBusiness.SetJaamRequestNumber(jaamVCode: JaamVCode, requestNumber: jaamRequestNo, requestStatus: 1);
                                        responseData.ExpireDate = jaamItem.ReportExpirationDate;
                                        responseData.ReportUrl = "";
                                        responseData.ReportID = JaamGUID;
                                        responseData.NationalCode = jaamItem.NationalCode;
                                        responseData.VerifyUrl = MainURL + "jaam/PayIndex?id=" + JaamGUID;
                                        responseData.ResponseMessage = "رکورد فرد مورد نظر ثبت شد و منتظر تایید می باشد.";
                                        responseData.ResponseState = jaamItem.JaamStateVCode;
                                        sendPayLink = true;
                                    }
                                }
                                else if (result == JaamInsStateEnum.CREDIT_IS_NOT_ENOUGH)
                                {
                                    responseData.ExpireDate = DateTime.Now;
                                    responseData.NationalCode = body.NationalCode;
                                    responseData.ResponseMessage = "خطا : اعتبار شما برای انجام این درخواست کافی نیست. ابتدا حساب کاربری خود را از طریق پنل حقوقی firmpanel.icescoring.com شارژ نمایید و مجددا اقدام به دریافت گزارش نمایید.";
                                }
                                else
                                {
                                    responseData.ExpireDate = DateTime.Now;
                                    responseData.NationalCode = body.NationalCode;
                                    responseData.ResponseMessage = "خطا : در ثبت اطلاعات مشکلی پیش آمده لطفا اطلاعات خود را بررسی کنید.";
                                }
                            }
                            else
                            {
                                response.State = ResponseStateEnum.FAILED;
                                response.Errors.Add(resultData["msg"].ToString());
                                responseData.ResponseMessage = resultData["msg"].ToString();
                                response.Data = responseData;
                                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                return ResponseMessage(objResponse);
                            }
                        }
                        response.State = ResponseStateEnum.SUCCESS;
                        response.Data = responseData;
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
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
            if (sendVerificationLink && jaamItem != null)
            {
                string msg = "آیس:" + "\n" + (user.Name != null ? user.Name : "") + " درخواست مشاهده گزارش گروه اشخاص شما را دارد." + "\n" + " لینک تایید:" + "\n";
                msg += MainURL + "jaam/PayComplete?id=" + jaamItem.GUID;
                Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: jaamItem.Cellphone, message: msg, ip: "", typeVCode: SMSTypeEnum.CONFIRMATION, operatorType: OperatorTypeEnum.GHASEDAK);
            }
            if (sendPayLink && jaamItem != null)
            {
                string msg = "آیس:" + "\n" + (user.Name != null ? user.Name : "") + " درخواست مشاهده گزارش گروه اشخاص شما را دارد." + "\n" + " لینک پرداخت:" + "\n";
                msg += MainURL + "jaam/PayIndex?id=" + jaamItem.GUID;
                Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: jaamItem.Cellphone, message: msg, ip: "", typeVCode: SMSTypeEnum.CONFIRMATION, operatorType: OperatorTypeEnum.GHASEDAK);
            }
            return ResponseMessage(objResponse);
        }

        [Route("CancelJaam")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult CancelJaam([FromBody] CancelJaamFirmRequest data)
        {
            UserData objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            JaamData jaamItem = new JaamData();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            var response = new ActionResponse<CancelJaamResponse>();
            HttpResponseMessage objResponse = null;
            bool success = false;
            try
            {
                jaamItem = Engine.Instance.JaamBusiness.GetJaam(guid: data.reportID);
                if (jaamItem != null)
                {
                    if (jaamItem.ReportExpirationDate >= DateTime.Now)
                    {
                        if (jaamItem.JaamStateVCode == JaamStateEnum.SUBMITTED || jaamItem.JaamStateVCode == JaamStateEnum.PAID)
                        {
                            if (jaamItem.UserPaymentTypeVCode == (int)UserPaymentTypeEnum.PAY_FROM_CREDIT)
                            {
                                success = Engine.Instance.PaymentBusiness.ReturnJaamCredit(jaamVCode: jaamItem.VCode, userVCode: objCurrentUser.VCode);
                            }
                            else
                            {
                                success = Engine.Instance.JaamBusiness.CancelJaam(JaamVCode: jaamItem.VCode, userVCode: objCurrentUser.VCode, JaamStateVCode: JaamStateEnum.DELETED);
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
                var responseData = new CancelJaamResponse
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

        [Route("GetReadyForSeeJaam")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult GetReadyForSeeJaam(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<JaamFrontData>>();
            try
            {
                //if (isFirstInitial)
                //{
                //    PersianCalendar objPersianCalendar = new PersianCalendar();
                //    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                //    toDate = DateTime.Now.ToJalali().ToString();
                //}
                List<JaamStateEnum> jaamStates = new List<JaamStateEnum>();
                jaamStates.Add(JaamStateEnum.READY_FOR_SEE);
                jaamStates.Add(JaamStateEnum.SEEN);
                var jaamList = Engine.Instance.JaamBusiness.GetJaamList(userVCode: CurrentUser.VCode, JaamStates: jaamStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate, withouExpired: true);
                var orderdList = jaamList.OrderByDescending(o => o.VCode).ToList();
                //jaam test
                response.Data = orderdList.Select(t => new JaamFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, JaamState = t.JaamStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "https://dev.icescoring.com/jam/" + t.GUID, JaamStateName = t.JaamStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList();
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


        [Route("GetWaitingConfirmJaam")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult GetWaitingConfirmJaam(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<JaamFrontData>>();
            try
            {
                if (isFirstInitial)
                {
                    PersianCalendar objPersianCalendar = new PersianCalendar();
                    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                    toDate = DateTime.Now.ToJalali().ToString();
                }
                List<JaamStateEnum> jaamStates = new List<JaamStateEnum>();
                jaamStates.Add(JaamStateEnum.SUBMITTED);
                jaamStates.Add(JaamStateEnum.PAID);
                jaamStates.Add(JaamStateEnum.ACCEPTED);
                var jaamList = Engine.Instance.JaamBusiness.GetJaamList(userVCode: CurrentUser.VCode, JaamStates: jaamStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate, withouExpired: true);
                var orderdList = jaamList.OrderByDescending(o => o.VCode).ToList();
                response.Data = orderdList.Select(t => new JaamFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, JaamState = t.JaamStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "", JaamStateName = t.JaamStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList();
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

        [Route("GetRejectedJaam")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult GetRejectedJaam(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<JaamFrontData>>();
            try
            {
                if (isFirstInitial)
                {
                    PersianCalendar objPersianCalendar = new PersianCalendar();
                    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                    toDate = DateTime.Now.ToJalali().ToString();
                }
                List<JaamStateEnum> jaamStates = new List<JaamStateEnum>();
                jaamStates.Add(JaamStateEnum.REJECTED);
                jaamStates.Add(JaamStateEnum.DELETED);
                var jaamList = ICE.Business.Engine.Instance.JaamBusiness.GetJaamList(userVCode: CurrentUser.VCode, JaamStates: jaamStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate);
                var jaamList2 = jaamList.Where(x => string.IsNullOrEmpty(x.ReportLink));
                jaamList.RemoveAll(i => jaamList2.Contains(i));
                var orderdList = jaamList.OrderByDescending(o => o.VCode).ToList();
                response.Data = orderdList.Select(t => new JaamFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, JaamState = t.JaamStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "", JaamStateName = t.JaamStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList();
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
        [Route("GetArchiveJaam")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult GetArchiveJaam(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<JaamFrontData>>();
            try
            {
                List<JaamStateEnum> jaamStates = new List<JaamStateEnum>();
                jaamStates.Add(JaamStateEnum.SUBMITTED);
                jaamStates.Add(JaamStateEnum.PAID);
                jaamStates.Add(JaamStateEnum.ACCEPTED);
                jaamStates.Add(JaamStateEnum.READY_FOR_SEE);
                jaamStates.Add(JaamStateEnum.SEEN);
                jaamStates.Add(JaamStateEnum.DELETED);
                jaamStates.Add(JaamStateEnum.REJECTED);
                var jaamList = ICE.Business.Engine.Instance.JaamBusiness.GetJaamList(userVCode: CurrentUser.VCode, JaamStates: jaamStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: Unify(fromDate), toDate: Unify(toDate), withouExpired: false);
                var jaamList2 = jaamList.Where(x => x.ReportExpirationDate >= DateTime.Now);
                jaamList.RemoveAll(i => jaamList2.Contains(i));

                response.Data = jaamList.Select(t => new JaamFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, JaamState = t.JaamStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "", JaamStateName = t.JaamStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList().OrderByDescending(o => o.VCode).ToList(); ;
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
        private static string UpperCaseUrlEncode(string s)
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
    }
}
