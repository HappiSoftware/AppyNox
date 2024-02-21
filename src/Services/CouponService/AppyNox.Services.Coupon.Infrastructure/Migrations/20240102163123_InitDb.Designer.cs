﻿// <auto-generated />
using System;
using AppyNox.Services.Coupon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    [DbContext(typeof(CouponDbContext))]
    [Migration("20240102163123_InitDb")]
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

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Entities.CouponDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Detail")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CouponDetails");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                            Code = "EXF50",
                            Detail = "TestDetail"
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Entities.Coupon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<Guid>("CouponDetailEntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(true)
                        .HasColumnType("character varying(60)");

                    b.Property<string>("Detail")
                        .HasMaxLength(60)
                        .IsUnicode(true)
                        .HasColumnType("character varying(60)");

                    b.Property<double>("DiscountAmount")
                        .HasColumnType("double precision");

                    b.Property<int>("MinAmount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CouponDetailEntityId");

                    b.ToTable("Coupons");

                    b.HasData(
                        new
                        {
                            Id = new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                            Code = "EXF50",
                            CouponDetailEntityId = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                            Description = "Description",
                            Detail = "Detail1",
                            DiscountAmount = 10.65,
                            MinAmount = 100
                        },
                        new
                        {
                            Id = new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                            Code = "EXF60",
                            CouponDetailEntityId = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                            Description = "Description2",
                            Detail = "Detail2",
                            DiscountAmount = 20.550000000000001,
                            MinAmount = 200
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Entities.Coupon", b =>
                {
                    b.HasOne("AppyNox.Services.Coupon.Domain.Entities.CouponDetail", "CouponDetail")
                        .WithMany("Coupons")
                        .HasForeignKey("CouponDetailEntityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CouponDetail");
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Entities.CouponDetail", b =>
                {
                    b.Navigation("Coupons");
                });
#pragma warning restore 612, 618
        }
    }
}
