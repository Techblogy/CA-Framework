using CAF.Core.Entities;
using CAF.Core.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.EFRepository.Mapping
{
    public class AllFileMapping : IEntityTypeConfiguration<AllFile>
    {
        public void Configure(EntityTypeBuilder<AllFile> builder)
        {
            builder.ToTable("ALL_FILES");
            builder.Property(c => c.Id).HasColumnName("ID").HasDefaultValueSql("NEWID()");
            builder.Property(c => c.CreateDate).HasColumnName("CREATE_DATE").IsRequired();
            builder.Property(c => c.FileUrl).HasColumnName("FILE_URL").IsRequired().HasMaxLength(4000);
            builder.Property(c => c.FilePath).HasColumnName("FILE_PATH").IsRequired().HasMaxLength(4000);
            builder.Property(c => c.FileName).HasColumnName("FILE_NAME").IsRequired().HasMaxLength(4000);
            builder.Property(c => c.RelationId).HasColumnName("RELATION_ID").IsRequired();
            builder.Property(c => c.CreateUserId).HasColumnName("CREATE_USER_ID").IsRequired();
            builder.Property(c => c.DbState).HasColumnName("DBSTATE").IsRequired();
            builder.Property(c => c.FileCategory).HasColumnName("FILE_CATEGORY").IsRequired();
        }
    }
}
