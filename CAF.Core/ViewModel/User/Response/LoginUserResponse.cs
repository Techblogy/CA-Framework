using CAF.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.User.Response
{
    public class LoginUserResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public long UserId { get; set; }
        public UserRole UserRole { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
