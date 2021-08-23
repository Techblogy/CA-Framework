using CAF.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.EFRepository.Mapping
{
    public class ActionLogMapping : IEntityTypeConfiguration<ActionLog>
    {
        public void Configure(EntityTypeBuilder<ActionLog> builder)
        {
            builder.ToTable("ACTION_LOG");
            builder.Property(c => c.Id).HasColumnName("ID").UseIdentityColumn();
            builder.Property(c => c.EnumType).HasColumnName("ENUM_TYPE").IsRequired();
            builder.Property(c => c.Message).HasColumnName("MESSAGE").IsRequired();
            builder.Property(c => c.UserId).HasColumnName("USER_ID").IsRequired();
            builder.Property(c => c.Date).HasColumnName("DATE").IsRequired();
        }
    }
}
