using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

using ICE.Api.Common.Controller;
using ICE.Api.Firm.Models.User;

namespace ICE.Api.Firm.Controllers
{
    public class BaseApiPresentationController : BaseApiController
    {
        public string MainURL = "https://www.icescoring.com/";
        public bool testEnabled = ICE.Business.Engine.Instance.ApplicationBusiness.GetSetting("TestEnabled") == "1";
        public Models.User.CurrentUser CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;
                var routeValues = Request.GetRouteData();
                var user = routeValues.Values["CurrentUser"] as CurrentUser;
                return user;
            }
        }
        public static string StringToURLCustom(string text)
        {
            return text.Replace("/", "(S)").Replace("&", "(A)").Replace("+", "(P)").Replace("=", "(E)");
        }
        public static string GetIp()
        {
            return GetClientIp();
        }

        private static string GetClientIp(HttpRequestMessage request = null)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
        public string Unify(string input)
        {
            if (input == null) {
                return null;
            }
            return Regex.Replace(input.Replace("ﻼ", "لا").Replace("ﻻ", "لا").Replace(" ", " ").Replace("أ", "ا").Replace("إ", "ا").Replace("ي", "ی").Replace("ة", "ه").Replace("ۀ", "ه").Replace("ك", "ک").Replace("ؤ", "و").Replace("٠", "0").Replace("۰", "0").Replace("١", "1").Replace("۱", "1").Replace("٢", "2").Replace("۲", "2").Replace("٣", "3").Replace("۳", "3").Replace("٤", "4").Replace("۴", "4").Replace("٥", "5").Replace("۵", "5").Replace("٦", "6").Replace("۶", "6").Replace("٧", "7").Replace("۷", "7").Replace("٨", "8").Replace("۸", "8").Replace("٩", "9").Replace("۹", "9"), "[ ]+", " ").Trim();
        }


    }
}
