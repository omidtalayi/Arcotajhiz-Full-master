using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ICE.Api.Common.Models;
using ICE.Entities.Enums;

namespace ICE.Api.Presentation.Models.Request.v1
{
    public class SendSmsRequest : ActionRequest
    {
        public string Cellphone { get; set; }
        public string Message { get; set; }
        public ApiErrorCodeEnum Validation()
        {
            ApiErrorCodeEnum errorCode = ApiErrorCodeEnum.NONE;
            if (!Regex.IsMatch(Cellphone, @"09\d{9}"))
                errorCode = ApiErrorCodeEnum.CELLPHONE_IS_INVALID;
            return errorCode;
        }
    }
}