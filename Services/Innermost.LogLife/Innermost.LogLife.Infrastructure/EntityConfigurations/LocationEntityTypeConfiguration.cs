namespace Innermost.LogLife.Infrastructure.EntityConfigurations
{
    class LocationEntityTypeConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");

            builder.HasKey(l=>l.Id);

            builder
                .Property(l => l.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder
                .Property(l => l.Province)
                .HasColumnName("Province")
                .IsRequired();

            builder
                .Property(l => l.City)
                .HasColumnName("City")
                .IsRequired();

            builder
                .Property(l => l.District)
                .HasColumnName("District")
                .IsRequired(false);

            builder
                .Property(l => l.Address)
                .HasColumnName("Address")
                .IsRequired();

            builder
                .OwnsOne(l => l.BaiduPOI);
        }
    }
}
