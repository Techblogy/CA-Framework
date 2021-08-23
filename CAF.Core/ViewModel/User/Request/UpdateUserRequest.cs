using CAF.Core.Enums;

namespace CAF.Core.ViewModel.User.Request
{
    public class UpdateUserRequest : AddUserRequest
    {
        public long Id { get; set; }
        public DbState DbState { get; set; }


        public override Entities.User ToEntity()
        {
            var user = base.ToEntity();
            user.Id = Id;
            user.DbState = DbState;
            return user;
        }
    }
}
