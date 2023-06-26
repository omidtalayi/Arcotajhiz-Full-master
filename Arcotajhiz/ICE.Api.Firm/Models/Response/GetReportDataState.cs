using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICE.Entities.Enums;

namespace ICE.Api.Firm.Models.Response
{
    public class GetReportDataState
    {
        public CompanyPersonStateEnum ReportState { get; set; }
        public string ReportStateStr { get; set; }
    }
}