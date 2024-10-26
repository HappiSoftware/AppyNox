﻿// <auto-generated />
using System;
using AppyNox.Services.License.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AppyNox.Services.License.Infrastructure.Migrations
{
    [DbContext(typeof(LicenseDatabaseContext))]
    [Migration("20240103213334_InitDb")]
    partial class InitDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AppyNox.Services.License.Domain.Entities.LicenseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(true)
                        .HasColumnType("character varying(60)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LicenseKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MaxUsers")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Licenses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                            Code = "LK001",
                            CompanyId = new Guid("221e8b2c-59d5-4e5b-b010-86c239b66738"),
                            Description = "License Description",
                            ExpirationDate = new DateTime(2025, 1, 2, 21, 33, 34, 534, DateTimeKind.Utc).AddTicks(4202),
                            LicenseKey = "7f033381-fbf7-4929-b5f7-c64261b20bf3",
                            MaxUsers = 3
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
