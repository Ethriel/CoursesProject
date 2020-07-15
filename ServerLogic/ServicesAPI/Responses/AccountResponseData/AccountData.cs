using ServicesAPI.DTO;

namespace ServicesAPI.Responses.AccountResponseData
{
    public class AccountData
    {
        public SystemUserDTO User { get; set; }
        public TokenData Token { get; set; }
        public AccountData()
        {

        }
        public AccountData(SystemUserDTO systemUserDTO, TokenData tokenResponse)
        {
            User = systemUserDTO;
            Token = tokenResponse;
        }
    }
}
