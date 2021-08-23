using CAF.Core.Enums;
using CAF.Core.Interface;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Entities
{
    public class User : KeyEntitiy, IDbState
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole RoleType { get; set; }
        public DbState DbState { get; set; }
        public string ResetPasswordCode { get; set; }
        public string ProfileImageUrl { get; set; }
        public string SubRoles { get; set; }

    }
}
