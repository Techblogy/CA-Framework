using CAF.Core.Enums;
using CAF.Core.Helper;

using System;

namespace CAF.Core.ViewModel.User.Request
{
    public class AddUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole RoleType { get; set; }
        public string ProfileImageBase64 { get; set; }
        public string SubRoles { get; set; }

        /// <summary>
        /// gönderilen şifreyi md5 ile şifreler
        /// </summary>
        /// <returns></returns>
        public virtual Entities.User ToEntity()
        {

            return new Entities.User()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password.ToMD5Hash(),
                RoleType = RoleType,
                SubRoles = SubRoles
            };
        }
    }
}
