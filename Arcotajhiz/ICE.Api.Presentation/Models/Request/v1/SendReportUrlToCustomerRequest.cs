using ICE.Api.Common.Models;

namespace ICE.Api.Presentation.Models.Request.v1
{
    public class SendReportUrlToCustomerRequest : ActionRequest
    {
        public long BatchResultVCode { get; set; }
        public long IdentificationVCode { get; set; }
        public long UserVCode { get; set; }
        public bool HasShahkarIdentified { get; set; }
        public bool IsLegalPerson { get; set; }
    }
}