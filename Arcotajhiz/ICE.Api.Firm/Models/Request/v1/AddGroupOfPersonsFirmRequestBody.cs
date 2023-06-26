using ICE.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class AddGroupOfPersonsFirmRequestBody
    {
        public string CellPhone { get; set; }
        public string NationalCode { get; set; }
        public string CompanyNationalID { get; set; }
        public bool hasChequeReport { get; set; }
        public Entities.Enums.UserPaymentTypeEnum UserPaymentTypeEnum { get; set; }
        public ApiErrorCodeEnum Validation()
        {
            ApiErrorCodeEnum errorCode = ApiErrorCodeEnum.NONE;
            if (!Regex.IsMatch(CellPhone, @"09\d{9}"))
                errorCode = ApiErrorCodeEnum.CELLPHONE_IS_INVALID;
            if (string.IsNullOrEmpty(CellPhone))
                errorCode = ApiErrorCodeEnum.CELLPHONE_IS_INVALID;
            if (!ICE.Presentation.Common.General.IsValidNationalCode(NationalCode))
                errorCode = ApiErrorCodeEnum.NATIONAL_CODE_IS_INVALID;
            if (string.IsNullOrEmpty(NationalCode))
                errorCode = ApiErrorCodeEnum.NATIONAL_CODE_IS_INVALID;
            return errorCode;
        }
    }
}