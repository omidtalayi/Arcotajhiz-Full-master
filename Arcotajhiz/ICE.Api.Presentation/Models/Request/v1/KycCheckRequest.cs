using ICE.Api.Common.Models;
using ICE.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ICE.Api.Presentation.Models.Request.v1
{
    public class KycCheckRequest : ActionRequest
    {
        public string NationalCode { get; set; }
        public string Cellphone { get; set; }
        public ApiErrorCodeEnum Validation()
        {
            ApiErrorCodeEnum errorCode = ApiErrorCodeEnum.NONE;
            if (!Regex.IsMatch(Cellphone, @"09\d{9}"))
                errorCode = ApiErrorCodeEnum.CELLPHONE_IS_INVALID;
            if (string.IsNullOrEmpty(Cellphone))
                errorCode = ApiErrorCodeEnum.CELLPHONE_IS_INVALID;
            if (!ICE.Presentation.Common.General.IsValidNationalCode(NationalCode))
                errorCode = ApiErrorCodeEnum.NATIONAL_CODE_IS_INVALID;
            if (string.IsNullOrEmpty(NationalCode))
                errorCode = ApiErrorCodeEnum.NATIONAL_CODE_IS_INVALID;
            return errorCode;
        }
    }
}