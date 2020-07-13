namespace ServerAPI.Responses
{
    public class TokenResponse
    {
        public string Key { get; set; }
        public double Expires { get; set; }
        public TokenResponse()
        {

        }
        public TokenResponse(string key, double expires)
        {
            Key = key;
            Expires = expires;
        }
    }
}
