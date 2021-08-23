using CAF.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.EFRepository.Mapping
{
    public class SettingMapping : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("SETTINGS");
            builder.Property(c => c.Id).HasColumnName("ID").HasDefaultValueSql("NEWID()");
            builder.Property(c => c.Key).HasColumnName("KEY").HasMaxLength(100).IsRequired();
            builder.Property(c => c.GroupKey).HasColumnName("GROUP_KEY").HasMaxLength(100);
            builder.Property(c => c.Value).HasColumnName("VALUE").HasDefaultValue<string>("");
        }
    }
}
