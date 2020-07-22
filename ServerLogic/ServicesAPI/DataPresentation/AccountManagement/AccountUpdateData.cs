using ServicesAPI.DTO;

namespace ServicesAPI.DataPresentation.AccountManagement
{
    public class AccountUpdateData
    {
        public SystemUserDTO User { get; set; }
        public bool IsEmailChanged { get; set; }
        public bool IsFirstNameChanged { get; set; }
        public bool IsLastNameChanged { get; set; }
    }
}
