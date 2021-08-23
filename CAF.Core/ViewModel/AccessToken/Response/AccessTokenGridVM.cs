using CAF.Core.Enums;

using System;

namespace CAF.Core.ViewModel.AccessToken.Response
{
    public class AccessTokenGridVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        // string AgencyName { get; set; }
        public DateTime ExpireDate { get; set; }
        public DbState Status { get; set; }
        public string AccessTokenCode { get; set; }


        public AccessTokenGridVM()
        {

        }
        public AccessTokenGridVM(Entities.AccessToken entity)
        {
            if (entity == null) return;

            ExpireDate = entity.ExpireDate;
            UserName = $"{entity.User?.FirstName} {entity.User?.LastName}";
            Status = entity.DbState;
            //AgencyName = entity.Agency?.Name;
            Id = entity.Id;
            AccessTokenCode = entity.AccessTokenCode;
        }
    }
}
