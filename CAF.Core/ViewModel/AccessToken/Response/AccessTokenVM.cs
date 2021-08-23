using CAF.Core.Enums;

using System;

namespace CAF.Core.ViewModel.AccessToken.Response
{
    public class AccessTokenVM
    {
        public Guid Id { get; set; }
        public string AccessTokenCode { get; set; }
        public string UserName { get; set; }
        //public string AgencyName { get; set; }
        public DateTime ExpireDate { get; set; }
        public DbState Status { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }


        public AccessTokenVM()
        {

        }
        public AccessTokenVM(Entities.AccessToken entity)
        {
            if (entity == null) return;

            ExpireDate = entity.ExpireDate;
            UserName = $"{entity.User?.FirstName} {entity.User?.LastName}";
            Status = entity.DbState;
            //AgencyName = entity.Agency?.Name;
            CreateUserName = $"{entity.CreateUser?.FirstName} {entity.CreateUser?.LastName}";
            UpdateUserName = $"{entity.UpdateUser?.FirstName} {entity.UpdateUser?.LastName}";
            AccessTokenCode = entity.AccessTokenCode;
            Id = entity.Id;
        }
    }
}
