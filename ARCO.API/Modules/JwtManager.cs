using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http.Filters;
using ARCO.Api.Presentation.Models.User;
using ARCO.Business;
using ARCO.Business.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ARCO.Api.Presentation.Modules
{
    public class JwtManager
    {
        public Models.User.CurrentUser GenerateToken(string username, long userVCode, int expireMinutes = 20, List<ApiData> apis = null)
        {
            var hmac = new HMACSHA256();
            var key = Convert.ToBase64String(hmac.Key);
            var symmetricKey = Convert.FromBase64String(key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var clientId = new Guid();
            var now = DateTime.Now;
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new []
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim("VCode", userVCode.ToString())
                        //new Claim(ClaimTypes.Role , "admin")
                        //new Claim(ClaimTypes.Role, String.Join(",", apis.Select(x=>x.EnumName).ToArray()))
                    }),
                Expires = now.AddMinutes(expireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //foreach (var item in apis)
            //{
            //    tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, item.EnumName)); 
            //}
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            var userSession = new Models.User.CurrentUser()
            {
                UserName = username,
                VCode = userVCode,
                TokenExpireTime = tokenDescriptor.Expires.Value,
                TokenCode = key,
                token = token
            };
            return userSession;
        }

        public static ClaimsPrincipal GetPrincipal(string token, HttpAuthenticationContext context)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                    return null;
                var userSession = Engine.Instance.UserBusiness.GetUser(token: token);
                if (userSession == null)
                    throw new Exception("User session not found");
                if (userSession.TokenState == Entities.Enums.TokenStateEnum.SUCCESSFUL)
                {
                    var symmetricKey = Convert.FromBase64String(userSession.SecretCode);
                    var validationParameters = new TokenValidationParameters()
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                    };
                    var principal = tokenHandler.ValidateToken(token, validationParameters,out SecurityToken securityToken);
                    var authenticatedUser = new CurrentUser()
                    {
                        UserName = userSession.Username,
                        VCode = userSession.VCode,
                        apis = userSession.APIs
                    };
                    context.ActionContext.RequestContext.RouteData.Values.Add("CurrentUser", authenticatedUser);
                    return principal;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}