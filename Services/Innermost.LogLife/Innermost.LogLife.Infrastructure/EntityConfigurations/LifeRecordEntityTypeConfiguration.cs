namespace Innermost.LogLife.Infrastructure.EntityConfigurations
{
    class LifeRecordEntityTypeConfiguration
        : IEntityTypeConfiguration<LifeRecord>
    {
        public void Configure(EntityTypeBuilder<LifeRecord> builder)
        {
            builder.ToTable("LifeRecords");

            builder.HasKey(l => l.Id);

            builder.HasIndex(l => l.CreateTime);
            builder.HasIndex("_userId");
            builder.HasIndex(l => l.Title).IsFullText(true);
            builder.HasIndex(l => l.Text).IsFullText(true);
            builder.HasIndex("_locationUId");
            builder.HasIndex("_musicRecordMId");
            builder.HasIndex("_isShared");

            builder
                .Property("_userId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("UserId")
                .HasMaxLength(95)
                .IsRequired();

            builder
                .Property(l => l.Title)
                .HasColumnName("Title")
                .HasMaxLength(200)
                .IsRequired(false);

            builder
                .Property(l => l.Text)
                .HasCharSet(CharSet.Utf8Mb4)
                .HasColumnName("Text")
                .HasMaxLength(3000)
                .IsRequired();

            builder
                .Property("_locationUId")
                .HasColumnName("LocationUId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired(false);

            builder
                .Property("_musicRecordMId")
                .HasColumnName("MusicRecordMId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired(false);

            builder
                .Property(l => l.CreateTime)
                .HasColumnType("DateTime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CreateTime")
                .IsRequired();

            builder
                .Property(l => l.UpdateTime)
                .HasColumnType("DateTime")
                .HasColumnName("UpdateTime")
                .IsRequired(false);

            builder
                .Property(l => l.DeleteTime)
                .HasColumnType("DateTime")
                .HasColumnName("DeleteTime")
                .IsRequired(false);

            builder
                .Property("_isShared")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("IsShared")
                .IsRequired();

            builder
                .HasOne(l => l.Location)
                .WithMany()
                .HasForeignKey("_locationUId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder
                .HasOne(l=>l.MusicRecord)
                .WithMany()
                .HasForeignKey("_musicRecordMId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasMany(l => l.Tags).WithMany(t => t.Entities).UsingEntity("LifeRecordTagSummary");
        }
    }
}
