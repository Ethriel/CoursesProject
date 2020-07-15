namespace ServicesAPI.Responses.AccountResponseData
{
    public enum AccountOperationResult
    {
        Succeeded,
        Failed
    }
    public class AccountResponse
    {
        public AccountData AccountData { get; set; }
        public AccountOperationResult AccountOperationResult { get; set; }
        public AccountResponse(AccountData accountData, AccountOperationResult accountOperationResult)
        {
            AccountData = accountData;
            AccountOperationResult = accountOperationResult;
        }
    }
}
