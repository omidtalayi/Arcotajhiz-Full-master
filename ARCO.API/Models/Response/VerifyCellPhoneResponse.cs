using ARCO.Entities.Enums;

namespace ARCO.Api.Presentation.Models.Response
{
    public class VerifyCellPhoneResponse
    {
        public VerifyCellphoneStateApiEnum verifyState { get; set; }
        public string reportProcessingLink { get; set; }
        public string verifyStateMessage { get; set; }
    }
}