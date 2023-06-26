using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICE.Entities.Enums;

namespace ICE.Api.Firm.Models.Response
{
    public class GetReportDataResponse
    {
        public CreditRiskReportEnum ReportState { get; set; }
        public string ReportData { get; set; }
        public string EmptyReportData { get; set; }
        public string ReportLink { get; set; }
        public string PDFLink { get; set; }
    }
}