using CAF.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.EFRepository.Mapping
{
    public class AccessTokenMapping : IEntityTypeConfiguration<AccessToken>
    {
        public void Configure(EntityTypeBuilder<AccessToken> builder)
        {
            builder.ToTable("ACCESS_TOKENS");
            builder.Property(c => c.Id).HasColumnName("ID").HasDefaultValueSql("NEWID()");
            builder.Property(c => c.ExpireDate).HasColumnName("EXPIRE_DATE").IsRequired();
            builder.Property(c => c.UserId).HasColumnName("USER_ID").IsRequired();
            builder.Property(c => c.AccessTokenCode).HasColumnName("ACCESS_TOKENCODE").HasMaxLength(4000).IsRequired();
            builder.Property(c => c.CreateDate).HasColumnName("CREATE_DATE").IsRequired();
            builder.Property(c => c.CreateUserId).HasColumnName("CREATE_USER_ID").IsRequired();
            builder.Property(c => c.DbState).HasColumnName("DBSTATE").IsRequired();
            builder.Property(c => c.UpdateDate).HasColumnName("UPDATE_DATE");
            builder.Property(c => c.UpdateUserId).HasColumnName("UPDATE_USER_ID");
        }
    }
}
