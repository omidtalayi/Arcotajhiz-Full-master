namespace ARCO.Api.Presentation.Models.Request.v1
{
    public class AuthenticationInputReqeust
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool NewToken { get; set; } = false;
    }
}