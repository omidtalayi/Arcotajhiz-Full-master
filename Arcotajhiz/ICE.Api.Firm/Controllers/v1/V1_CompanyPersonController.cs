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
using ICE.Presentation.Common.Models;

namespace ICE.Api.Firm.Controllers.v1
{
    [RoutePrefix("api/v1/companyPerson")]
    public class V1_CompanyPersonController : BaseApiPresentationController
    {
        [Route("AddCompanyPersonFromFirm")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult AddCompanyPersonFromFirm([FromBody] AddCompanyPersonFirmRequestBody body)
        {
            string token = CurrentUser.token;
            bool success = false;
            bool sendVerificationLink = false;
            bool sendPayLink = false;
            string reportID;
            decimal remainCredit = 0;
            CompanyPersonData companyPersonItem = new CompanyPersonData();
            var response = new ActionResponse<AddCompanyPersonFirmResponse>();
            var responseData = new AddCompanyPersonFirmResponse();
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
                        remainCredit = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode).Credit;
                        responseData.RemainCredit = remainCredit;
                        var companyPersonLinkAndExpirationDate = Engine.Instance.CompanyPersonBusiness.GetCompanyPersonReportLinkAndExpirationDate(cellphone: body.CellPhone, nationalCode: body.NationalCode, UserVCode: CurrentUser.VCode, companyNationalId: body.CompanyNationalID);
                        if (companyPersonLinkAndExpirationDate != null)
                        {
                            switch (companyPersonLinkAndExpirationDate.CompanyPersonState)
                            {
                                case CompanyPersonStateEnum.PAID:
                                    responseData.ExpireDate = companyPersonLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = companyPersonLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "companyPerson/PayComplete?id=" + companyPersonLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر قبلا ثبت شده است و منتظر تایید می باشد.";
                                    responseData.ResponseState = companyPersonLinkAndExpirationDate.CompanyPersonState;

                                    sendVerificationLink = true;
                                    break;
                                case CompanyPersonStateEnum.ACCEPTED:
                                    sendPayLink = true;
                                    responseData.ExpireDate = companyPersonLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = companyPersonLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "companyPerson/PayComplete?id=" + companyPersonLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر تایید شده و در انتظار دریافت اطلاعات از مرکز می باشد.";
                                    responseData.ResponseState = companyPersonLinkAndExpirationDate.CompanyPersonState;
                                    break;
                                case CompanyPersonStateEnum.READY_FOR_SEE:
                                case CompanyPersonStateEnum.SEEN:
                                    responseData.ExpireDate = companyPersonLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = MainURL + "gop/" + companyPersonLinkAndExpirationDate.GUID;
                                    responseData.ReportID = companyPersonLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "companyPerson/PayComplete?id=" + companyPersonLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر قبلا ثبت و اطلاعات ایشان دریافت شده و قابل مشاهده می باشد.";
                                    responseData.ResponseState = companyPersonLinkAndExpirationDate.CompanyPersonState;
                                    break;
                            }
                            success = false;
                        }
                        else
                        {
                            long GOPVCode = 0;
                            string GOPGUID = "";
                            var result = Engine.Instance.CompanyPersonBusiness.AddCompanyPerson(vcode: ref GOPVCode, cellphone: body.CellPhone, nationalCode: body.NationalCode, userVCode: CurrentUser.VCode, companyNationalId: body.CompanyNationalID, userPaymentTypeVCode: body.UserPaymentTypeEnum, fromApp: false, guid: ref GOPGUID);
                            if (result == GroupOfPersonInsStateEnum.SUCCESSFUL)
                            {
                                companyPersonItem = Engine.Instance.CompanyPersonBusiness.GetCompanyPerson(vcode: GOPVCode);
                                //pay by credit of firm
                                if (companyPersonItem.UserPaymentTypeVCode == (int)UserPaymentTypeEnum.PAY_FROM_CREDIT)
                                {
                                    var PayCompanyPersonByCreditState = Engine.Instance.PaymentBusiness.PayCompanyPersonByCredit(userVCode: user.VCode, companyPersonVCode: companyPersonItem.VCode);
                                    switch (PayCompanyPersonByCreditState)
                                    {
                                        case PayCompanyPersonByCreditStateEnum.SUCCESSFUL:
                                            response.State = ResponseStateEnum.SUCCESS;
                                            reportID = companyPersonItem.GUID;
                                            remainCredit = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode).Credit;
                                            var responseData3 = new AddCompanyPersonFirmResponse
                                            {
                                                ExpireDate = companyPersonItem.ReportExpirationDate,
                                                ReportUrl = "",
                                                VerifyUrl = MainURL + "companyPerson/PayComplete?id=" + reportID,
                                                RemainCredit = remainCredit,
                                                ReportID = Regex.Replace(reportID, "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()),
                                                NationalCode = companyPersonItem.NationalCode,
                                                ResponseMessage = "درخواست شما با موفقیت ثبت شد.",
                                                ResponseState = companyPersonItem.CompanyPersonStateVCode
                                            };
                                            responseData = responseData3;
                                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                            if (!user.SelfOtp)
                                            {
                                                sendVerificationLink = true;
                                            }
                                            var extraInfoCode = ApiCall.Instance.EmtaSetExtraDataToSsoRequest(companyPersonItem.Cellphone, companyPersonItem.CompanyNationalID);
                                            ICE.Business.Engine.Instance.CompanyPersonBusiness.SetCompanyPersonExtraInfoCode(CompanyPersonVCode: companyPersonItem.VCode, extraInfoCode: extraInfoCode);
                                            success = true;
                                            break;
                                        case PayCompanyPersonByCreditStateEnum.CREDIT_IS_NOT_ENOUGH:
                                            var x = Engine.Instance.CompanyPersonBusiness.CancelCompanyPerson(CompanyPersonVCode: companyPersonItem.VCode, userVCode: CurrentUser.VCode, CompanyPersonStateVCode: CompanyPersonStateEnum.DELETED);
                                            response.Errors.Add(((int)(ApiErrorCodeEnum.CREDIT_IS_NOT_ENOUGH)).ToString());
                                            var responseData4 = new AddCompanyPersonFirmResponse()
                                            {
                                                ResponseMessage = "خطا : اعتبار شما برای انجام این درخواست کافی نیست. ابتدا حساب کاربری خود را از طریق پنل حقوقی firmpanel.icescoring.com شارژ نمایید و مجددا اقدام به دریافت گزارش نمایید."
                                            };
                                            responseData = responseData4;
                                            break;
                                    }
                                }
                                //pay by customer
                                else
                                {
                                    responseData.ExpireDate = companyPersonItem.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = GOPGUID;
                                    responseData.NationalCode = companyPersonItem.NationalCode;
                                    responseData.VerifyUrl = MainURL + "companyPerson/PayIndex?id=" + GOPGUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر ثبت شد و منتظر تایید می باشد.";
                                    responseData.ResponseState = companyPersonItem.CompanyPersonStateVCode;
                                    sendPayLink = true;
                                    success = true;
                                }

                            }
                            else if (result == GroupOfPersonInsStateEnum.CREDIT_IS_NOT_ENOUGH)
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
                        if (success)
                        {
                            response.State = ResponseStateEnum.SUCCESS;
                        }
                        else
                        {
                            response.State = ResponseStateEnum.FAILED;
                        }
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
            if (sendVerificationLink && companyPersonItem != null)
            {
                string msg = "آیس:" + "\n" + (user.Name != null ? user.Name : "") + " درخواست مشاهده استعلام ثبت شرکتها شما را دارد." + "\n";
                msg += MainURL + "companyPerson/PayComplete?id=" + companyPersonItem.GUID;
                Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: companyPersonItem.Cellphone, message: msg, ip: "", typeVCode: SMSTypeEnum.CONFIRMATION, operatorType: OperatorTypeEnum.GHASEDAK);
            }
            if (sendPayLink && companyPersonItem != null)
            {
                string msg = "آیس:" + "\n" + (user.Name != null ? user.Name : "") + " درخواست مشاهده استعلام ثبت شرکتها شما را دارد." + "\n";
                msg += MainURL + "companyPerson/PayIndex?id=" + companyPersonItem.GUID;
                Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: companyPersonItem.Cellphone, message: msg, ip: "", typeVCode: SMSTypeEnum.CONFIRMATION, operatorType: OperatorTypeEnum.GHASEDAK);
            }
            return ResponseMessage(objResponse);
        }

