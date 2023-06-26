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

namespace ICE.Api.Firm.Controllers.v1
{
    [RoutePrefix("api/v1/groupOfPersons")]
    public class V1_GroupOfPersonsController : BaseApiPresentationController
    {
        [Route("AddGroupOfPersonsFromFirm")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult AddGroupOfPersonsFromFirm([FromBody] AddGroupOfPersonsFirmRequestBody body)
        {
            string token = CurrentUser.token;
            bool sendVerificationLink = false;
            bool sendPayLink = false;
            string reportID;
            decimal remainCredit = 0;
            GroupOfPersonsData groupOfPersonsItem = new GroupOfPersonsData();
            var response = new ActionResponse<AddGroupOfPersonsFirmResponse>();
            var responseData = new AddGroupOfPersonsFirmResponse();
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
                        var groupOfPersonsLinkAndExpirationDate = Engine.Instance.GroupOfPersonsBusiness.GetGroupOfPersonsReportLinkAndExpirationDate(cellphone: body.CellPhone, nationalCode: body.NationalCode, UserVCode: CurrentUser.VCode, companyNationalId: body.CompanyNationalID);
                        if (groupOfPersonsLinkAndExpirationDate != null)
                        {
                            switch (groupOfPersonsLinkAndExpirationDate.GroupOfPersonsState)
                            {
                                case GroupOfPersonsStateEnum.PAID:
                                    responseData.ExpireDate = groupOfPersonsLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "groupOfPersons/PayComplete?id=" + groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر قبلا ثبت شده است و منتظر تایید می باشد.";
                                    responseData.ResponseState = groupOfPersonsLinkAndExpirationDate.GroupOfPersonsState;
                                 
                                    sendVerificationLink = true;
                                    break;
                                case GroupOfPersonsStateEnum.ACCEPTED:
                                    sendPayLink = true;
                                    responseData.ExpireDate = groupOfPersonsLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "groupOfPersons/PayComplete?id=" + groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر تایید شده و در انتظار دریافت اطلاعات از مرکز می باشد.";
                                    responseData.ResponseState = groupOfPersonsLinkAndExpirationDate.GroupOfPersonsState;
                                    break;
                                case GroupOfPersonsStateEnum.READY_FOR_SEE:
                                case GroupOfPersonsStateEnum.SEEN:
                                    responseData.ExpireDate = groupOfPersonsLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = MainURL + "gop/" + groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.ReportID = groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "groupOfPersons/PayComplete?id=" + groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر قبلا ثبت و اطلاعات ایشان دریافت شده و قابل مشاهده می باشد.";
                                    responseData.ResponseState = groupOfPersonsLinkAndExpirationDate.GroupOfPersonsState;
                                    break;
                            }
                        }
                        else
                        {
                            long GOPVCode = 0;
                            string GOPGUID = "";
                            var result = Engine.Instance.GroupOfPersonsBusiness.AddGroupOfPersons(vcode: ref GOPVCode, cellphone: body.CellPhone, nationalCode: body.NationalCode, userVCode: CurrentUser.VCode, companyNationalId: body.CompanyNationalID, userPaymentTypeVCode: body.UserPaymentTypeEnum, fromApp: false, guid: ref GOPGUID);
                            if (result == GroupOfPersonInsStateEnum.SUCCESSFUL)
                            {
                                groupOfPersonsItem = Engine.Instance.GroupOfPersonsBusiness.GetGroupOfPersons(vcode: GOPVCode);
                                //pay by credit of firm
                                if (groupOfPersonsItem.UserPaymentTypeVCode == (int)UserPaymentTypeEnum.PAY_FROM_CREDIT)
                                {
                                    var PayGroupOfPersonsByCreditState = Engine.Instance.PaymentBusiness.PayGroupOfPersonsByCredit(userVCode : user.VCode, groupOfPersonsVCode : groupOfPersonsItem.VCode);
                                    switch (PayGroupOfPersonsByCreditState)
                                    {
                                        case PayGroupOfPersonsByCreditStateEnum.SUCCESSFUL:
                                            response.State = ResponseStateEnum.SUCCESS;
                                            reportID = groupOfPersonsItem.GUID;
                                            remainCredit = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode).Credit;
                                            var responseData3 = new AddGroupOfPersonsFirmResponse
                                            {
                                                ExpireDate = groupOfPersonsItem.ReportExpirationDate,
                                                ReportUrl = "",
                                                VerifyUrl = MainURL + "groupOfPersons/PayComplete?id=" + reportID,
                                                RemainCredit = remainCredit,
                                                ReportID = Regex.Replace(reportID, "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()),
                                                NationalCode = groupOfPersonsItem.NationalCode,
                                                ResponseMessage = "درخواست شما با موفقیت ثبت شد.",
                                                ResponseState = groupOfPersonsItem.GroupOfPersonsStateVCode
                                            };
                                            responseData = responseData3;
                                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                            if (!user.SelfOtp)
                                            {
                                                sendVerificationLink = true;
                                            }
                                            break;
                                        case PayGroupOfPersonsByCreditStateEnum.CREDIT_IS_NOT_ENOUGH:
                                            response.Errors.Add(((int)(ApiErrorCodeEnum.CREDIT_IS_NOT_ENOUGH)).ToString());
                                            response.Data = new AddGroupOfPersonsFirmResponse()
                                            {
                                                ResponseMessage = "خطا : اعتبار شما برای انجام این درخواست کافی نیست. ابتدا حساب کاربری خود را از طریق پنل حقوقی firmpanel.icescoring.com شارژ نمایید و مجددا اقدام به دریافت گزارش نمایید."
                                            };
                                            break;
                                    }
                                }
                                //pay by customer
                                else
                                {
                                    responseData.ExpireDate = groupOfPersonsItem.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = GOPGUID;
                                    responseData.NationalCode = groupOfPersonsItem.NationalCode;
                                    responseData.VerifyUrl = MainURL + "groupOfPersons/PayIndex?id=" + GOPGUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر ثبت شد و منتظر تایید می باشد.";
                                    responseData.ResponseState = groupOfPersonsItem.GroupOfPersonsStateVCode;
                                    sendPayLink = true;
                                }
                               
                            }
                            else if(result == GroupOfPersonInsStateEnum.CREDIT_IS_NOT_ENOUGH)
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
            if (sendVerificationLink && groupOfPersonsItem != null)
            {
                string msg = "آیس:" + "\n" + (user.Name != null ? user.Name : "") + " درخواست مشاهده گزارش گروه اشخاص شما را دارد." + "\n" + " لینک تایید:" + "\n";
                msg += MainURL + "groupOfPersons/PayComplete?id=" + groupOfPersonsItem.GUID;
                Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: groupOfPersonsItem.Cellphone, message: msg, ip: "", typeVCode: SMSTypeEnum.CONFIRMATION, operatorType: OperatorTypeEnum.GHASEDAK);
            }
            if (sendPayLink && groupOfPersonsItem != null)
            {
                string msg = "آیس:" + "\n" + (user.Name != null ? user.Name : "") + " درخواست مشاهده گزارش گروه اشخاص شما را دارد." + "\n" + " لینک پرداخت:" + "\n";
                msg += MainURL + "groupOfPersons/PayIndex?id=" + groupOfPersonsItem.GUID;
                Engine.Instance.ApiBusiness.SendSms(userVCode: CurrentUser.VCode, cellphone: groupOfPersonsItem.Cellphone, message: msg, ip: "", typeVCode: SMSTypeEnum.CONFIRMATION, operatorType: OperatorTypeEnum.GHASEDAK);
            }
            return ResponseMessage(objResponse);
        }

        [Route("CancelGroupOfPersons")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult CancelGroupOfPersons([FromBody] CancelGroupOfPersonsFirmRequest data)
        {
            UserData objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            GroupOfPersonsData groupOfPersonsItem = new GroupOfPersonsData();
            CryptographyBusiness cryptography = new CryptographyBusiness();
            var response = new ActionResponse<CancelGroupOfPersonsResponse>();
            HttpResponseMessage objResponse = null;
            bool success = false;
            try
            {
                groupOfPersonsItem = Engine.Instance.GroupOfPersonsBusiness.GetGroupOfPersons(guid:data.reportID);
                if (groupOfPersonsItem != null)
                {
                    if (groupOfPersonsItem.ReportExpirationDate >= DateTime.Now)
                    {
                        if (groupOfPersonsItem.GroupOfPersonsStateVCode == GroupOfPersonsStateEnum.SUBMITTED || groupOfPersonsItem.GroupOfPersonsStateVCode == GroupOfPersonsStateEnum.PAID)
                        {
                            if (groupOfPersonsItem.UserPaymentTypeVCode == (int)UserPaymentTypeEnum.PAY_FROM_CREDIT)
                            {
                                success = Engine.Instance.PaymentBusiness.ReturnGroupOfPersonsCredit(groupOfPersonsVCode: groupOfPersonsItem.VCode, userVCode: objCurrentUser.VCode);
                            }
                            else
                            {
                                success = Engine.Instance.GroupOfPersonsBusiness.CancelGroupOfPersons(groupOfPersonsVCode: groupOfPersonsItem.VCode, userVCode: objCurrentUser.VCode, GroupOfPersonsStateVCode: GroupOfPersonsStateEnum.DELETED);
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
                var responseData = new CancelGroupOfPersonsResponse
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

        [Route("GetReadyForSeeGroupOfPersons")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult GetReadyForSeeGroupOfPersons(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<GroupOfPersonsFrontData>>();
            try
            {
                //if (isFirstInitial)
                //{
                //    PersianCalendar objPersianCalendar = new PersianCalendar();
                //    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                //    toDate = DateTime.Now.ToJalali().ToString();
                //}
                List<GroupOfPersonsStateEnum> groupOfPersonsStates = new List<GroupOfPersonsStateEnum>();
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.READY_FOR_SEE);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.SEEN);
                var groupOfPersonsList = Engine.Instance.GroupOfPersonsBusiness.GetGroupOfPersonsList(userVCode: CurrentUser.VCode, groupOfPersonsStates: groupOfPersonsStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate,withouExpired:true);
                var orderdList = groupOfPersonsList.OrderByDescending(o => o.VCode).ToList();
                response.Data = orderdList.Select(t => new GroupOfPersonsFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, GroupOfPersonsState = t.GroupOfPersonsStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "https://www.icescoring.com/" + t.ReportLink, GroupOfPersonsStateName = t.GroupOfPersonsStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList();
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


        [Route("GetWaitingConfirmGroupOfPersons")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult GetWaitingConfirmGroupOfPersons(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<GroupOfPersonsFrontData>>();
            try
            {
                if (isFirstInitial)
                {
                    PersianCalendar objPersianCalendar = new PersianCalendar();
                    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                    toDate = DateTime.Now.ToJalali().ToString();
                }
                List<GroupOfPersonsStateEnum> groupOfPersonsStates = new List<GroupOfPersonsStateEnum>();
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.SUBMITTED);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.PAID);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.ACCEPTED);
                var groupOfPersonsList = Engine.Instance.GroupOfPersonsBusiness.GetGroupOfPersonsList(userVCode: CurrentUser.VCode, groupOfPersonsStates: groupOfPersonsStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate, withouExpired: true);
                var orderdList = groupOfPersonsList.OrderByDescending(o => o.VCode).ToList();
                response.Data = orderdList.Select(t => new GroupOfPersonsFrontData { VCode = t.VCode,Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, GroupOfPersonsState = t.GroupOfPersonsStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink ="",GroupOfPersonsStateName  = t.GroupOfPersonsStateName , UserPaymentTypeEnumName  = t.UserPaymentTypeEnumName }).ToList();
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

        [Route("GetRejectedGroupOfPersons")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult GetRejectedGroupOfPersons(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<GroupOfPersonsFrontData>>();
            try
            {
                if (isFirstInitial)
                {
                    PersianCalendar objPersianCalendar = new PersianCalendar();
                    fromDate = DateTime.Now.AddDays(-2).ToJalali().ToString();
                    toDate = DateTime.Now.ToJalali().ToString();
                }
                List<GroupOfPersonsStateEnum> groupOfPersonsStates = new List<GroupOfPersonsStateEnum>();
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.REJECTED);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.DELETED);
                var groupOfPersonsList = ICE.Business.Engine.Instance.GroupOfPersonsBusiness.GetGroupOfPersonsList(userVCode: CurrentUser.VCode, groupOfPersonsStates: groupOfPersonsStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: fromDate, toDate: toDate);
                var groupOfPersonsList2 = groupOfPersonsList.Where(x => string.IsNullOrEmpty(x.ReportLink));
                groupOfPersonsList.RemoveAll(i => groupOfPersonsList2.Contains(i));
                var orderdList = groupOfPersonsList.OrderByDescending(o => o.VCode).ToList();
                response.Data = orderdList.Select(t => new GroupOfPersonsFrontData { VCode = t.VCode,Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, GroupOfPersonsState = t.GroupOfPersonsStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "", GroupOfPersonsStateName = t.GroupOfPersonsStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList();
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
        [Route("GetArchiveGroupOfPersons")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult GetArchiveGroupOfPersons(bool isFirstInitial = true, string cellphone = null, string nationalCode = null, string fromDate = null, string toDate = null)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<List<GroupOfPersonsFrontData>>();
            try
            {
                List<GroupOfPersonsStateEnum> groupOfPersonsStates = new List<GroupOfPersonsStateEnum>();
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.SUBMITTED);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.PAID);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.ACCEPTED);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.READY_FOR_SEE);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.SEEN);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.DELETED);
                groupOfPersonsStates.Add(GroupOfPersonsStateEnum.REJECTED);
                var groupOfPersonsList = ICE.Business.Engine.Instance.GroupOfPersonsBusiness.GetGroupOfPersonsList(userVCode: CurrentUser.VCode, groupOfPersonsStates: groupOfPersonsStates, cellphone: cellphone, nationalCode: nationalCode, fromDate: Unify(fromDate), toDate: Unify(toDate), withouExpired: false);
                var groupOfPersonsList2 = groupOfPersonsList.Where(x => x.ReportExpirationDate >= DateTime.Now);
                groupOfPersonsList.RemoveAll(i => groupOfPersonsList2.Contains(i));

                response.Data = groupOfPersonsList.Select(t => new GroupOfPersonsFrontData { VCode = t.VCode, Id = t.GUID, EntryDate = t.EntryDate, Cellphone = t.Cellphone, NationalCode = t.NationalCode, CompanyNationalID = t.CompanyNationalID, GroupOfPersonsState = t.GroupOfPersonsStateVCode, UserPaymentTypeName = ((UserPaymentTypeEnum)(t.UserPaymentTypeVCode)).ToString(), UserPaymentTypeEnum = (UserPaymentTypeEnum)t.UserPaymentTypeVCode, ReportLink = "", GroupOfPersonsStateName = t.GroupOfPersonsStateName, UserPaymentTypeEnumName = t.UserPaymentTypeEnumName }).ToList().OrderByDescending(o => o.VCode).ToList(); ;
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

        [Route("AddGroupOfPersonsFromFirmTest")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.GROUP_OF_PERSONS))]
        public IHttpActionResult AddGroupOfPersonsFromFirmTest([FromBody] AddGroupOfPersonsFirmRequestBody body)
        {
            string token = CurrentUser.token;
            string reportID;
            decimal remainCredit = 0;
            GroupOfPersonsData groupOfPersonsItem = new GroupOfPersonsData();
            var response = new ActionResponse<AddGroupOfPersonsFirmResponse>();
            var responseData = new AddGroupOfPersonsFirmResponse();
            var objResponse = new HttpResponseMessage();
            UserData user = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
            try
            {
                var disableEmtaSubmitStr = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("DisableEmtaGraphSubmit");
                bool disableEmtaSubmit = disableEmtaSubmitStr == "1";

                var disableEmtaTestSubmitStr = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("DisableEmtaTestSubmit");
                bool disableEmtaTestSubmit = disableEmtaSubmitStr == "1";

                if (!string.IsNullOrEmpty(token) && !disableEmtaSubmit && !disableEmtaTestSubmit)
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
                        var groupOfPersonsLinkAndExpirationDate = Engine.Instance.GroupOfPersonsBusiness.GetGroupOfPersonsReportLinkAndExpirationDate(cellphone: body.CellPhone, nationalCode: body.NationalCode, UserVCode: CurrentUser.VCode, companyNationalId: body.CompanyNationalID);
                        if (groupOfPersonsLinkAndExpirationDate != null)
                        {
                            switch (groupOfPersonsLinkAndExpirationDate.GroupOfPersonsState)
                            {
                                case GroupOfPersonsStateEnum.PAID:
                                    responseData.ExpireDate = groupOfPersonsLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "groupOfPersons/PayComplete?id=" + groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر قبلا ثبت شده است و منتظر تایید می باشد.";
                                    responseData.ResponseState = groupOfPersonsLinkAndExpirationDate.GroupOfPersonsState;
                                    break;
                                case GroupOfPersonsStateEnum.ACCEPTED:
                                    responseData.ExpireDate = groupOfPersonsLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = "";
                                    responseData.ReportID = groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "groupOfPersons/PayComplete?id=" + groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "رکورد فرد مورد نظر تایید شده ثبت شده و در انتظار دریافت اطلاعات از مرکز می باشد.";
                                    responseData.ResponseState = groupOfPersonsLinkAndExpirationDate.GroupOfPersonsState;
                                    break;
                                case GroupOfPersonsStateEnum.READY_FOR_SEE:
                                case GroupOfPersonsStateEnum.SEEN:
                                    responseData.ExpireDate = groupOfPersonsLinkAndExpirationDate.ReportExpirationDate;
                                    responseData.ReportUrl = MainURL + "gop/" + groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.ReportID = groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.VerifyUrl = MainURL + "groupOfPersons/PayComplete?id=" + groupOfPersonsLinkAndExpirationDate.GUID;
                                    responseData.ResponseMessage = "اطلاعات رکورد فرد مورد نظر دریافت شده و قابل مشاهده می باشد..";
                                    responseData.ResponseState = groupOfPersonsLinkAndExpirationDate.GroupOfPersonsState;
                                    break;
                            }
                        }
                        else
                        {
                            long GOPVCode = 0;
                            string GOPGUID = "";
                            var result = Engine.Instance.GroupOfPersonsBusiness.AddGroupOfPersons(vcode: ref GOPVCode, cellphone: body.CellPhone, nationalCode: body.NationalCode, userVCode: CurrentUser.VCode, companyNationalId: body.CompanyNationalID, userPaymentTypeVCode: body.UserPaymentTypeEnum, fromApp: false, guid: ref GOPGUID);
                            if (result == GroupOfPersonInsStateEnum.SUCCESSFUL)
                            {
                                object responseObj;
                                groupOfPersonsItem = Engine.Instance.GroupOfPersonsBusiness.GetGroupOfPersons(vcode: GOPVCode);
                                Engine.Instance.GroupOfPersonsBusiness.SetGroupOfPersonsGrantCode(groupOfPersonsVCode: groupOfPersonsItem.VCode, grantCode: "123");
                                Engine.Instance.GroupOfPersonsBusiness.SetGroupOfPersonsAccessToken(groupOfPersonsVCode : groupOfPersonsItem.VCode, accessToken: "123");
                                if (groupOfPersonsItem.GroupOfPersonsType == GroupOfPersonsTypeEnum.INDIVIDUAL)
                                {
                                    responseObj = ApiCall.Instance.EmtaGetPersonGroupsHaghighiTest(nationalID: groupOfPersonsItem.NationalCode);
                                }
                                else
                                {
                                    responseObj = ApiCall.Instance.EmtaGetPersonGroupsHoghoghiTest(companyNationalID: groupOfPersonsItem.CompanyNationalID,nationalID:groupOfPersonsItem.NationalCode);
                                }
                                var batchGroupOfPersons = new BatchGroupOfPersonsData() {
                                    GroupOfPersonsVCode = groupOfPersonsItem.VCode,
                                    UserVCode = groupOfPersonsItem.UserVCode,
                                    NationalCode = groupOfPersonsItem.NationalCode,
                                    ResponseJson = responseObj.ToString()
                                };
                                Core.Business.Engine.Instance.BatchGroupOfPersonsBusiness.BatchGroupOfPersonsIns(batchGroupOfPersons);
                                string trackingCode = "";
                                Engine.Instance.GroupOfPersonsBusiness.SetGroupOfPersonsSeen(groupOfPersonsVCode: groupOfPersonsItem.VCode,ref trackingCode);
                            }
                            else
                            {
                                responseData.ExpireDate = DateTime.Now;
                                responseData.NationalCode = body.NationalCode;
                                responseData.ResponseMessage = "خطا : در ثبت اطلاعات مشکلی پیش آمده لطفا اطلاعات خود را بررسی کنید.";
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
