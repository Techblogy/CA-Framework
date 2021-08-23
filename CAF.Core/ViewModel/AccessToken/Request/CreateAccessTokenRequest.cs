using System;

namespace CAF.Core.ViewModel.AccessToken.Request
{
    public class CreateAccessTokenRequest
    {
        public long UserId { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
