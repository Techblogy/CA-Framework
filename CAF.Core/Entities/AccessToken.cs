using CAF.Core.Enums;
using CAF.Core.Interface;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Entities
{
    public class AccessToken : GuidEntitiy, IDbState, ICreateUserEntity, IUpdateUserEntity
    {
        public DateTime ExpireDate { get; set; }
        public long UserId { get; set; }
        //public Guid AgencyId { get; set; }
        public string AccessTokenCode { get; set; }

        #region Base
        public DateTime CreateDate { get; set; }
        public long? CreateUserId { get; set; }
        public DbState DbState { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserId { get; set; }
        #endregion

        public User CreateUser { get; set; }
        public User UpdateUser { get; set; }
        //public Agency Agency { get; set; }
        public User User { get; set; }
    }
}
