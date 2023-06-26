namespace ICE.Api.Firm.Models.Request.v1
{
    public class VerifyCellPhoneData
    {
        public string verifyCode { get; set; }
        public string firmCode { get; set; }
        public string cellPhone { get; set; }
    }
}