using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationEventServiceSQL.EntityFrameworkConfigurations
{
    public class IntegrationEventSQLModelEntityTypeConfiguration : IEntityTypeConfiguration<IntegrationEventSQLModel>
    {
        public void Configure(EntityTypeBuilder<IntegrationEventSQLModel> builder)
        {
            builder.ToTable("IntegrationEventRecords");

            builder.HasKey(i => i.EventId);

            builder
                .Property(i => i.EventContent)
                .IsRequired();

            builder
                .Property(i => i.State)
                .IsRequired();

            builder
                .Property(i => i.CreateTime)
                .IsRequired();

            builder
                .Property(i => i.EventTypeName)
                .IsRequired();

            builder
                .Property(i => i.TimesSend)
                .IsRequired();

            builder
                .Property(i => i.CreateTime)
                .IsRequired();

            builder
                .Property(i => i.TransactionId)
                .IsRequired(false);
        }
    }
}
