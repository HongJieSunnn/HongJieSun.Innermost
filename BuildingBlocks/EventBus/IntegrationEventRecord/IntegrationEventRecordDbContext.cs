// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
// Modify by HongJieSun 2022

// Framework code of microservices and domain drive design pattern

namespace IntegrationEventRecord
{
    public class IntegrationEventRecordDbContext : DbContext
    {
        public DbSet<IntegrationEventRecordModel> IntegrationEventRecords { get; set; }
        public IntegrationEventRecordDbContext(DbContextOptions<IntegrationEventRecordDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IntegrationEventRecordEntityTypeConfiguration());
        }
    }
}
