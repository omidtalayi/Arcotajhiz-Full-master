namespace ICE.Api.Firm.Models.Request.v1
{
    public class ResendVerifyCodeData
    {
        public string firmCode { get; set; }
        public string reportId { get; set; }
        public string cellPhone { get; set; }
    }
}