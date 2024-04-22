﻿// <auto-generated />
using System;
using AppyNox.Services.Coupon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    [DbContext(typeof(CouponDbContext))]
    partial class CouponDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AppyNox.Services.Base.Domain.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RetryCount")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Coupons.Coupon", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<Guid>("CouponDetailId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(true)
                        .HasColumnType("character varying(60)");

                    b.Property<string>("Detail")
                        .HasMaxLength(60)
                        .IsUnicode(true)
                        .HasColumnType("character varying(60)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CouponDetailId");

                    b.HasIndex("CreationDate");

                    b.HasIndex("UpdateDate");

                    b.ToTable("Coupons");

                    b.HasData(
                        new
                        {
                            Id = new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                            Code = "EXF50",
                            CouponDetailId = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                            CreatedBy = "System",
                            CreationDate = new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Description",
                            Detail = "Detail1"
                        },
                        new
                        {
                            Id = new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                            Code = "EXF60",
                            CouponDetailId = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                            CreatedBy = "System",
                            CreationDate = new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Description2",
                            Detail = "Detail2"
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Coupons.CouponDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Detail")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreationDate");

                    b.HasIndex("UpdateDate");

                    b.ToTable("CouponDetails");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                            Code = "EXD10",
                            CreatedBy = "System",
                            CreationDate = new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc),
                            Detail = "TestDetail"
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Coupons.CouponDetailTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CouponDetailId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CouponDetailId");

                    b.HasIndex("CreationDate");

                    b.HasIndex("UpdateDate");

                    b.ToTable("CouponDetailTags");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                            CouponDetailId = new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                            CreatedBy = "System",
                            CreationDate = new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc),
                            Tag = "Tag Description"
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Coupons.CouponHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CouponId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MinimumAmount")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CouponId");

                    b.HasIndex("CreationDate");

                    b.HasIndex("UpdateDate");

                    b.ToTable("CouponHistories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d9bd5f7e-990a-4260-8acf-3015bfa241d5"),
                            CouponId = new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                            CreatedBy = "System",
                            CreationDate = new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc),
                            Date = new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc),
                            MinimumAmount = 100
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Entities.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreationDate");

                    b.HasIndex("UpdateDate");

                    b.ToTable("Tickets");

                    b.HasData(
                        new
                        {
                            Id = new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                            Content = "Ticket content",
                            CreatedBy = "System",
                            CreationDate = new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc),
                            ReportDate = new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc),
                            Title = "Title"
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Entities.TicketTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketTags");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6125498b-ca83-4d9f-ae4d-55b97d98b47d"),
                            Description = "Tag Description",
                            TicketId = new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0")
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Coupons.Coupon", b =>
                {
                    b.HasOne("AppyNox.Services.Coupon.Domain.Coupons.CouponDetail", "CouponDetail")
                        .WithMany()
                        .HasForeignKey("CouponDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("AppyNox.Services.Coupon.Domain.Coupons.Amount", "Amount", b1 =>
                        {
                            b1.Property<Guid>("CouponId")
                                .HasColumnType("uuid");

                            b1.Property<double>("DiscountAmount")
                                .HasColumnType("double precision");

                            b1.Property<int>("MinAmount")
                                .HasColumnType("integer");

                            b1.HasKey("CouponId");

                            b1.ToTable("Coupons");

                            b1.WithOwner()
                                .HasForeignKey("CouponId");

                            b1.HasData(
                                new
                                {
                                    CouponId = new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                                    DiscountAmount = 10.65,
                                    MinAmount = 100
                                },
                                new
                                {
                                    CouponId = new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                                    DiscountAmount = 10.65,
                                    MinAmount = 100
                                });
                        });

                    b.Navigation("Amount")
                        .IsRequired();

                    b.Navigation("CouponDetail");
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Coupons.CouponDetailTag", b =>
                {
                    b.HasOne("AppyNox.Services.Coupon.Domain.Coupons.CouponDetail", null)
                        .WithMany("CouponDetailTags")
                        .HasForeignKey("CouponDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Coupons.CouponHistory", b =>
                {
                    b.HasOne("AppyNox.Services.Coupon.Domain.Coupons.Coupon", null)
                        .WithMany("Histories")
                        .HasForeignKey("CouponId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Entities.TicketTag", b =>
                {
                    b.HasOne("AppyNox.Services.Coupon.Domain.Entities.Ticket", "Ticket")
                        .WithMany("Tags")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Coupons.Coupon", b =>
                {
                    b.Navigation("Histories");
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Coupons.CouponDetail", b =>
                {
                    b.Navigation("CouponDetailTags");
                });

            modelBuilder.Entity("AppyNox.Services.Coupon.Domain.Entities.Ticket", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
