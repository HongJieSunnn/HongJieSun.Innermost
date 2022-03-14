namespace Innermost.LogLife.Infrastructure.EntityConfigurations
{
    public class MusicRecordEntityTypeConfiguration
        : IEntityTypeConfiguration<MusicRecord>
    {
        public void Configure(EntityTypeBuilder<MusicRecord> builder)
        {
            builder.ToTable("MusicRecord");

            builder.HasKey(m => m.Id);

            builder
                .Property(m => m.MusicName)
                .HasColumnName("MusicName")
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(m => m.Singer)
                .HasColumnName("Singer")
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(m => m.Album)
                .HasColumnName("Album")
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
