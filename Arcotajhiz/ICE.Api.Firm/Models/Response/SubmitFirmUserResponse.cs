using ICE.Entities.Enums;

namespace ICE.Api.Firm.Models.Response
{
    public class SubmitFirmUserResponse
    {
        public string firmCode { get; set; }
        public string cellphone { get; set; }
        public string message { get; set; }
        public FirmRegistrationStateEnum firmRegistrationState { get; set; }
    }
}