using System;
using System.IO.Pipes;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
//using ICE.Api.Presentation.Models.Request.v1;
//using ICE.Api.Presentation.Models.Response;
//using ICE.Api.Presentation.Modules;
//using ResponseStateEnum = ICE.Api.Common.Enums.ResponseStateEnum;

namespace ICE.Api.Presentation.Controllers.v1
{
    //[RoutePrefix("api/v1/user")]
    //public class V1_UserController : BaseApiController
    //{
    //    // GET: User
    //    [Route("authenticate")]
    //    [HttpPost]
    //    public IHttpActionResult Authenticate([FromBody]AuthenticationInputReqeust input)
    //    {
    //        var response = new ActionResponse<LoginResponse>();
    //        HttpResponseMessage objResponse;
    //        try
    //        {
    //            var loginData = Engine.Instance.UserBusiness.Login(username: input.UserName, password: input.Password);

    //            if (loginData.LoginState == LoginStateEnum.SUCCESSFUL)
    //            {
    //                DateTime databaseCurrentServerDate = ICE.Business.Engine.Instance.ApplicationBusiness.GetDatabaseServerTime();
    //                string token = "";
    //                string tokenCode = "";
    //                DateTime tokenExpireTime = DateTime.MinValue;
    //                if (loginData.User.Token != null && loginData.User.TokenExpirationDate  >= databaseCurrentServerDate && !input.NewToken)
    //                {
    //                    token = loginData.User.Token;
    //                    tokenExpireTime = loginData.User.TokenExpirationDate;
    //                }
    //                else
    //                {
    //                    var objJwt = new JwtManager();
    //                    var objUserSession = objJwt.GenerateToken(username: input.UserName, userVCode: loginData.User.VCode,
    //                        expireMinutes: loginData.User.TokenExpirationTime, apis: loginData.User.APIs);
    //                    token = objUserSession.token;
    //                    tokenCode = objUserSession.TokenCode;
    //                    tokenExpireTime = objUserSession.TokenExpireTime;
    //                    var objUser = new UserData();
    //                    objUser.Username = input.UserName;
    //                    objUser.Token = token;
    //                    objUser.SecretCode = tokenCode;
    //                    objUser.TokenExpirationDate = tokenExpireTime;
    //                    Engine.Instance.UserBusiness.UserTokenIns(ref objUser);
    //                }
    //                response.State = ResponseStateEnum.SUCCESS;
    //                response.Data = new LoginResponse();
    //                response.Data.token = token;
    //                response.Data.userName = loginData.User.Username;
    //                response.Data.tokenExpireDate = tokenExpireTime;
    //                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
    //            }
    //            else
    //            {
    //                response.State = ResponseStateEnum.FAILED;
    //                response.Errors.Add(ApiErrorCodeEnum.USERNAME_OR_PASSWORD_IS_INVALID.ToString());
    //                objResponse = Request.CreateResponse(HttpStatusCode.Forbidden, response);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
    //            response.State = ResponseStateEnum.FAILED;
    //            ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/Authenticate");
    //            objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
    //        }
    //        return ResponseMessage(objResponse);
    //    }
        //[Route("authenticateHash")]
        //[HttpPost]
        //public IHttpActionResult authenticateHash([FromBody]AuthenticationInputReqeust input)
        //{
        //    var response = new ActionResponse<LoginResponse>();
        //    HttpResponseMessage objResponse;
        //    try
        //    {
        //        LoginData loginData = null;
        //        if (HttpContext.Current.Request.UserHostAddress == "79.175.172.101" || HttpContext.Current.Request.UserHostAddress == "92.119.70.154" || HttpContext.Current.Request.UserHostAddress == "91.92.215.157" || HttpContext.Current.Request.UserHostAddress == "::1")
        //        {
        //            loginData = Engine.Instance.UserBusiness.Login(username: input.UserName, hashedPassword: input.Password);
        //            if (loginData.LoginState == LoginStateEnum.SUCCESSFUL)
        //            {
        //                DateTime databaseCurrentServerDate = ICE.Business.Engine.Instance.ApplicationBusiness.GetDatabaseServerTime();
        //                string token = "";
        //                string tokenCode = "";
        //                DateTime tokenExpireTime = DateTime.MinValue;
        //                if (loginData.User.Token != null && loginData.User.TokenExpirationDate >= databaseCurrentServerDate && !input.NewToken)
        //                {
        //                    token = loginData.User.Token;
        //                    tokenExpireTime = loginData.User.TokenExpirationDate;
        //                }
        //                else
        //                {
        //                    var objJwt = new JwtManager();
        //                    var objUserSession = objJwt.GenerateToken(username: input.UserName, userVCode: loginData.User.VCode,
        //                        expireMinutes: loginData.User.TokenExpirationTime, apis: loginData.User.APIs);
        //                    var objUser = new UserData();
        //                    objUser.Username = input.UserName;
        //                    objUser.Token = objUserSession.token;
        //                    token = objUserSession.token;
        //                    objUser.SecretCode = objUserSession.TokenCode;
        //                    objUser.TokenExpirationDate = objUserSession.TokenExpireTime;
        //                    Engine.Instance.UserBusiness.UserTokenIns(ref objUser);
        //                }
        //                response.State = ResponseStateEnum.SUCCESS;
        //                response.Data = new LoginResponse();
        //                response.Data.token = token;
        //                response.Data.userName = loginData.User.Username;
        //                response.Data.tokenExpireDate = loginData.User.TokenExpirationDate;
        //                objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
        //            }
        //            else
        //            {
        //                response.State = ResponseStateEnum.FAILED;
        //                response.Errors.Add(ApiErrorCodeEnum.USERNAME_OR_PASSWORD_IS_INVALID.ToString());
        //                objResponse = Request.CreateResponse(HttpStatusCode.Forbidden, response);
        //            }
        //        }
        //        else
        //        {
        //            response.State = ResponseStateEnum.FAILED;
        //            response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
        //            objResponse = Request.CreateResponse(HttpStatusCode.Forbidden, response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
        //        response.State = ResponseStateEnum.FAILED;
        //        ICE.Common.Business.Engine.Instance.ErrorBusiness.ErrorLogIns(ex, ModuleName: "User/authenticateHash");
        //        objResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
        //    }
        //    return ResponseMessage(objResponse);
        //}
    }
}