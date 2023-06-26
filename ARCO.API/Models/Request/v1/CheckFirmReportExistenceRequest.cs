
using ARCO.Api.Common.Class;
using ARCO.Api.Common.Models;
using ARCO.Api.Common.Models;
using ARCO.Entities.Enums;
using System.Text.RegularExpressions;

namespace ARCO.Api.Presentation.Models.Request.v1
{
    public class CheckFirmReportExistenceRequest : ActionRequest
    {
        public string NationalCode { get; set; }
        public string Cellphone { get; set; }
        public string ReceiverCellphone { get; set; }

        public ApiErrorCodeEnum Validation()
        {
            var errorCode = ApiErrorCodeEnum.NONE;
            if (!Regex.IsMatch(Cellphone, @"09\d{9}"))
                errorCode = ApiErrorCodeEnum.CELLPHONE_IS_INVALID;
            if (string.IsNullOrEmpty(Cellphone))
                errorCode = ApiErrorCodeEnum.CELLPHONE_IS_INVALID;
            if (!General.IsValidNationalCode(NationalCode))
                errorCode = ApiErrorCodeEnum.NATIONAL_CODE_IS_INVALID;
            if (string.IsNullOrEmpty(NationalCode))
                errorCode = ApiErrorCodeEnum.NATIONAL_CODE_IS_INVALID;
            return errorCode;
        }
    }
}