using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICE.Entities.Enums;
using ICE.Presentation.Common.Models;

namespace ICE.Api.Firm.Models.Response
{
    public class GetReportDataResponseCompanyPerson
    {
        public CreditRiskReportEnum ReportState { get; set; }
        public CompanyPersonReportData ReportData { get; set; }
        public string EmptyReportData { get; set; }
        public string ReportLink { get; set; }
        public string PDFLink { get; set; }
    }
}