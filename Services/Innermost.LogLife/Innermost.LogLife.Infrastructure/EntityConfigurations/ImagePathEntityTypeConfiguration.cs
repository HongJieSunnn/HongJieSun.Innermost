namespace Innermost.LogLife.Infrastructure.EntityConfigurations
{
    public class ImagePathEntityTypeConfiguration : IEntityTypeConfiguration<ImagePath>
    {
        public void Configure(EntityTypeBuilder<ImagePath> builder)
        {
            builder.ToTable("ImagePaths");

            builder.HasKey(x => x.Id);

            builder
                .Property(i => i.Path)
                .HasColumnName("Path")
                .IsRequired();

            builder
                .HasIndex(i => i.Path)
                .IsUnique();

            builder
                .HasOne(i => i.LifeRecord)
                .WithMany(l => l.ImagePaths)
                .HasForeignKey(i => i.RecordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
