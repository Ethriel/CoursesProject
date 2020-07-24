using ServicesAPI.DTO;

namespace ServicesAPI.DataPresentation.AccountManagement
{
    public class AccountUpdateData
    {
        public SystemUserDTO User { get; set; }
        public bool IsEmailChanged { get; set; }
        public bool AnyFieldChanged { get; set; }
    }
}
