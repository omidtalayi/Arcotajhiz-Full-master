using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ICE.Api.Common.Models;
using ICE.Entities.Enums;

namespace ICE.Api.Presentation.Models.Request.v1
{
    public class GetIndividualInformationRequest : ActionRequest

    {
        public string NationalCode { get; set; }
        public string Cellphone { get; set; }
        public string SalesRefID { get; set; }
        public decimal SaleAmount { get; set; }
        public string ReceiverCellphone { get; set; }
        public List<ReportSourceEnum> ReportSources { get; set; }
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
            if (string.IsNullOrEmpty(SalesRefID))
                errorCode = ApiErrorCodeEnum.SALES_REFID_IS_INVALID;
            if (SaleAmount <= 0)
                errorCode = ApiErrorCodeEnum.SALES_AMOUNT_IS_INVALID;
            if (string.IsNullOrEmpty(NationalCode))
                errorCode = ApiErrorCodeEnum.NATIONAL_CODE_IS_INVALID;
            foreach (ReportSourceEnum reportSource in ReportSources)
            {
                if (reportSource != ReportSourceEnum.ICS)
                    errorCode = ApiErrorCodeEnum.REPORT_SOURCE_IS_INVALID;
            }
            return errorCode;
        }
    }
}