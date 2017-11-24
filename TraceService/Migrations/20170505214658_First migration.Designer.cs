using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Repository;
using Models;

namespace TraceService.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20170505214658_First migration")]
    partial class Firstmigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Models.Trace", b =>
                {
                    b.Property<int>("TraceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CorrelationId");

                    b.Property<string>("Description");

                    b.Property<string>("Details");

                    b.Property<int>("Level");

                    b.Property<string>("Module");

                    b.Property<string>("Object");

                    b.Property<string>("ObjectId");

                    b.Property<string>("Operation");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("TraceDate");

                    b.HasKey("TraceId");

                    b.ToTable("Traces");
                });

            modelBuilder.Entity("Models.TraceOrigin", b =>
                {
                    b.Property<int>("OriginId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("OriginId");

                    b.ToTable("TraceOrigins");
                });
        }
    }
}
