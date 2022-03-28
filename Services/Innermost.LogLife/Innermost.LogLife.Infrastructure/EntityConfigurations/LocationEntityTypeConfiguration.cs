namespace Innermost.LogLife.Infrastructure.EntityConfigurations
{
    class LocationEntityTypeConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");

            builder.HasKey(l=>l.Id);

            builder
                .Property(l => l.LocationName)
                .HasColumnName("LocationName")
                .IsRequired();

            builder
                .Property(l => l.Province)
                .HasMaxLength(80)
                .HasColumnName("Province")
                .IsRequired();

            builder
                .Property(l => l.City)
                .HasMaxLength(80)
                .HasColumnName("City")
                .IsRequired();

            builder
                .Property(l => l.District)
                .HasMaxLength(80)
                .HasColumnName("District")
                .IsRequired(false);

            builder
                .Property(l => l.Address)
                .HasMaxLength(200)
                .HasColumnName("Address")
                .IsRequired();

            builder
                .OwnsOne(l => l.BaiduPOI);
        }
    }
}
