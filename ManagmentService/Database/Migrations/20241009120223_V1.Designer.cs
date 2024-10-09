﻿// <auto-generated />
using System;
using ManagmentService.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ManagmentService.Database.Migrations
{
    [DbContext(typeof(NoPersaDbContext))]
    [Migration("20241009120223_V1")]
    partial class V1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SharedLibrary.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("Article")
                        .HasColumnType("integer");

                    b.Property<string>("ContactInformation")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("DefaultNumberOfBoxes")
                        .HasColumnType("integer");

                    b.Property<double>("DefaultPrice")
                        .HasColumnType("double precision");

                    b.Property<string>("GeoLocation")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("HolidaysId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int?>("Position")
                        .HasColumnType("integer");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int?>("RouteId")
                        .HasColumnType("integer");

                    b.Property<string>("SerialNumber")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<bool>("TemporaryDelivery")
                        .HasColumnType("boolean");

                    b.Property<bool>("TemporaryNoDelivery")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("WorkdaysId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("HolidaysId")
                        .IsUnique();

                    b.HasIndex("RouteId");

                    b.HasIndex("WorkdaysId")
                        .IsUnique();

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("SharedLibrary.Models.DailyOverview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DayOfMonth")
                        .HasColumnType("integer");

                    b.Property<int>("MonthlyOverviewId")
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfBoxes")
                        .HasColumnType("integer");

                    b.Property<double?>("Price")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("MonthlyOverviewId");

                    b.ToTable("DailyOverview");
                });

            modelBuilder.Entity("SharedLibrary.Models.MonthlyOverview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("MonthlyOverview");
                });

            modelBuilder.Entity("SharedLibrary.Models.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Route");
                });

            modelBuilder.Entity("SharedLibrary.Models.Weekdays", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Friday")
                        .HasColumnType("boolean");

                    b.Property<bool>("Monday")
                        .HasColumnType("boolean");

                    b.Property<bool>("Saturday")
                        .HasColumnType("boolean");

                    b.Property<bool>("Sunday")
                        .HasColumnType("boolean");

                    b.Property<bool>("Thursday")
                        .HasColumnType("boolean");

                    b.Property<bool>("Tuesday")
                        .HasColumnType("boolean");

                    b.Property<bool>("Wednesday")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Weekdays");
                });

            modelBuilder.Entity("SharedLibrary.Models.Customer", b =>
                {
                    b.HasOne("SharedLibrary.Models.Weekdays", "Holidays")
                        .WithOne()
                        .HasForeignKey("SharedLibrary.Models.Customer", "HolidaysId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SharedLibrary.Models.Route", "Route")
                        .WithMany("Customers")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("SharedLibrary.Models.Weekdays", "Workdays")
                        .WithOne()
                        .HasForeignKey("SharedLibrary.Models.Customer", "WorkdaysId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Holidays");

                    b.Navigation("Route");

                    b.Navigation("Workdays");
                });

            modelBuilder.Entity("SharedLibrary.Models.DailyOverview", b =>
                {
                    b.HasOne("SharedLibrary.Models.MonthlyOverview", "MonthlyOverview")
                        .WithMany("DailyOverviews")
                        .HasForeignKey("MonthlyOverviewId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("MonthlyOverview");
                });

            modelBuilder.Entity("SharedLibrary.Models.MonthlyOverview", b =>
                {
                    b.HasOne("SharedLibrary.Models.Customer", "Customer")
                        .WithMany("MonthlyOverviews")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("SharedLibrary.Models.Customer", b =>
                {
                    b.Navigation("MonthlyOverviews");
                });

            modelBuilder.Entity("SharedLibrary.Models.MonthlyOverview", b =>
                {
                    b.Navigation("DailyOverviews");
                });

            modelBuilder.Entity("SharedLibrary.Models.Route", b =>
                {
                    b.Navigation("Customers");
                });
#pragma warning restore 612, 618
        }
    }
}
