using System.Collections.Generic;
using System.Text.RegularExpressions;
using ICE.Api.Common.Models;
using ICE.Entities.Enums;
using ICE.Presentation.Common;

namespace ICE.Api.Presentation.Models.Request.v1
{
    public class CheckIndividualReportExistenceRequest : ActionRequest
    {
        public string NationalCode { get; set; }
        public string Cellphone { get; set; }
        public string ReceiverCellphone { get; set; }
        public List<ReportSourceEnum> ReportSources { get; set; }
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
            foreach (var reportSource in ReportSources)
                if (reportSource != ReportSourceEnum.ICS)
                    errorCode = ApiErrorCodeEnum.REPORT_SOURCE_IS_INVALID;
            return errorCode;
        }
    }
}