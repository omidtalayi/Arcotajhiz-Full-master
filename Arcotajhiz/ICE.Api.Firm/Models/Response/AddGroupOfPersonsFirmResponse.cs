using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICE.Entities.Enums;

namespace ICE.Api.Firm.Models.Response
{
    public class AddGroupOfPersonsFirmResponse
    {
        public string ReportID { get; set; }
        public GroupOfPersonsStateEnum ResponseState { get; set; }
        public string ResponseMessage { get; set; }
        public string ReportUrl { get; set; }
        public string VerifyUrl { get; set; }
        public string NationalCode { get; set; }
        public decimal RemainCredit { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}