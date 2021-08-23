using CAF.Core.Enums;
using CAF.Core.Entities;
using CAF.Core.Extensions;

using System;

namespace CAF.Core.ViewModel.User.Response
{
    public class UserVM
    {
        public long Id { get; set; }
        public DbState DbState { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole RoleType { get; set; }
        public string ProfileImageUrl { get; set; }
        public string SubRoles { get; set; }

        public UserVM()
        {

        }
        public UserVM(Core.Entities.User entity)
        {

            FirstName = entity.FirstName;
            LastName = entity.LastName;
            Email = entity.Email;
            Password = string.Empty;
            RoleType = entity.RoleType;
            Id = entity.Id;
            DbState = entity.DbState;
            ProfileImageUrl = entity.ProfileImageUrl;
            SubRoles = entity.SubRoles;
        }
    }
}
