namespace ServicesAPI.Responses.AccountResponseData
{
    public class TokenData
    {
        public string Key { get; set; }
        public double Expires { get; set; }
        public TokenData()
        {

        }
        public TokenData(string key, double expires)
        {
            Key = key;
            Expires = expires;
        }
    }
}
