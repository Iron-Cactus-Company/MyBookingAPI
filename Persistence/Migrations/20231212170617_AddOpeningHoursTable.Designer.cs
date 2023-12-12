﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231212170617_AddOpeningHoursTable")]
    partial class AddOpeningHoursTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.BusinessProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BusinessProfile");
                });

            modelBuilder.Entity("Domain.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AcceptingTimeText")
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<Guid?>("BusinessProfileId")
                        .HasColumnType("uuid");

                    b.Property<string>("CancellingTimeText")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BusinessProfileId");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Domain.OpeningHours", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<int>("FridayEnd")
                        .HasColumnType("integer");

                    b.Property<int>("FridayStart")
                        .HasColumnType("integer");

                    b.Property<int>("MondayEnd")
                        .HasColumnType("integer");

                    b.Property<int>("MondayStart")
                        .HasColumnType("integer");

                    b.Property<int>("SaturdayEnd")
                        .HasColumnType("integer");

                    b.Property<int>("SaturdayStart")
                        .HasColumnType("integer");

                    b.Property<int>("SundayEnd")
                        .HasColumnType("integer");

                    b.Property<int>("SundayStart")
                        .HasColumnType("integer");

                    b.Property<int>("ThursdayEnd")
                        .HasColumnType("integer");

                    b.Property<int>("ThursdayStart")
                        .HasColumnType("integer");

                    b.Property<int>("TuesdayEnd")
                        .HasColumnType("integer");

                    b.Property<int>("TuesdayStart")
                        .HasColumnType("integer");

                    b.Property<int>("WednesdayEnd")
                        .HasColumnType("integer");

                    b.Property<int>("WednesdayStart")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId")
                        .IsUnique();

                    b.ToTable("OpeningHours");
                });

            modelBuilder.Entity("Domain.Company", b =>
                {
                    b.HasOne("Domain.BusinessProfile", "BusinessProfile")
                        .WithMany("Companies")
                        .HasForeignKey("BusinessProfileId");

                    b.Navigation("BusinessProfile");
                });

            modelBuilder.Entity("Domain.OpeningHours", b =>
                {
                    b.HasOne("Domain.Company", "Company")
                        .WithOne("OpeningHours")
                        .HasForeignKey("Domain.OpeningHours", "CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Domain.BusinessProfile", b =>
                {
                    b.Navigation("Companies");
                });

            modelBuilder.Entity("Domain.Company", b =>
                {
                    b.Navigation("OpeningHours");
                });
#pragma warning restore 612, 618
        }
    }
}
