﻿// <auto-generated />
using System;
using IntegrationEventRecord;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Innermost.LogLife.API.Migrations.IntegrationEventRecord
{
    [DbContext(typeof(IntegrationEventRecordDbContext))]
    [Migration("20220517122625_InitIntegrationEventRecordDbContext")]
    partial class InitIntegrationEventRecordDbContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("IntegrationEventRecord.IntegrationEventRecordModel", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EventContent")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("TimesSend")
                        .HasColumnType("int");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EventId");

                    b.ToTable("IntegrationEventRecords", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}