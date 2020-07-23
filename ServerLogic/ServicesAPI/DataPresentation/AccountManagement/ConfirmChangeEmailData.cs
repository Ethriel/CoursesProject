namespace ServicesAPI.DataPresentation.AccountManagement
{
    public class ConfirmChangeEmailData
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
