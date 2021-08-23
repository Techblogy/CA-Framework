using CAF.Core.Enums;
using CAF.Core.Extensions;

namespace CAF.Core.ViewModel.User.Response
{
    public class UserGridVM
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public UserRole RoleType { get; set; }
        public string RoleTypeCaption { get; set; }
        public DbState DbState { get; set; }
        public string Email { get; set; }

        public UserGridVM()
        {

        }

        public UserGridVM(Core.Entities.User entity)
        {
            Id = entity.Id;
            FullName = $"{entity.FirstName} {entity.LastName}";
            RoleType = entity.RoleType;
            DbState = entity.DbState;
            RoleTypeCaption = entity.RoleType.GetDisplayNameValue();
            Email = entity.Email;
        }
    }
}
