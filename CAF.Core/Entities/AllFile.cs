using CAF.Core.Enums;
using CAF.Core.Interface;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Entities
{
    public class AllFile : GuidEntitiy, ICreateUserEntity, IUpdateUserEntity, IDbState
    {
        public Guid RelationId { get; set; }
        public string FileUrl { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public FileCategory FileCategory { get; set; }

        #region Interface
        public DateTime CreateDate { get; set; }
        public long? CreateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserId { get; set; }
        public DbState DbState { get; set; }
        #endregion

        public User CreateUser { get; set; }
    }
}
