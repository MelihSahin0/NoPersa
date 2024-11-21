﻿// <auto-generated />
using System;
using MaintenanceService.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaintenanceService.Database.Migrations
{
    [DbContext(typeof(NoPersaDbContext))]
    [Migration("20241121155925_V3")]
    partial class V3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SharedLibrary.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NewPrice")
                        .HasColumnType("integer");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("SharedLibrary.Models.BoxContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BoxContent");
                });

            modelBuilder.Entity("SharedLibrary.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ArticleId")
                        .HasColumnType("integer");

                    b.Property<string>("ContactInformation")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("DefaultNumberOfBoxes")
                        .HasColumnType("integer");

                    b.Property<int>("HolidaysId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

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

                    b.HasIndex("ArticleId");

                    b.HasIndex("HolidaysId")
                        .IsUnique();

                    b.HasIndex("RouteId");

                    b.HasIndex("WorkdaysId")
                        .IsUnique();

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("SharedLibrary.Models.CustomersLightDiet", b =>
                {
                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<int>("LightDietId")
                        .HasColumnType("integer");

                    b.Property<bool>("Selected")
                        .HasColumnType("boolean");

                    b.HasKey("CustomerId", "LightDietId");

                    b.HasIndex("LightDietId");

                    b.ToTable("CustomersLightDiet");
                });

            modelBuilder.Entity("SharedLibrary.Models.CustomersMenuPlan", b =>
                {
                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<int>("BoxContentId")
                        .HasColumnType("integer");

                    b.Property<int>("PortionSizeId")
                        .HasColumnType("integer");

                    b.HasKey("CustomerId", "BoxContentId");

                    b.HasIndex("BoxContentId");

                    b.HasIndex("PortionSizeId");

                    b.ToTable("CustomersMenuPlan");
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

            modelBuilder.Entity("SharedLibrary.Models.DeliveryLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<string>("DeliveryWhishes")
                        .HasColumnType("text");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("DeliveryLocation");
                });

            modelBuilder.Entity("SharedLibrary.Models.Holiday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Day")
                        .HasColumnType("integer");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Holiday");
                });

            modelBuilder.Entity("SharedLibrary.Models.LightDiet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LightDiet");
                });

            modelBuilder.Entity("SharedLibrary.Models.Maintenance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Maintenance");
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

            modelBuilder.Entity("SharedLibrary.Models.PortionSize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PortionSize");
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

            modelBuilder.Entity("SharedLibrary.Models.Weekday", b =>
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

                    b.ToTable("Weekday");
                });

            modelBuilder.Entity("SharedLibrary.Models.Customer", b =>
                {
                    b.HasOne("SharedLibrary.Models.Article", "Article")
                        .WithMany("Customers")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("SharedLibrary.Models.Weekday", "Holidays")
                        .WithOne()
                        .HasForeignKey("SharedLibrary.Models.Customer", "HolidaysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedLibrary.Models.Route", "Route")
                        .WithMany("Customers")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("SharedLibrary.Models.Weekday", "Workdays")
                        .WithOne()
                        .HasForeignKey("SharedLibrary.Models.Customer", "WorkdaysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Holidays");

                    b.Navigation("Route");

                    b.Navigation("Workdays");
                });

            modelBuilder.Entity("SharedLibrary.Models.CustomersLightDiet", b =>
                {
                    b.HasOne("SharedLibrary.Models.Customer", "Customer")
                        .WithMany("CustomersLightDiets")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SharedLibrary.Models.LightDiet", "LightDiet")
                        .WithMany("CustomersLightDiets")
                        .HasForeignKey("LightDietId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("LightDiet");
                });

            modelBuilder.Entity("SharedLibrary.Models.CustomersMenuPlan", b =>
                {
                    b.HasOne("SharedLibrary.Models.BoxContent", "BoxContent")
                        .WithMany("CustomerMenuPlans")
                        .HasForeignKey("BoxContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SharedLibrary.Models.Customer", "Customer")
                        .WithMany("CustomerMenuPlans")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SharedLibrary.Models.PortionSize", "PortionSize")
                        .WithMany("CustomerMenuPlans")
                        .HasForeignKey("PortionSizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoxContent");

                    b.Navigation("Customer");

                    b.Navigation("PortionSize");
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

            modelBuilder.Entity("SharedLibrary.Models.DeliveryLocation", b =>
                {
                    b.HasOne("SharedLibrary.Models.Customer", "Customer")
                        .WithOne("DeliveryLocation")
                        .HasForeignKey("SharedLibrary.Models.DeliveryLocation", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
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

            modelBuilder.Entity("SharedLibrary.Models.Article", b =>
                {
                    b.Navigation("Customers");
                });

            modelBuilder.Entity("SharedLibrary.Models.BoxContent", b =>
                {
                    b.Navigation("CustomerMenuPlans");
                });

            modelBuilder.Entity("SharedLibrary.Models.Customer", b =>
                {
                    b.Navigation("CustomerMenuPlans");

                    b.Navigation("CustomersLightDiets");

                    b.Navigation("DeliveryLocation")
                        .IsRequired();

                    b.Navigation("MonthlyOverviews");
                });

            modelBuilder.Entity("SharedLibrary.Models.LightDiet", b =>
                {
                    b.Navigation("CustomersLightDiets");
                });

            modelBuilder.Entity("SharedLibrary.Models.MonthlyOverview", b =>
                {
                    b.Navigation("DailyOverviews");
                });

            modelBuilder.Entity("SharedLibrary.Models.PortionSize", b =>
                {
                    b.Navigation("CustomerMenuPlans");
                });

            modelBuilder.Entity("SharedLibrary.Models.Route", b =>
                {
                    b.Navigation("Customers");
                });
#pragma warning restore 612, 618
        }
    }
}
