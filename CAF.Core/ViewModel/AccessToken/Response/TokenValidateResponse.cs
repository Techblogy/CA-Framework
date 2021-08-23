namespace CAF.Core.ViewModel.AccessToken.Response
{
    public class TokenValidateResponse
    {
        public bool Valid { get; set; }
        public string Message { get; set; }

        public TokenValidateResponse()
        {

        }
        public TokenValidateResponse(bool valid, string message)
        {
            Valid = valid;
            Message = message;
        }
    }
}
