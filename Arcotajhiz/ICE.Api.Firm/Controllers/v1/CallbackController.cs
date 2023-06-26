using ICE.Business;
using ICE.Business.Models;
using ICE.Entities.Enums;
using ICE.Presentation.Common;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ICE.Api.Firm.Controllers.v1
{
    public class CallbackController : Controller
    {
        public bool testEnabled = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("TestEnabled") == "1";
        // GET: Callback
        public ActionResult ICSIndex(string message, string hashCode = null, string reportLink = null)
        {
            if (testEnabled)
            {
                ApiCall.Instance.APILogIns(request: "https://icescoring.com/Callback/ICSIndex?message=" + message + "&hashCode=" + hashCode + "&reportLink=" + reportLink, response: hashCode != null ? hashCode : "hashCode Is Nothing", methodName: "ApiCall/Callback", state: Response.StatusCode.ToString());
                try
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        var responseParams = message.Split('-')[1];
                        string appIcs24HashCode = null;
                        if (!string.IsNullOrEmpty(hashCode))
                            appIcs24HashCode = hashCode;
                        else
                            appIcs24HashCode = message.Split('-')[1];
                        var identification = Business.Engine.Instance.IdentificationBusiness.GetIdentification(appIcs24HashCode: appIcs24HashCode);
                        var callbackResult = ICSResultDescriptionByName(message: message.Split('-')[0]);
                        CryptographyBusiness cryptography = new CryptographyBusiness();
                        var encryptedIdentification = cryptography.Encrypt(identification.VCode.ToString(), EncryptKeyEnum.Key1);
                        if (identification.IdentificationState.VCode == 11 || identification.IdentificationState.VCode == 18 || identification.IdentificationState.VCode == 17 || !string.IsNullOrEmpty(identification.SecondCellphone))
                        {
                            switch (callbackResult.StateCode)
                            {
                                case 8:
                                    {
                                        JObject xmlResult = null;
                                        JObject jsonResult = null;
                                        jsonResult = ApiCall.Instance.NewCBSIcs24GetReportJson(reportLink: reportLink);
                                        if (jsonResult != null)
                                        {
                                            var batch = new ICE.Business.Models.NewBatchData()
                                            {
                                                UserVCode = identification.UserVCode,
                                                Cellphone = identification.Cellphone,
                                                NationalCode = identification.NationalCode,
                                                CompanyNationalID = identification.CompanyNationalID,
                                                IdentificationVCode = identification.VCode,
                                                IdentificationTypeVCode = identification.IdentificationType.VCode
                                            };
                                            var batchResult = new ICE.Business.Models.NewBatchResultData()
                                            {
                                                ResponseXml = xmlResult != null && !string.IsNullOrEmpty(xmlResult["data"]["reportXml"].ToString()) ? xmlResult["data"]["reportXml"].ToString() : null,
                                                ResponseXmlScore = xmlResult != null && !string.IsNullOrEmpty(xmlResult["data"]["scoreXml"].ToString()) ? xmlResult["data"]["scoreXml"].ToString() : null,
                                                ResponseXmlEmpty = xmlResult != null && !string.IsNullOrEmpty(xmlResult["data"]["availabiltyXml"].ToString()) ? xmlResult["data"]["availabiltyXml"].ToString() : null,
                                                ResponseJson = jsonResult != null ? jsonResult.ToString() : null,
                                                HasShahkarIdentified = true,
                                                IsLegalPerson = true
                                            };
                                            long CreditRiskReportVCode = 0;
                                            DateTime ReportDate = DateTime.Now;
                                            string TrackingCode = "";
                                            bool hasShahkarIdentified = true;
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationHasShahkarIdentified(identificationVCode: identification.VCode, hasShahkarIdentified: true);
                                            var shahkarState = Engine.Instance.IdentificationBusiness.VerifyShahkar(identificationVCode: identification.VCode,
                                                                               nationalCode: identification.NationalCode,
                                                                               cellphone: identification.Cellphone,
                                                                               hasShahkarIdentified: hasShahkarIdentified);
                                            Engine.Instance.CreditRiskReportBusiness.AddCreditRiskReport( vcode:ref CreditRiskReportVCode, identificationVCode: identification.VCode,entryDate:ref ReportDate,trackingCode:ref TrackingCode);
                                            if (identification.IdentificationType.VCode == 2)
                                            {
                                                ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationIsLegalPerson(identificationVCode: identification.VCode, isLegalPerson: true);
                                                
                                            }
                                            Engine.Instance.IdentificationBusiness.BatchCompleteIns(batch: batch, batchResult: batchResult, IsNewCoreConnection: true);
                                            UserData identificationUser = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: identification.UserVCode);
                                            if (identification.FromFirmPanel && identificationUser.WebHooks != null && identificationUser.WebHooks.Any())
                                            {
                                                foreach (WebHookData item in identificationUser.WebHooks)
                                                {
                                                    var webHookUrl = item.WebHook;
                                                    var client = new RestClient(string.Format(webHookUrl + "?ReportID={0}&ReportLink=Https://www.icescoring.com{1}&ReportPdf=Https://www.icescoring.com/Report/GetPdf?id={2}&printPage=false", Regex.Replace(WebUtility.UrlEncode(encryptedIdentification), "(%[0-9A-F][0-9A-F])", c => c.Value.ToLower()), identification.ReportLink, StringToURLCustom(encryptedIdentification)));
                                                    var request = new RestRequest(Method.GET);
                                                    client.Execute(request);
                                                    ICE.Business.Engine.Instance.UserBusiness.UserWebHookCallIns(userVcode: identificationUser.VCode, userWebHookVCode: item.VCode, identificationVCode: identification.VCode);
                                                }
                                            }
                                            ICE.Business.Engine.Instance.IdentificationBusiness.SetUserSeenCreditReport(identificationVCode: identification.VCode, userVCode: identification.UserVCode);
                                        }
                                        break;
                                    };

                                case 3:
                                    {
                                        ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationHasShahkarIdentified(identificationVCode: identification.VCode, hasShahkarIdentified: false);
                                        return RedirectToAction("CreditRiskReportFromSite", "Report", new { id = WebUtility.UrlEncode(encryptedIdentification), hasShahkarIdentified = false });
                                    }

                                case 4:
                                    {
                                        ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationHasShahkarIdentified(identificationVCode: identification.VCode, hasShahkarIdentified: true);
                                        ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationIsLegalPerson(identificationVCode: identification.VCode, isLegalPerson: false);
                                        return RedirectToAction("CreditRiskReportFromSite", "Report", new { id = WebUtility.UrlEncode(encryptedIdentification) });
                                    }

                                case 0:
                                    {
                                        break;
                                    }

                                case 2:
                                    {
                                        ICE.Business.Engine.Instance.IdentificationBusiness.SetIdentificationOtpLock(identificationVCode: identification.VCode);
                                        break;
                                    }
                            }
                        }
                        else if (identification.IdentificationState.VCode == 4)
                        {
                            Response.Redirect(identification.ReportLink);
                            return null/* TODO Change to default(_) if this is not a reference type */;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return View();
        }
        public static string StringToURLCustom(string text)
        {
            return text.Replace("/", "(S)").Replace("&", "(A)").Replace("+", "(P)").Replace("=", "(E)");
        }
        private ICSCallbackMessageState ICSResultDescriptionByName(string message)
        {
            var objICSCallbackMessageState = new ICSCallbackMessageState();
            switch (message)
            {
                case "OTPAccepted":
                    {
                        objICSCallbackMessageState.Message = "OTP تایید شد";
                        objICSCallbackMessageState.StateCode = 1;
                        break;
                    }

                case "OTPLocked":
                    {
                        objICSCallbackMessageState.Message = "OTP قفل شد و کاربر نمیتواند کد جدید دریافت کند.";
                        objICSCallbackMessageState.StateCode = 2;
                        break;
                    }

                case "ShahkarFailed":
                    {
                        objICSCallbackMessageState.Message = "شماره ملی با شماره موبایل تطابق ندارد.";
                        objICSCallbackMessageState.StateCode = 3;
                        break;
                    }

                case "SherkathaFailed":
                    {
                        objICSCallbackMessageState.Message = "شناسه ملی و شماره ملی با هم تطابق ندارد.";
                        objICSCallbackMessageState.StateCode = 4;
                        break;
                    }

                case "ShahkarError":
                    {
                        objICSCallbackMessageState.Message = "خطای دریافت شاهکار";
                        objICSCallbackMessageState.StateCode = 5;
                        break;
                    }

                case "SherkathaError":
                    {
                        objICSCallbackMessageState.Message = "خطای سامانه بررسی شناسه ملی";
                        objICSCallbackMessageState.StateCode = 6;
                        break;
                    }

                case "IdentityCheckAccepted":
                    {
                        objICSCallbackMessageState.Message = "احراز هویت کاربر با موفقیت انجام شد.";
                        objICSCallbackMessageState.StateCode = 7;
                        break;
                    }

                case "ReportGenerated":
                    {
                        objICSCallbackMessageState.Message = "گزارش ایجاد شد";
                        objICSCallbackMessageState.StateCode = 8;
                        break;
                    }

                case "GenerateReportFailed":
                    {
                        objICSCallbackMessageState.Message = "خطا در دریافت گزارش";
                        objICSCallbackMessageState.StateCode = 9;
                        break;
                    }

                case "ReportDataLost":
                    {
                        objICSCallbackMessageState.Message = "گزارش ایجاد شده اما ذخیره سازی با خطا مواجه شده.";
                        objICSCallbackMessageState.StateCode = 10;
                        break;
                    }

                case "ReportShared":
                    {
                        objICSCallbackMessageState.Message = "گزارش از طرف کاربر با سرویس گیرنده به اشتراک گذاشته شد.";
                        objICSCallbackMessageState.StateCode = 11;
                        break;
                    }

                default:
                    {
                        objICSCallbackMessageState.Message = "";
                        objICSCallbackMessageState.StateCode = 0;
                        break;
                    }
            }
            return objICSCallbackMessageState;
        }
        public class ICSCallbackMessageState
        {
            public int StateCode { get; set; }
            public string Message { get; set; }
        }
    }
}