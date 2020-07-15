using ServicesAPI.DTO;

namespace ServicesAPI.Responses
{
    public class AccountResponse
    {
        public SystemUserDTO User { get; set; }
        public TokenResponse Token { get; set; }
        public AccountResponse()
        {

        }
        public AccountResponse(SystemUserDTO systemUserDTO, TokenResponse tokenResponse)
        {
            User = systemUserDTO;
            Token = tokenResponse;
        }
    }
}
