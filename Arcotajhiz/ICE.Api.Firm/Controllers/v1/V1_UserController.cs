using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ICE.Api.Common.Models;
using ICE.Api.Firm.Filters;
using ICE.Api.Firm.Models.Request.v1;
using ICE.Api.Firm.Models.Response;
using ICE.Api.Firm.Modules;
using ICE.Business;
using ICE.Business.Models;
using ICE.Entities.Enums;
using ICE.Presentation.Common;
using System.Linq;
using ResponseStateEnum = ICE.Api.Common.Enums.ResponseStateEnum;

namespace ICE.Api.Firm.Controllers.v1
{
    [RoutePrefix("api/v1/user")]
    public class V1_UserController : BaseApiPresentationController
    {
        // GET: User
        [Route("authenticate")]
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] AuthenticationInputRequest input)
        {
            var response = new ActionResponse<LoginResponse>();
            HttpResponseMessage objResponse;
            try
            {
                var ip = HttpContext.Current.Request.UserHostAddress;
                var loginData = Engine.Instance.UserBusiness.Login(username: input.UserName, password: input.Password, deviceId: input.DeviceId, IP: ip);

                if (loginData.LoginState == LoginStateEnum.SUCCESSFUL)
                {
                    string token = "";
                    string tokenCode = "";
                    DateTime tokenExpireTime = DateTime.MinValue;
                    if (loginData.User.Token != null && loginData.User.TokenExpirationDate >= DateTime.Now)
                    {
                        token = loginData.User.Token;
                        tokenExpireTime = loginData.User.TokenExpirationDate;
                    }
                    else
                    {
                        var objJwt = new JwtManager();
                        var objUserSession = objJwt.GenerateToken(username: input.UserName, userVCode: loginData.User.VCode,
                            expireMinutes: loginData.User.TokenExpirationTime, apis: loginData.User.APIs);
                        var objUser = new UserData();
                        objUser.Username = input.UserName;
                        objUser.Token = objUserSession.token;
                        objUser.SecretCode = objUserSession.TokenCode;
                        objUser.TokenExpirationDate = objUserSession.TokenExpireTime;
                        objUser.DeviceId = input.DeviceId;
                        Engine.Instance.UserBusiness.UserTokenIns(ref objUser);
                    }
                    response.State = ResponseStateEnum.SUCCESS;
                    response.Data = new LoginResponse();
                    response.Data.token = token;
                    response.Data.userName = loginData.User.Username;
                    response.Data.tokenExpireDate = loginData.User.TokenExpirationDate;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(ApiErrorCodeEnum.USERNAME_OR_PASSWORD_IS_INVALID.ToString());
                    objResponse = Request.CreateResponse(HttpStatusCode.Forbidden, response);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/Authenticate");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }
        [Route("authenticateHash")]
        [HttpPost]
        public IHttpActionResult authenticateHash([FromBody] AuthenticationInputRequest input)
        {
            var response = new ActionResponse<LoginResponse>();
            HttpResponseMessage objResponse;
            try
            {
                LoginData loginData = null;
                if (HttpContext.Current.Request.UserHostAddress == "79.175.172.101" || HttpContext.Current.Request.UserHostAddress == "92.119.70.154" || HttpContext.Current.Request.UserHostAddress == "91.92.215.157" || HttpContext.Current.Request.UserHostAddress == "::1")
                {
                    loginData = Engine.Instance.UserBusiness.Login(username: input.UserName, hashedPassword: input.Password, deviceId: input.DeviceId);
                    if (loginData.LoginState == LoginStateEnum.SUCCESSFUL)
                    {
                        string token = "";
                        string tokenCode = "";
                        DateTime tokenExpireTime = DateTime.MinValue;
                        if (loginData.User.Token != null && loginData.User.TokenExpirationDate >= DateTime.Now)
                        {
                            token = loginData.User.Token;
                            tokenExpireTime = loginData.User.TokenExpirationDate;
                        }
                        else
                        {
                            var objJwt = new JwtManager();
                            var objUserSession = objJwt.GenerateToken(username: input.UserName, userVCode: loginData.User.VCode,
                                expireMinutes: loginData.User.TokenExpirationTime, apis: loginData.User.APIs);
                            var objUser = new UserData();
                            objUser.Username = input.UserName;
                            objUser.Token = objUserSession.token;
                            objUser.SecretCode = objUserSession.TokenCode;
                            objUser.TokenExpirationDate = objUserSession.TokenExpireTime;
                            Engine.Instance.UserBusiness.UserTokenIns(ref objUser);
                        }
                        response.State = ResponseStateEnum.SUCCESS;
                        response.Data = new LoginResponse();
                        response.Data.token = token;
                        response.Data.userName = loginData.User.Username;
                        response.Data.tokenExpireDate = loginData.User.TokenExpirationDate;
                        objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else
                    {
                        response.State = ResponseStateEnum.FAILED;
                        response.Errors.Add(ApiErrorCodeEnum.USERNAME_OR_PASSWORD_IS_INVALID.ToString());
                        objResponse = Request.CreateResponse(HttpStatusCode.Forbidden, response);
                    }
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                    objResponse = Request.CreateResponse(HttpStatusCode.Forbidden, response);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/authenticateHash");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("SubmitFirmUser")]
        [HttpPost]
        public IHttpActionResult SubmitFirmUser([FromBody] FirmUserData input)
        {
            var response = new ActionResponse<SubmitFirmUserResponse>();
            HttpResponseMessage objResponse = null;
            CryptographyBusiness cryptography = new CryptographyBusiness();
            try
            {
                if (!string.IsNullOrEmpty(input.presenterCode))
                {
                    if (!Engine.Instance.FirmBusiness.isPresenterExists(presenterCode: input.presenterCode))
                    {
                        response.State = ResponseStateEnum.FAILED;
                        response.Errors.Add("شماره موبایل / کد معرف وارد شده صحیح نیست.");
                        objResponse = Request.CreateResponse(HttpStatusCode.Forbidden, response);
                    }
                }
                long VCode = 0;
                string cellPhoneVerifyCode = null;
                var objFirmRegisterationData = new ICE.Business.Models.FirmRegistrationData();
                objFirmRegisterationData.Address = input.address;
                objFirmRegisterationData.ContactPointCellphone = input.cellphone;
                objFirmRegisterationData.ContactPointFamilyName = input.lastName;
                objFirmRegisterationData.ContactPointName = input.firstName;
                objFirmRegisterationData.Email = input.email;
                objFirmRegisterationData.FirmName = input.companyName;
                objFirmRegisterationData.LetterFileName = input.letterFileName;
                objFirmRegisterationData.OfficialPaperFileName = input.officialPaperFileName;
                objFirmRegisterationData.LocationVCode = input.city;
                objFirmRegisterationData.PostalCode = input.postalCode;
                objFirmRegisterationData.RegisterDate = input.registerDate > 0 ? input.registerDate.ToString().Substring(0, 4) + input.registerDate.ToString().Substring(4, 2) + input.registerDate.ToString().Substring(6, 2) : null;
                objFirmRegisterationData.RegistrationNo = input.registrationNo;
                objFirmRegisterationData.RegisteredIdentificationNo = input.registeredIdentificationNo;
                objFirmRegisterationData.Telephone = input.tel;
                objFirmRegisterationData.PresenterCode = input.presenterCode;
                var result = Engine.Instance.FirmBusiness.FirmRegistrationIns(ref VCode, objFirmRegisterationData, ref cellPhoneVerifyCode);
                switch (result)
                {
                    case FirmRegistrationStateEnum.SUCCESSFUL:
                        {
                            var returnMessage = "اطلاعات شما با موفقیت ثبت شد لطفا شماره همراه خود ";
                            var encryptedVCode = cryptography.Encrypt(VCode.ToString(), EncryptKeyEnum.Key1);
                            if (!string.IsNullOrEmpty(input.email))
                            {
                                var encryptedEmail = cryptography.Encrypt(input.email, EncryptKeyEnum.Key1);
                                string verifyLink = "https://icescoring.com/Membership/VerifyFirmEmail?token=" + WebUtility.UrlEncode(encryptedVCode) + "&token2=" + WebUtility.UrlEncode(encryptedEmail);
                                SmtpProvider.Instance.SendEmail(email: input.email, subject: "لینک فعالسازی ایمیل اکانت", message: "برای فعالسازی ایمیل خود بر روی لینک زیر کلیک نمایید :‌ " + "/" + "\n" + "<a href='" + verifyLink + "'>" + verifyLink + "</a>");
                                returnMessage += "و ایمیل ";
                            }
                            string smsMessage = string.Empty;
                            var onlyCallActiveStr = General.GetSetting(subSystem: SubSystemEnum.WEBSITE, key: "onlyCallActive");
                            var onlyCallActive = onlyCallActiveStr == "1";
                            smsMessage += "کد تایید آیس : " + cellPhoneVerifyCode;
                            if (onlyCallActive)
                            {
                                Task.Run(() =>
                                {
                                    Engine.Instance.ApiBusiness.MakePhoneCall(userVCode: 0, cellphone: input.cellphone, message: smsMessage, ip: "");
                                });
                            }
                            else
                            {
                                Task.Run(() =>
                                {
                                    Engine.Instance.ApiBusiness.SendSms(userVCode: 0, cellphone: input.cellphone, message: smsMessage, ip: "", typeVCode: SMSTypeEnum.OTP);
                                });
                            }
                            returnMessage += " را تایید نمایید";

                            response.State = ResponseStateEnum.SUCCESS;
                            response.Data = new SubmitFirmUserResponse();
                            response.Data.firmCode = WebUtility.UrlEncode(encryptedVCode);
                            response.Data.cellphone = input.cellphone;
                            response.Data.message = returnMessage;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);

                            break;
                        }

                    case FirmRegistrationStateEnum.CELLPHONE_CONTACT_POINT_IS_DUPLICATE:
                        {
                            response.State = ResponseStateEnum.FAILED;
                            response.Errors.Add("شماره تلفن همراه وارد شده قبلا ثبت شده است.");
                            response.Data = new SubmitFirmUserResponse();
                            response.Data.firmRegistrationState = FirmRegistrationStateEnum.CELLPHONE_CONTACT_POINT_IS_DUPLICATE;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                            break;
                        }

                    case FirmRegistrationStateEnum.EMAIL_IS_DUPLICATE:
                        {
                            response.State = ResponseStateEnum.FAILED;
                            response.Errors.Add("ایمیل وارد شده قبلا ثبت شده است.");
                            response.Data = new SubmitFirmUserResponse();
                            response.Data.firmRegistrationState = FirmRegistrationStateEnum.EMAIL_IS_DUPLICATE;
                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/SubmitFirmUser");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("VerifyCellPhone")]
        [HttpPost]
        public IHttpActionResult VerifyCellPhone([FromBody] VerifyCellPhoneData input)
        {
            var response = new ActionResponse<VerifyCellPhoneResponse>();
            HttpResponseMessage objResponse = null;
            CryptographyBusiness cryptography = new CryptographyBusiness();
            try
            {
                var firmUserRegistrationVCode = cryptography.Decrypt(WebUtility.UrlDecode(input.firmCode), EncryptKeyEnum.Key1);
                string message = "کد وارد  شده صحیح نیست.";
                var success = false;
                var result = Engine.Instance.FirmBusiness.VerifyContactPointCellphone(firmRegistrationVCode: long.Parse(firmUserRegistrationVCode), contactPointCellphone: input.cellPhone, contactPointCellphoneVerificationCode: input.verifyCode);
                success = result.VerificationState == VerificationStateEnum.SUCCESSFUL;
                message = result.Description;
                if (result.VerificationState == VerificationStateEnum.SUCCESSFUL)
                {
                    response.State = ResponseStateEnum.SUCCESS;
                    response.Data = new VerifyCellPhoneResponse();
                    response.Data.cellphone = input.cellPhone;
                    response.Data.message = "";
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Data = new VerifyCellPhoneResponse();
                    response.Data.cellphone = input.cellPhone;
                    response.Data.message = message;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/VerifyCellPhone");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }


        [Route("CreateUserForFirm")]
        [HttpPost]
        public IHttpActionResult CreateUserForFirm([FromBody] CreateUserForFirmData input)
        {
            var response = new ActionResponse<VerifyCellPhoneResponse>();
            HttpResponseMessage objResponse = null;
            CryptographyBusiness cryptography = new CryptographyBusiness();
            var success = false;
            var message = "خطا لطفا مجددا تلاش نماید.";
            string loginResult = string.Empty;
            try
            {
                var firmUserRegistrationVCode = cryptography.Decrypt(WebUtility.UrlDecode(input.firmCode), EncryptKeyEnum.Key1);
                var firmRegistrationData = ICE.Business.Engine.Instance.FirmBusiness.GetFirmRegistration(vcode: long.Parse(firmUserRegistrationVCode));
                if (firmRegistrationData != null)
                {
                    var objUser = new ICE.Business.Models.UserData();
                    objUser.CellPhone = firmRegistrationData.ContactPointCellphone;
                    objUser.FirmRegistrationVCode = firmRegistrationData.VCode;
                    objUser.Name = "شماره موبایل : " + firmRegistrationData.ContactPointCellphone;
                    objUser.Password = input.password;
                    objUser.Username = input.username;
                    objUser.ip = GetIp();
                    var result = ICE.Business.Engine.Instance.UserBusiness.CreateUser(user: objUser, password: input.password);
                    if (result != null)
                    {
                        switch (result.UserRegisterationState)
                        {
                            case UserRegistrationStateEnum.SUCCESSFUL:
                                {
                                    var userResult = Engine.Instance.UserBusiness.Login(username: input.username, password: input.password);
                                    if (userResult != null)
                                    {
                                        loginResult = userResult.LoginState.ToString();
                                        if (userResult.LoginState == LoginStateEnum.SUCCESSFUL)
                                        {
                                            var User = userResult.User;
                                            var objJwt = new JwtManager();
                                            var objUserSession = objJwt.GenerateToken(username: userResult.User.Username, userVCode: userResult.User.VCode,
                                                expireMinutes: userResult.User.TokenExpirationTime, apis: userResult.User.APIs);
                                            objUser = new UserData();
                                            objUser.Username = input.username;
                                            objUser.Token = objUserSession.token;
                                            objUser.SecretCode = objUserSession.TokenCode;
                                            objUser.TokenExpirationDate = objUserSession.TokenExpireTime;
                                            Engine.Instance.UserBusiness.UserTokenIns(ref objUser);
                                            response.State = ResponseStateEnum.SUCCESS;
                                            response.Data = new VerifyCellPhoneResponse();
                                            response.Data.token = objUserSession.token;
                                            response.Data.username = objUserSession.UserName;
                                            response.Data.tokenExpireDate = userResult.User.TokenExpirationDate;
                                            objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                            message = "OK";
                                        }
                                    }
                                    break;
                                }
                            case UserRegistrationStateEnum.DUPLICATECELLPHONE:
                                {
                                    message = "شماره تلفن قبلا در سیستم ثبت شده است.";
                                    response.State = ResponseStateEnum.FAILED;
                                    response.Data = new VerifyCellPhoneResponse();
                                    response.Data.message = message;
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                    break;
                                }
                            case UserRegistrationStateEnum.DUPLICATEUSERNAME:
                                {
                                    message = "نام کاربری قبلا در سیستم ثبت شده است.";
                                    response.State = ResponseStateEnum.FAILED;
                                    response.Data = new VerifyCellPhoneResponse();
                                    response.Data.message = message;
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                    break;
                                }
                            case UserRegistrationStateEnum.DUPLICATEEMAIL:
                                {
                                    message = "ایمیل قبلا در سیستم ثبت شده است.";
                                    response.State = ResponseStateEnum.FAILED;
                                    response.Data = new VerifyCellPhoneResponse();
                                    response.Data.message = message;
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                    break;
                                }
                            case UserRegistrationStateEnum.WEAKPASSWORD:
                                {
                                    message = "رمز عبور نباید کمتر از 6 کاراکتر باشد.";
                                    response.State = ResponseStateEnum.FAILED;
                                    response.Data = new VerifyCellPhoneResponse();
                                    response.Data.message = message;
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                    break;
                                }
                            case UserRegistrationStateEnum.WEAKUSERNAME:
                                {
                                    message = "نام کاربری نباید کمتر از 5 کاراکتر باشد.";
                                    response.State = ResponseStateEnum.FAILED;
                                    response.Data = new VerifyCellPhoneResponse();
                                    response.Data.message = message;
                                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    message = "لطفا اطلاعات خود را برای ثبت نام وارد نمایید.";
                }
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/CreateUserForFirm");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("ResendVerifyCode")]
        [HttpPost]
        public IHttpActionResult ResendVerifyCode([FromBody] ResendVerifyCodeData input)
        {
            var response = new ActionResponse<ResendVerifyCodeResponse>();
            HttpResponseMessage objResponse = null;
            CryptographyBusiness cryptography = new CryptographyBusiness();
            try
            {
                var firmUserRegistrationVCode = long.Parse(cryptography.Decrypt(WebUtility.UrlDecode(input.firmCode), EncryptKeyEnum.Key1));
                var firmUserRegistration = ICE.Business.Engine.Instance.FirmBusiness.GetFirmRegistration(firmUserRegistrationVCode);
                if (firmUserRegistration != null && firmUserRegistration.ContactPointCellphone != null && firmUserRegistration.VerificationCodeExpirationDate > DateTime.Now)
                {
                    var getNewVerifyCode = ICE.Business.FirmBusiness.ResendContactPointCellphoneVerificationCode(firmRegistrationVCode: firmUserRegistration.VCode);
                    string phoneNumber = firmUserRegistration.ContactPointCellphone.ToString();
                    string msg = string.Empty;
                    msg += "کد تایید آیس : " + getNewVerifyCode.ToString();
                    var onlyCallActive = General.GetSetting(subSystem: SubSystemEnum.WEBSITE, key: "onlyCallActive") == "1";
                    if (!string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(msg))
                    {
                        if (onlyCallActive)
                            ICE.Business.Engine.Instance.ApiBusiness.MakePhoneCall(userVCode: 0, cellphone: phoneNumber, message: msg, ip: "");
                        else
                            ICE.Business.Engine.Instance.ApiBusiness.SendSms(userVCode: 0, cellphone: phoneNumber, message: msg, ip: "", typeVCode: SMSTypeEnum.OTP);
                    }
                    response.State = ResponseStateEnum.SUCCESS;
                    response.Data = new ResendVerifyCodeResponse();
                    response.Data.message = "ok";
                    response.Data.cellphone = firmUserRegistration.ContactPointCellphone;
                    response.Data.firmCode = input.firmCode;
                    response.Data.tokenExpireDate = DateTime.Now.AddMinutes(3);
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ResendVerifyCode");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("FirmRegistrationState")]
        [HttpPost]
        public IHttpActionResult FirmRegistrationState([FromBody] FirmRegistrationStateData input)
        {
            var response = new ActionResponse<ResendVerifyCodeResponse>();
            HttpResponseMessage objResponse = null;
            CryptographyBusiness cryptography = new CryptographyBusiness();
            try
            {
                var firmUserRegistrationVCode = long.Parse(cryptography.Decrypt(WebUtility.UrlDecode(input.firmCode), EncryptKeyEnum.Key1));
                var firmUserRegistration = ICE.Business.Engine.Instance.FirmBusiness.GetFirmRegistration(firmUserRegistrationVCode);
                if (firmUserRegistration != null && firmUserRegistration.ContactPointCellphone != null && firmUserRegistration.VerificationCodeExpirationDate > DateTime.Now)
                {
                    var getNewVerifyCode = ICE.Business.FirmBusiness.ResendContactPointCellphoneVerificationCode(firmRegistrationVCode: firmUserRegistration.VCode);
                    string phoneNumber = firmUserRegistration.ContactPointCellphone.ToString();
                    string msg = string.Empty;
                    msg += "کد تایید آیس : " + getNewVerifyCode.ToString();
                    var onlyCallActive = System.Convert.ToBoolean(General.GetSetting(subSystem: SubSystemEnum.WEBSITE, key: "onlyCallActive"));
                    if (!string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(msg))
                    {
                        if (onlyCallActive)
                            ICE.Business.Engine.Instance.ApiBusiness.MakePhoneCall(userVCode: 0, cellphone: phoneNumber, message: msg, ip: "");
                        else
                            ICE.Business.Engine.Instance.ApiBusiness.SendSms(userVCode: 0, cellphone: phoneNumber, message: msg, ip: "", typeVCode: SMSTypeEnum.OTP);
                    }
                    response.State = ResponseStateEnum.SUCCESS;
                    response.Data = new ResendVerifyCodeResponse();
                    response.Data.message = "ok";
                    response.Data.cellphone = firmUserRegistration.ContactPointCellphone;
                    response.Data.firmCode = input.firmCode;
                    response.Data.tokenExpireDate = DateTime.Now.AddMinutes(3);
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ResendVerifyCode");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            return ResponseMessage(objResponse);
        }

        [Route("ChangePassword")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult ChangePassword([FromBody] ChangePasswordFirmRequest input)
        {
            var response = new ActionResponse<string>();
            HttpResponseMessage objResponse = null;
            string message = "";
            bool success = false;
            try
            {
                if ((input.newpassword ?? "") == (input.repeatnewPassword ?? ""))
                {
                    var user = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                    UserUpdateData result = ICE.Business.Engine.Instance.UserBusiness.ChangePassword(user, input.oldPassword, input.newpassword);
                    switch (result.State)
                    {
                        case UserUpdateStateEnum.SUCCESSFUL:
                            {
                                success = true;
                                message = "با موفقیت انجام شد.";
                                break;
                            }
                        case UserUpdateStateEnum.LOGINFAILED:
                            {
                                message = "رمز عبور قبلی شما صحیح نیست.";
                                break;
                            }
                        case UserUpdateStateEnum.WEAKPASSWORD:
                            {
                                message = "رمز عبور شما ساده میباشد لطفا از اعداد و حروف انگلیسی بیشتری در آن استفاده نمایید.";
                                break;
                            }
                        default:
                            {
                                message = "خطا : خطا به مدیر سایت ارسال شد و به زودی رفع می شود.";
                                break;
                            }
                    }
                }
                else
                {
                    message = "خطا : رمز عبور و تکرار همخوانی ندارد";
                }
                if (success)
                {
                    response.State = ResponseStateEnum.SUCCESS;
                    response.Data = message;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Data = message;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ChangePassword");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }

            return ResponseMessage(objResponse);
        }

        [Route("ChangeEmail")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult ChangeEmail([FromBody] ChangeEmailRequest input)
        {
            var response = new ActionResponse<string>();
            HttpResponseMessage objResponse = null;
            try
            {
                string message = "";
                bool success = false;
                long VCode = 0L;
                VCode = CurrentUser.VCode;
                var user = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                UserUpdateData result = ICE.Business.Engine.Instance.UserBusiness.ChangeEmail(user: user, password: input.password);
                switch (result.State)
                {
                    case var @case when @case == UserUpdateStateEnum.SUCCESSFUL:
                        {
                            success = true;
                            message = "لینک فعالسازی ایمیل برای شما ارسال شد.";
                            string returnMessage = "";
                            var cryptography = new CryptographyBusiness();
                            var encryptedVCode = cryptography.Encrypt(VCode.ToString(), EncryptKeyEnum.Key1);
                            if (!string.IsNullOrEmpty(input.newEmail))
                            {
                                var encryptedEmail = cryptography.Encrypt(input.newEmail, EncryptKeyEnum.Key1);
                                string verifyLink = "https://icescoring.com/Membership/VerifyFirmEmail?token=" + WebUtility.UrlEncode(encryptedVCode) + "&token2=" + WebUtility.UrlEncode(encryptedEmail);
                                SmtpProvider.Instance.SendEmail(email: input.newEmail, subject: "لینک فعالسازی ایمیل اکانت", message: "برای فعالسازی ایمیل خود بر روی لینک زیر کلیک نمایید :‌ " + "\n" + "<a href='" + verifyLink + "'>" + verifyLink + "</a>");
                                returnMessage += "و ایمیل ";
                                success = true;
                            }

                            break;
                        }

                    case var case1 when case1 == UserUpdateStateEnum.LOGINFAILED:
                        {
                            success = false;
                            message = "کاربر شما شناسایی نشد.";
                            break;
                        }

                    default:
                        {
                            success = false;
                            message = "خطا : خطا به مدیر سایت ارسال شد و به زودی رفع می شود.";
                            break;
                        }
                }
                if (success)
                {
                    response.State = ResponseStateEnum.SUCCESS;
                    response.Data = message;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Data = message;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ChangeEmail");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }

            return ResponseMessage(objResponse);
        }

        [Route("ChangeUsername")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult ChangeUsername([FromBody] ChangeUsernameRequest input)
        {
            var response = new ActionResponse<string>();
            HttpResponseMessage objResponse = null;
            string message = "";
            bool success = false;
            try
            {
                if (!string.IsNullOrEmpty(input.newUsername))
                {
                    if (input.newUsername.Length <= 4)
                    {
                        success = false;
                        message = "تعداد حروف نام کاربری جدید باید بیشتر از 4 باشد.";
                    }
                    else
                    {
                        var user = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                        UserUpdateData result = ICE.Business.Engine.Instance.UserBusiness.ChangeUserName(user: user, newUserName: input.newUsername, password: input.password);
                        switch (result.State)
                        {
                            case UserUpdateStateEnum.SUCCESSFUL:
                                {
                                    success = true;
                                    message = " با موفقیت انجام شد لطفا با نام کاربری جدید وارد شوید.";
                                    break;
                                }
                            case UserUpdateStateEnum.LOGINFAILED:
                                {
                                    success = false;
                                    message = "کاربر شما شناسایی نشد.";
                                    break;
                                }
                            default:
                                {
                                    success = false;
                                    message = "خطا : خطا به مدیر سایت ارسال شد و به زودی رفع می شود.";
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    success = false;
                    message = "لطفا نام کاربری را وارد نمایید";
                }
                if (success)
                {
                    response.State = ResponseStateEnum.SUCCESS;
                    response.Data = message;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Data = message;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ChangeUsername");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }

            return ResponseMessage(objResponse);
        }
        [Route("GetUserDashboardInfo")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetUserDashboardInfo()
        {
            var response = new ActionResponse<string>();
            HttpResponseMessage objResponse = null;
            try
            {
                var dashboardData = Engine.Instance.IdentificationBusiness.GetUserDashboard(CurrentUser.VCode);
                var objCurrentUser = ICE.Business.Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                dashboardData.Credit = objCurrentUser.Credit;
                var jsonStr = new JavaScriptSerializer().Serialize(new { datetime = DateTime.Now, dashboardData = dashboardData });
                response.Data = jsonStr;
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ChangeUsername");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }

            return ResponseMessage(objResponse);
        }

        [Route("GetUserRoles")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.ADD_IDENTIFICATION_FROM_FIRM))]
        public IHttpActionResult GetUserRoles()
        {
            var response = new ActionResponse<string>();
            HttpResponseMessage objResponse = null;
            try
            {
                var objCurrentUser = Engine.Instance.UserBusiness.GetUser(userVCode: CurrentUser.VCode);
                
                string roles = string.Join(",", objCurrentUser.APIs.Select(t => t.EnumName).ToArray());
                var jsonStr = new JavaScriptSerializer().Serialize(new { roles = roles });
                response.Data = jsonStr;
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ChangeUsername");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }

            return ResponseMessage(objResponse);
        }

        [Route("GetVerifyCellphoneCode")]
        [HttpGet]
        public IHttpActionResult GetVerifyCellphoneCode(string cellphone)
        {
            var response = new ActionResponse<string>();
            HttpResponseMessage objResponse = null;
            string returnMessage = "";
            try
            {
                if (!string.IsNullOrEmpty(cellphone))
                {
                    string verificationCode = "";
                    int state = 0;
                    string message = "";
                    Engine.Instance.UserBusiness.GetUserVerificationCode(cellphone, ref verificationCode, ref state, ref message);
                    if (state == 0)
                    {
                        returnMessage = "کاربری با این شماره همراه یافت نشد.";
                        response.State = ResponseStateEnum.FAILED;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            Engine.Instance.ApiBusiness.SendSms(userVCode: 0, cellphone: cellphone, message: "آيس:" + "\n" + "کد تایید : " + verificationCode, ip: "", typeVCode: SMSTypeEnum.OTP);
                        });
                        returnMessage = "کد تایید به شماره " + cellphone + "ارسال شد.";
                        response.State = ResponseStateEnum.SUCCESS;
                    }

                    response.Data = returnMessage;
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Data = "شماره همراه وارد شده صحیح نیست.";
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ChangeUsername");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }

            return ResponseMessage(objResponse);
        }

        [Route("ForgotPasswordVerifyCellphone")]
        [HttpPost]
        public IHttpActionResult ForgotPasswordVerifyCellphone([FromBody] VerifyCellPhoneData input)
        {
            var response = new ActionResponse<string>();
            HttpResponseMessage objResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(input.cellPhone))
                {
                    string verificationCode = "";
                    int state = 0;
                    string message = "";
                    var result = Engine.Instance.UserBusiness.VerifyCellphone(input.cellPhone, input.verifyCode);
                    if (result.VerificationState == VerificationStateEnum.SUCCESSFUL)
                    {
                        response.State = ResponseStateEnum.SUCCESS;
                        CryptographyBusiness cryptography = new CryptographyBusiness();
                        var objUser = Engine.Instance.UserBusiness.GetUser(CellPhone: input.cellPhone);
                        var id = cryptography.Encrypt(objUser.VCode.ToString(), EncryptKeyEnum.Key1);
                        var data = new JavaScriptSerializer().Serialize(new { message = result.Description, id = id });
                        response.Data = data;
                    }
                    else
                    {
                        response.State = ResponseStateEnum.FAILED;
                        response.Data = result.Description;
                    }
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Data = "شماره همراه وارد شده صحیح نیست.";
                }
                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ChangeUsername");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }

            return ResponseMessage(objResponse);
        }

        [Route("ForgotedChangePassword")]
        [HttpPost]
        public IHttpActionResult ForgotedChangePassword([FromBody] ForgotedChangePasswordFirmRequest input)
        {
            var response = new ActionResponse<string>();
            HttpResponseMessage objResponse = null;
            string message = "";
            bool success = false;
            try
            {
                if (!string.IsNullOrEmpty(input.id))
                {
                    CryptographyBusiness cryptography = new CryptographyBusiness();
                    var userVCode = long.Parse(cryptography.Decrypt(input.id, EncryptKeyEnum.Key1));
                    var objUser = Engine.Instance.UserBusiness.GetUser(userVCode: userVCode);
                    if (objUser.CellPhone == input.cellphone)
                    {
                        if ((input.password ?? "") == (input.confirmPassword ?? ""))
                        {
                            UserUpdateData result = ICE.Business.Engine.Instance.UserBusiness.ChangePassword(objUser, input.password);
                            switch (result.State)
                            {
                                case UserUpdateStateEnum.SUCCESSFUL:
                                    {
                                        success = true;
                                        message = "با موفقیت انجام شد.";
                                        break;
                                    }
                                case UserUpdateStateEnum.LOGINFAILED:
                                    {
                                        message = "رمز عبور قبلی شما صحیح نیست.";
                                        break;
                                    }
                                case UserUpdateStateEnum.WEAKPASSWORD:
                                    {
                                        message = "رمز عبور شما ساده میباشد لطفا از اعداد و حروف انگلیسی بیشتری در آن استفاده نمایید.";
                                        break;
                                    }
                                default:
                                    {
                                        message = "خطا : خطا به مدیر سایت ارسال شد و به زودی رفع می شود.";
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            message = "خطا : رمز عبور و تکرار همخوانی ندارد";
                        }
                    }
                    else
                    {
                        message = "خطا : شماره موبایل وارد شده با شماره کاربر مورد نظر همخانی ندارد.";
                    }
                }

                if (success)
                {
                    response.State = ResponseStateEnum.SUCCESS;
                    response.Data = message;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.State = ResponseStateEnum.FAILED;
                    response.Data = message;
                    objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                response.State = ResponseStateEnum.FAILED;
                ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/ChangePassword");
                objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }

            return ResponseMessage(objResponse);
        }
    }
}
