namespace IntegrationEventRecord
{
    public class IntegrationEventRecordEntityTypeConfiguration : IEntityTypeConfiguration<IntegrationEventRecordModel>
    {
        public void Configure(EntityTypeBuilder<IntegrationEventRecordModel> builder)
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

            //transactionid can be empty
        }
    }
}
