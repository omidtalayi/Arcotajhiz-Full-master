using ICE.Api.Common.Models;
using ICE.Api.Firm.Filters;
using ICE.Api.Firm.ir.shaparak.bpm;
using ICE.Api.Firm.ir.shaparak.sep;
using ICE.Api.Firm.Models.Request.v1;
using ICE.Business;
using ICE.Entities.Enums;
using ICE.Presentation.Common;
using ICE.Presentation.Common.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace ICE.Api.Firm.Controllers.v1
{
    [RoutePrefix("api/v1/credit")]

    public class V1_CreditController : BaseApiPresentationController
    {
        [Route("GetUserCredits")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetUserCredits([FromBody] GetUserCreditsRequestBody body)
        {
            //[EnableCors(origins: "https://firmpanel.icescoring.com/", headers: "*", methods: "*")]
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            try
            {
                int rowCount = 0;
                var creditList = Engine.Instance.PaymentBusiness.GetUserCreditDetail(userVCode: CurrentUser.VCode,rowCount: ref rowCount,pageNo  :body.pageNo,pageSize:body.pageSize);
                var objCurrentUser = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                var jsonStr = new JavaScriptSerializer().Serialize(new { CreditList = creditList, UserCredit = objCurrentUser.Credit ,rowCount  = rowCount});
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

        [Route("CreditCharge")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult CreditCharge([FromBody] CreditChargeRequest body)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            string strResponse = "";
            try
            {
                bool payResult = false;
                decimal? paymentAmount = body.Amount;
                long? onlinePaymentId = 0;
                string jDate = "";
                string jTime = "";
                string paymentAmounts = "";
                string ip = GetIp();
                var result = Engine.Instance.PaymentBusiness.PayIdentification(ip: ip, PortalPaymentTypeVCode: body.PortalPaymentTypeVCode, identificationVCode: null, bankPortalVCode: GetBankPortal(body.BankVCode), onlinePaymentId: ref onlinePaymentId, jDate: ref jDate, jTime: ref jTime, paymentAmount: ref paymentAmount, null, payFromApp: false, paymentAmounts: ref paymentAmounts, userVCode: CurrentUser.VCode, requestRegistrationVCode: body.RequestRegistrationVCode);
                switch (result)
                {
                    case PayIdentificationStateEnum.SUCCESSFUL:
                        strResponse = LocalPay(FromApp: false, OnlinePaymentID: (long)onlinePaymentId, IdentificationVCode: 0, BankVCode: body.BankVCode, paymentAmount: (decimal)paymentAmount, paymentAmounts: paymentAmounts, jDate: jDate, jTime: jTime, PayResult: ref payResult);
                        response.Data = strResponse;
                        response.State = Common.Enums.ResponseStateEnum.SUCCESS;
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        break;
                    default:
                        var errorEnumCode = "Pay-Identification-unknown-error";
                        response.State = Common.Enums.ResponseStateEnum.FAILED;
                        response.Errors.Add(errorEnumCode);
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                        break;
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

        [Route("GetCreditDetails")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetCreditDetails([FromBody] CreditDetailsRequest body)
        {
            HttpResponseMessage objResponse = null;
            var response = new ActionResponse<string>();
            string strResponse = "";
            try
            {
                var result = ICE.Business.Engine.Instance.PaymentBusiness.GetCreditDetails(body.accountingVCode);
                if (result != null)
                {
                    var currentUserDetails = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                    var jsonStr = new JavaScriptSerializer().Serialize(new { CreditDetails = result, userCompanyName = currentUserDetails.Name });
                    response.Data = jsonStr;
                    response.State = Common.Enums.ResponseStateEnum.SUCCESS;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.Data = null;
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
        private string LocalPay(bool FromApp, long OnlinePaymentID, long IdentificationVCode, BankEnum BankVCode, Decimal paymentAmount, string paymentAmounts, string jDate, string jTime, ref bool PayResult, string SaleRefID = "")
        {
            try
            {
                var objSetting = Engine.Instance.PaymentBusiness.GetSetting(bankPortal: GetBankPortal(BankVCode));
                switch (BankVCode)
                {
                    case BankEnum.SAMAN:
                        {
                            var callbackUrl = "https://www.icescoring.com/" + objSetting["CallbackUrl"].ToString(); //"https://www.Icescoring.com/" + objSetting["CallbackUrl"].ToString();
                            var sepTxn = new SepTxnData();
                            sepTxn.Action = objSetting["PostUrl"].ToString();
                            sepTxn.Amount = paymentAmount;
                            sepTxn.CellNumber = 0;
                            sepTxn.RedirectUrl = callbackUrl;
                            sepTxn.TerminalId = objSetting["TerminalId"].ToString();
                            sepTxn.ResNum = OnlinePaymentID.ToString();
                            var Result = ApiCall.Instance.GetTokenFromSep(sepTxn, null);
                            if (!string.IsNullOrEmpty(Result))
                            {
                                var sb = new StringBuilder();
                                sb.Append("<html>");
                                sb.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
                                sb.AppendFormat("<form name='form' action='{0}' method='post'>", "https://sep.shaparak.ir/OnlinePG/OnlinePG"); //objSetting["PostUrl"]);
                                sb.AppendFormat("<input type='hidden' id='Token' name='Token' value='{0}'/>", Result);
                                sb.Append("<input type='text' name='GetMethod' value=''/>");
                                sb.Append("</form>");
                                sb.Append("</body>");
                                sb.Append("</html>");
                                PayResult = true;
                                return sb.ToString();
                            }
                            break;
                        }
                    case BankEnum.MELLAT:
                        {
                            string result;
                            result = PayBehpardakht(OnlinePaymentID, (long)paymentAmount, jDate, jTime, long.Parse(objSetting["TerminalId"].ToString()), objSetting["Username"].ToString(), objSetting["Password"].ToString(), GetCallBackUrl(Setting: objSetting, FromApp: false), paymentAmounts);
                            if ((result != "0"))
                            {
                                var s = result.Split(',');
                                if (s[0] == "0")
                                {
                                    var sb = new StringBuilder();
                                    sb.Append("<html>");
                                    sb.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
                                    sb.AppendFormat("<form name='form' action='{0}' method='post'>", objSetting["PostUrl"]);
                                    sb.AppendFormat("<input type='hidden' name='RefId' value='{0}'>", s[1]);
                                    sb.Append("</form>");
                                    sb.Append("</body>");
                                    sb.Append("</html>");
                                    PayResult = true;
                                    return sb.ToString();
                                }
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        private string GetCallBackUrl(Hashtable Setting, bool FromApp, bool fromFirmPanel = false)
        {
            string queryString = "";
            if (FromApp)
            {
                if (fromFirmPanel)
                    queryString = "?FromApp=True&fromFirmPanel=True";
                else
                    queryString = "?FromApp=True";
            }
            else if (fromFirmPanel)
                queryString = "?fromFirmPanel=True";
            return "https://www.icescoring.com/" + Setting["CallbackUrl"].ToString() + queryString;
        }
        public static BankPortalEnum GetBankPortal(BankEnum BankVCode)
        {
            switch (BankVCode)
            {
                case BankEnum.SAMAN:
                    {
                        return BankPortalEnum.SAMAN_PARDAKHT_KISH;
                    }

                case BankEnum.MELLAT:
                    {
                        return BankPortalEnum.BEH_PARDAKHT_MELLAT;
                    }
            }
            return BankPortalEnum.SAMAN_PARDAKHT_KISH;
        }
        public string GetTokenFromSep(SepTxnData txn)
        {
            string token = null;
            try
            {
                var restClient = new RestClient("https://sep.shaparak.ir/onlinepg/onlinepg");
                var restRequest = new RestRequest(Method.POST)
                {
                    RequestFormat = DataFormat.Json,
                    OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
                };
                txn.Action = "token";
                restRequest.AddBody(txn);
                var sepResult = restClient.Execute(restRequest);
                var jsonSerlializer = new JavaScriptSerializer();
                var sepPgToken = (IDictionary<string, object>)
                jsonSerlializer.DeserializeObject(sepResult.Content);
                var jobj = Json(sepPgToken);
                General.LogError(new Exception(sepResult.StatusCode + sepResult.Content), Request);
                object statusObj;
                int status;
                sepPgToken.TryGetValue("status", out statusObj);
                if (statusObj != null && int.TryParse(statusObj.ToString(), out status))
                {
                    if (status == 1)
                    {
                        object objToken;
                        sepPgToken.TryGetValue("token", out objToken);
                        token = objToken.ToString();
                    }
                    else
                    {
                        General.LogError(new Exception(sepPgToken["errorDesc"].ToString()), Request);
                    }
                }
                else
                {
                    General.LogError(new Exception(sepPgToken["errorDesc"].ToString()), Request);
                }
            }
            catch (Exception ex)
            {
                General.LogError(ex, Request);
            }
            return token;
        }
        //public string GetTokenFromSep(SepTxnData txn)
        //{
        //    string token = null;
        //    try
        //    {
        //        var sepMultiInit = new PaymentIFBinding();
        //        token = sepMultiInit.RequestMultiSettleTypeToken(TermID: txn.TerminalId, ResNum: txn.ResNum, Amounts: txn.Amounts, TotalAmount: (long)txn.Amount, SegAmount1: 0, SegAmount2: 0, SegAmount3: 0, SegAmount4: 0, SegAmount5: 0, SegAmount6: 0, AdditionalData1: "", AdditionalData2: "", Wage: 0, RedirectUrl: txn.RedirectUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        General.LogError(ex, Request);
        //    }
        //    return token;
        //}
        private string PayBehpardakht(long orderID, long amount, string localDate, string localTime, long TerminalId, string Username, string Password, string CallbackUrl, string paymentAmounts)
        {
            string result;
            try
            {
                var bpService = new PaymentGatewayImplService();
                var amounts = paymentAmounts.Split(',');
                if (int.Parse(amounts[1]) > 0)
                    result = bpService.bpCumulativeDynamicPayRequest(terminalId: TerminalId, userName: Username, userPassword: Password, orderId: orderID, amount: amount, localDate: localDate, localTime: localTime, additionalData: "1," + amounts[0] + ",0;2," + amounts[1] + ",0", callBackUrl: CallbackUrl);
                else
                    result = bpService.bpCumulativeDynamicPayRequest(terminalId: TerminalId, userName: Username, userPassword: Password, orderId: orderID, amount: amount, localDate: localDate, localTime: localTime, additionalData: "1," + amounts[0] + ",0", callBackUrl: CallbackUrl);
            }
            catch (Exception ex)
            {
                General.LogError(ex, Request);
                result = "-1";
            }
            return result;
        }

    }
}