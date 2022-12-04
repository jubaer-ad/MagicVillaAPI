﻿// <auto-generated />
using System;
using MagicVillaAPI.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVillaAPI.Migrations
{
    [DbContext(typeof(VillaDBContext))]
    [Migration("20221128042027_AddMoreData")]
    partial class AddMoreData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVillaAPI.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Occupency")
                        .HasColumnType("int");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<int>("Sqft")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateddAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenity = "Test Amenity",
                            CreatedAt = new DateTime(2022, 11, 28, 10, 20, 27, 72, DateTimeKind.Local).AddTicks(7615),
                            Details = "Test details",
                            ImageUrl = "https://images.pexels.com/photos/7583935/pexels-photo-7583935.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                            Name = "A Villa",
                            Occupency = 2,
                            Rate = 5.0,
                            Sqft = 750,
                            UpdateddAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            Amenity = "Test002 Amenity",
                            CreatedAt = new DateTime(2022, 11, 28, 10, 20, 27, 72, DateTimeKind.Local).AddTicks(7625),
                            Details = "Test002 details",
                            ImageUrl = "https://images.pexels.com/photos/7583935/pexels-photo-7583935.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                            Name = "B Villa",
                            Occupency = 4,
                            Rate = 8.0,
                            Sqft = 1050,
                            UpdateddAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 10,
                            Amenity = "drrrrrrrrrrrrrrrrrrrrrrr",
                            CreatedAt = new DateTime(2022, 11, 28, 10, 20, 27, 72, DateTimeKind.Local).AddTicks(7627),
                            Details = "hrrrrrrrrrrrrrrr",
                            ImageUrl = "https://images.pexels.com/photos/7583935/pexels-photo-7583935.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                            Name = "grrrrrrrrrrr",
                            Occupency = 4,
                            Rate = 8.0,
                            Sqft = 1050,
                            UpdateddAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
