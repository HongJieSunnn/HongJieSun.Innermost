namespace Innermost.LogLife.Infrastructure.EntityConfigurations
{
    class LifeRecordEntityTypeConfiguration
        : IEntityTypeConfiguration<LifeRecord>
    {
        public void Configure(EntityTypeBuilder<LifeRecord> builder)
        {
            builder.ToTable("LifeRecords");

            builder.HasKey(l => l.Id);

            //TODO add indexs to the columns always be searched.
            builder.HasIndex(l => l.CreateTime);

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
                .Property("_locationId")
                .HasColumnName("LocationId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired();

            builder
                .Property("_mId")
                .HasColumnName("MusicId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired();

            builder
                .Property(l => l.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CreateTime")
                .IsRequired();

            builder
                .Property(l => l.UpdateTime)
                .HasColumnName("UpdateTime")
                .IsRequired(false);

            builder
                .Property(l => l.DeleteTime)
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
                .HasForeignKey("_locationId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder
                .HasOne(l=>l.MusicRecord)
                .WithMany()
                .HasForeignKey("_musicRecordId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
