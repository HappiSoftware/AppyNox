﻿// <auto-generated />
using System;
using AppyNox.Services.Authentication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AppyNox.Services.Authentication.Infrastructure.Migrations
{
    [DbContext(typeof(IdentityDatabaseContext))]
    partial class IdentityDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AppyNox.Services.Authentication.Domain.Entities.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d"),
                            Code = "Role1",
                            CompanyId = new Guid("221e8b2c-59d5-4e5b-b010-86c239b66738"),
                            Description = "RoleDescription",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("f51a5d58-ff38-4563-9d32-f658ef2b40d0"),
                            Code = "Role2",
                            CompanyId = new Guid("0ebae1bf-6610-4967-a8ed-b149219caf68"),
                            Description = "RoleDescription",
                            Name = "SuperAdmin",
                            NormalizedName = "SUPERADMIN"
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Authentication.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                            AccessFailedCount = 0,
                            Code = "USR01",
                            CompanyId = new Guid("221e8b2c-59d5-4e5b-b010-86c239b66738"),
                            ConcurrencyStamp = "e1e78e12-f634-4f3e-8973-675452023739",
                            Email = "admin@email.com",
                            EmailConfirmed = true,
                            IsAdmin = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@EMAIL.COM",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEFVj9ae9qPDDKSly+xKcHHzLOwncZc2VKk6w1R/+xn6qzfD3zG0jDNiEk/y7OCm/uw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "6c411693-c21c-4321-845b-d56b7babcf1a",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        },
                        new
                        {
                            Id = new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                            AccessFailedCount = 0,
                            Code = "USR02",
                            CompanyId = new Guid("0ebae1bf-6610-4967-a8ed-b149219caf68"),
                            ConcurrencyStamp = "d22d41ec-6dcd-4407-aaa1-dcdee94dc7a4",
                            Email = "sadmin@email.com",
                            EmailConfirmed = true,
                            IsAdmin = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "SADMIN@EMAIL.COM",
                            NormalizedUserName = "SUPERADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAENtL76hfjxVVqMzyYs2TzMHGGwZ7LvhoE2SRci3uk7AZSyQLFpA8is9VkaUqq2kR7g==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "6bc7b617-b645-45a8-a168-73eceeb18af4",
                            TwoFactorEnabled = false,
                            UserName = "superadmin"
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Authentication.Domain.Entities.CompanyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0ebae1bf-6610-4967-a8ed-b149219caf68"),
                            Code = "",
                            Name = "HappiSoft"
                        },
                        new
                        {
                            Id = new Guid("221e8b2c-59d5-4e5b-b010-86c239b66738"),
                            Code = "",
                            Name = "TestCompany"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "Permission",
                            ClaimValue = "Users.View",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "Permission",
                            ClaimValue = "Users.Create",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 3,
                            ClaimType = "Permission",
                            ClaimValue = "Users.Edit",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "Permission",
                            ClaimValue = "Users.Delete",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 5,
                            ClaimType = "Permission",
                            ClaimValue = "Roles.View",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 6,
                            ClaimType = "Permission",
                            ClaimValue = "Roles.Create",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 7,
                            ClaimType = "Permission",
                            ClaimValue = "Roles.Edit",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 8,
                            ClaimType = "Permission",
                            ClaimValue = "Roles.Delete",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 9,
                            ClaimType = "Permission",
                            ClaimValue = "Roles.AssignPermission",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 10,
                            ClaimType = "Permission",
                            ClaimValue = "Roles.WithdrawPermission",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 11,
                            ClaimType = "Permission",
                            ClaimValue = "Coupons.View",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 12,
                            ClaimType = "Permission",
                            ClaimValue = "Coupons.Create",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 13,
                            ClaimType = "Permission",
                            ClaimValue = "Coupons.Edit",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 14,
                            ClaimType = "Permission",
                            ClaimValue = "Coupons.Delete",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 15,
                            ClaimType = "Permission",
                            ClaimValue = "Licenses.View",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 16,
                            ClaimType = "Permission",
                            ClaimValue = "Licenses.Create",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 17,
                            ClaimType = "Permission",
                            ClaimValue = "Licenses.Edit",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 18,
                            ClaimType = "Permission",
                            ClaimValue = "Licenses.Delete",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            UserId = new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                            RoleId = new Guid("f51a5d58-ff38-4563-9d32-f658ef2b40d0")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("AppyNox.Services.Authentication.Domain.Entities.ApplicationRole", b =>
                {
                    b.HasOne("AppyNox.Services.Authentication.Domain.Entities.CompanyEntity", "Company")
                        .WithMany("Roles")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("AppyNox.Services.Authentication.Domain.Entities.ApplicationUser", b =>
                {
                    b.HasOne("AppyNox.Services.Authentication.Domain.Entities.CompanyEntity", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Authentication.Domain.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Authentication.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Authentication.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Authentication.Domain.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppyNox.Services.Authentication.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Authentication.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AppyNox.Services.Authentication.Domain.Entities.CompanyEntity", b =>
                {
                    b.Navigation("Roles");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