        [Route("CancelCompanyPerson")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult CancelCompanyPerson([FromBody] CancelCompanyPersonFirmRequest data)
        {
            UserData objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            CompanyPersonData companyPersonItem = new CompanyPersonData();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            var response = new ActionResponse<CancelCompanyPersonResponse>();
            HttpResponseMessage objResponse = null;
            bool success = false;
            try
            {
                companyPersonItem = Engine.Instance.CompanyPersonBusiness.GetCompanyPerson(guid: data.reportID);
                if (companyPersonItem != null)
                {
                    if (companyPersonItem.ReportExpirationDate >= DateTime.Now)
                    {
                        if (companyPersonItem.CompanyPersonStateVCode == CompanyPersonStateEnum.SUBMITTED || companyPersonItem.CompanyPersonStateVCode == CompanyPersonStateEnum.PAID)
                        {
                            if (companyPersonItem.UserPaymentTypeVCode == (int)UserPaymentTypeEnum.PAY_FROM_CREDIT)
                            {
                                success = Engine.Instance.PaymentBusiness.ReturnCompanyPersonCredit(companyPersonVCode: companyPersonItem.VCode, userVCode: objCurrentUser.VCode);
                            }
                            else
                            {
                                success = Engine.Instance.CompanyPersonBusiness.CancelCompanyPerson(CompanyPersonVCode: companyPersonItem.VCode, userVCode: objCurrentUser.VCode, CompanyPersonStateVCode: CompanyPersonStateEnum.DELETED);
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
                var responseData = new CancelCompanyPersonResponse
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

        [Route("GetReadyForSeeCompanyPerson")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetReadyForSeeCompanyPerson(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<CompanyPersonFrontData>>();
            try
            {
                //if (isFirstInitial)
                //{
                //    PersianCalendar objPersianCalendar = new PersianCalendar();
                //    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                //    toDate = DateTime.Now.ToJalali().ToString();
                //}
                List<CompanyPersonStateEnum> companyPersonStates = new List<CompanyPersonStateEnum>();
                companyPersonStates.Add(CompanyPersonStateEnum.READY_FOR_SEE);
                companyPersonStates.Add(CompanyPersonStateEnum.SEEN);
                var companyPersonList = Engine.Instance.CompanyPersonBusiness.GetCompanyPersonList(userVCode: CurrentUser.VCode, CompanyPersonStates: companyPersonStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate, withouExpired: true);
                var orderdList = companyPersonList.OrderByDescending(o => o.VCode).ToList();
                response.Data = orderdList.Select(t => new CompanyPersonFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, CompanyPersonState = t.CompanyPersonStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "https://www.icescoring.com/" + t.ReportLink, CompanyPersonStateName = t.CompanyPersonStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList();
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


        [Route("GetWaitingConfirmCompanyPerson")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetWaitingConfirmCompanyPerson(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<CompanyPersonFrontData>>();
            try
            {
                if (isFirstInitial)
                {
                    PersianCalendar objPersianCalendar = new PersianCalendar();
                    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                    toDate = DateTime.Now.ToJalali().ToString();
                }
                List<CompanyPersonStateEnum> companyPersonStates = new List<CompanyPersonStateEnum>();
                companyPersonStates.Add(CompanyPersonStateEnum.SUBMITTED);
                companyPersonStates.Add(CompanyPersonStateEnum.PAID);
                companyPersonStates.Add(CompanyPersonStateEnum.ACCEPTED);
                var companyPersonList = Engine.Instance.CompanyPersonBusiness.GetCompanyPersonList(userVCode: CurrentUser.VCode, CompanyPersonStates: companyPersonStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate, withouExpired: true);
                var orderdList = companyPersonList.OrderByDescending(o => o.VCode).ToList();
                response.Data = orderdList.Select(t => new CompanyPersonFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, CompanyPersonState = t.CompanyPersonStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "", CompanyPersonStateName = t.CompanyPersonStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList();
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

        [Route("GetRejectedCompanyPerson")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetRejectedCompanyPerson(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<CompanyPersonFrontData>>();
            try
            {
                if (isFirstInitial)
                {
                    PersianCalendar objPersianCalendar = new PersianCalendar();
                    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                    toDate = DateTime.Now.ToJalali().ToString();
                }
                List<CompanyPersonStateEnum> companyPersonStates = new List<CompanyPersonStateEnum>();
                companyPersonStates.Add(CompanyPersonStateEnum.REJECTED);
                companyPersonStates.Add(CompanyPersonStateEnum.DELETED);
                var companyPersonList = ICE.Business.Engine.Instance.CompanyPersonBusiness.GetCompanyPersonList(userVCode: CurrentUser.VCode, CompanyPersonStates: companyPersonStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate);
                var companyPersonList2 = companyPersonList.Where(x => string.IsNullOrEmpty(x.ReportLink));
                companyPersonList.RemoveAll(i => companyPersonList2.Contains(i));
                var orderdList = companyPersonList.OrderByDescending(o => o.VCode).ToList();
                response.Data = orderdList.Select(t => new CompanyPersonFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, CompanyPersonState = t.CompanyPersonStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "", CompanyPersonStateName = t.CompanyPersonStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList();
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
        [Route("GetArchiveCompanyPerson")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetArchiveCompanyPerson(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<CompanyPersonFrontData>>();
            try
            {
                List<CompanyPersonStateEnum> companyPersonStates = new List<CompanyPersonStateEnum>();
                companyPersonStates.Add(CompanyPersonStateEnum.SUBMITTED);
                companyPersonStates.Add(CompanyPersonStateEnum.PAID);
                companyPersonStates.Add(CompanyPersonStateEnum.ACCEPTED);
                companyPersonStates.Add(CompanyPersonStateEnum.READY_FOR_SEE);
                companyPersonStates.Add(CompanyPersonStateEnum.SEEN);
                companyPersonStates.Add(CompanyPersonStateEnum.DELETED);
                companyPersonStates.Add(CompanyPersonStateEnum.REJECTED);
                var companyPersonList = ICE.Business.Engine.Instance.CompanyPersonBusiness.GetCompanyPersonList(userVCode: CurrentUser.VCode, CompanyPersonStates: companyPersonStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: Unify(fromDate), toDate: Unify(toDate), withouExpired: false);
                var companyPersonList2 = companyPersonList.Where(x => x.ReportExpirationDate >= DateTime.Now);
                companyPersonList.RemoveAll(i => companyPersonList2.Contains(i));

                response.Data = companyPersonList.Select(t => new CompanyPersonFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, CompanyPersonState = t.CompanyPersonStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "", CompanyPersonStateName = t.CompanyPersonStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList().OrderByDescending(o => o.VCode).ToList(); ;
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

        [Route("GetReportData")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.COMPANY_PERSON))]
        public IHttpActionResult GetReportData(string reportID)
        {
            CompanyPersonData companyPersonItem = new CompanyPersonData();
            var response = new ActionResponse<GetReportDataResponseCompanyPerson>();
            HttpResponseMessage objResponse = null;
            string responseData = null;
            CreditRiskReportEnum reportState = CreditRiskReportEnum.NONE;
            try
            {

                companyPersonItem = ICE.Business.Engine.Instance.CompanyPersonBusiness.GetCompanyPerson(guid:reportID);
                if (companyPersonItem != null)
                {
                    if (CurrentUser != null && companyPersonItem.UserVCode == CurrentUser.VCode)
                    {
                        if (companyPersonItem.ReportExpirationDate >= DateTime.Now)
                        {
                            string IndividualReport = "";
                            string EmptyReport = "";
                            if (companyPersonItem.CompanyPersonStateVCode == CompanyPersonStateEnum.READY_FOR_SEE || companyPersonItem.CompanyPersonStateVCode == CompanyPersonStateEnum.SEEN)
                            {
                                var batchCompanyPersonsResult = ICE.Core.Business.Engine.Instance.BatchCompanyPersonBusiness.GetBatchCompanyPersonResult(CompanyPersonVCode: companyPersonItem.VCode);

                                if (batchCompanyPersonsResult != null)
                                {
                                    var companyPersonReportData = JsonConvert.DeserializeObject<CompanyPersonReportData>(batchCompanyPersonsResult.ResponseJSon);
                                    responseData = batchCompanyPersonsResult.ResponseJSon;
                                    reportState = CreditRiskReportEnum.INDIVIDUAL_REPORT;
                                    GetReportDataResponseCompanyPerson data = new GetReportDataResponseCompanyPerson()
                                    {
                                        EmptyReportData = EmptyReport,
                                        ReportData = companyPersonReportData,
                                        ReportState = reportState,
                                        ReportLink = MainURL + companyPersonItem.ReportLink,
                                    };
                                    response.State = ResponseStateEnum.SUCCESS;
                                    response.Data = data;
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                }
                                else 
                                {
                                    GetReportDataResponseCompanyPerson data = new GetReportDataResponseCompanyPerson()
                                    {
                                        ReportData = null,
                                        ReportState = CreditRiskReportEnum.EMPTY_REPORT
                                    };
                                    response.State = ResponseStateEnum.FAILED;
                                    response.Data = data;
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                }
                            }
                            else
                            {
                                GetReportDataResponseCompanyPerson data = new GetReportDataResponseCompanyPerson()
                                {
                                    ReportData = null,
                                    ReportState = CreditRiskReportEnum.INVALID_STATE

                                };
                                response.State = ResponseStateEnum.FAILED;
                                response.Data = data;
                                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                            }
                        }
                        else
                        {
                            GetReportDataResponseCompanyPerson data = new GetReportDataResponseCompanyPerson()
                            {
                                ReportData = null,
                                ReportState = CreditRiskReportEnum.IS_EXPIRED

                            };
                            response.State = ResponseStateEnum.FAILED;
                            response.Data = data;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                    }
                    else
                    {
                        GetReportDataResponseCompanyPerson data = new GetReportDataResponseCompanyPerson()
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

        [Route("GetReportState")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.COMPANY_PERSON))]
        public IHttpActionResult GetReportState(string reportID)
        {
            CompanyPersonData companyPersonItem = new CompanyPersonData();
            var response = new ActionResponse<GetReportDataState>();
            HttpResponseMessage objResponse = null;
            string responseData = null;
            try
            {
                companyPersonItem = ICE.Business.Engine.Instance.CompanyPersonBusiness.GetCompanyPerson(guid:reportID);
                if (companyPersonItem != null)
                {
                    if (CurrentUser != null && companyPersonItem.UserVCode == CurrentUser.VCode)
                    {
                        if (companyPersonItem.ReportExpirationDate >= DateTime.Now)
                        {
                            GetReportDataState data = new GetReportDataState()
                            {
                                ReportState = companyPersonItem.CompanyPersonStateVCode,
                                ReportStateStr = companyPersonItem.CompanyPersonStateName
                            };
                            response.State = ResponseStateEnum.SUCCESS;
                            response.Data = data;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                        else
                        {
                            GetReportDataState data = new GetReportDataState()
                            {
                                ReportState = CompanyPersonStateEnum.IS_EXPIRED,
                                ReportStateStr = "گزارش مورد نظر منقضی شده است."

                            };
                            response.State = ResponseStateEnum.FAILED;
                            response.Data = data;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        }
                    }
                    else
                    {
                        GetReportDataState data = new GetReportDataState()
                        {
                            ReportState = CompanyPersonStateEnum.NONE,
                            ReportStateStr = "خطا : کاربر اشتباه است ورودی های خود را چک کنید."

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
