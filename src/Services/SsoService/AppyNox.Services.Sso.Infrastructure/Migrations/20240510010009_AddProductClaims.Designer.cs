﻿// <auto-generated />
using System;
using AppyNox.Services.Sso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AppyNox.Services.Sso.Infrastructure.Migrations
{
    [DbContext(typeof(IdentityDatabaseContext))]
    [Migration("20240510010009_AddProductClaims")]
    partial class AddProductClaims
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AppyNox.Services.Sso.Domain.Entities.ApplicationRole", b =>
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
                        },
                        new
                        {
                            Id = new Guid("4d0f77eb-2ad5-4b43-848e-826cd32d684b"),
                            Code = "Role3",
                            CompanyId = new Guid("0ebae1bf-6610-4967-a8ed-b149219caf68"),
                            Description = "RoleDescription",
                            Name = "NotAdmin",
                            NormalizedName = "NOTADMIN"
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Sso.Domain.Entities.ApplicationUser", b =>
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

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

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

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
                            ConcurrencyStamp = "22983e7e-7b0c-4f6a-9f2d-15b29bea4c0b",
                            Email = "admin@email.com",
                            EmailConfirmed = true,
                            IsAdmin = true,
                            LockoutEnabled = false,
                            Name = "Name1",
                            NormalizedEmail = "ADMIN@EMAIL.COM",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEDWddlD4GknyLTTEs3lk/gv7traAFYlK6AGy7r3VNVjjEeX+6kLq1n3wmwFfcGrFkw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "6507632d-22d1-47e4-bd52-d913ea933fd8",
                            Surname = "Surname1",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        },
                        new
                        {
                            Id = new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                            AccessFailedCount = 0,
                            Code = "USR02",
                            CompanyId = new Guid("0ebae1bf-6610-4967-a8ed-b149219caf68"),
                            ConcurrencyStamp = "497c55b0-62da-4921-9345-b58b82bd029c",
                            Email = "sadmin@email.com",
                            EmailConfirmed = true,
                            IsAdmin = true,
                            LockoutEnabled = false,
                            Name = "Name2",
                            NormalizedEmail = "SADMIN@EMAIL.COM",
                            NormalizedUserName = "SUPERADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAELIkY6pXxeKuXd7ew84/YcKeqb/o5iJB8wsQz+QB6BpCHfHC0oPlqZgan3FEsyml5A==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "75362c3d-0a90-4699-9ad0-40e165470f4c",
                            Surname = "Surname2",
                            TwoFactorEnabled = false,
                            UserName = "superadmin"
                        },
                        new
                        {
                            Id = new Guid("2c0e21cf-4845-4b0f-a653-6b7c414af2f9"),
                            AccessFailedCount = 0,
                            Code = "USR03",
                            CompanyId = new Guid("0ebae1bf-6610-4967-a8ed-b149219caf68"),
                            ConcurrencyStamp = "2e373f72-2fc9-4a34-86d0-fde55a9da142",
                            Email = "test3@happisoft.com",
                            EmailConfirmed = true,
                            IsAdmin = false,
                            LockoutEnabled = false,
                            Name = "Name3",
                            NormalizedEmail = "TEST3@HAPPISOFT.COM",
                            NormalizedUserName = "TESTUSER3",
                            PasswordHash = "AQAAAAIAAYagAAAAENuCjXeryRGU+1fBWsWdIj7+T5zuOyWbxsZULjz89iIxl4V8NEKq8xa+12X3v718DA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "4a2fcd07-8517-43c7-b4e2-23c81c408fe6",
                            Surname = "Surname3",
                            TwoFactorEnabled = false,
                            UserName = "TestUser3"
                        });
                });

            modelBuilder.Entity("AppyNox.Services.Sso.Domain.Entities.Company", b =>
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
                            ClaimType = "API.Permission",
                            ClaimValue = "Users.View",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "API.Permission",
                            ClaimValue = "Users.Create",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 3,
                            ClaimType = "API.Permission",
                            ClaimValue = "Users.Edit",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "API.Permission",
                            ClaimValue = "Users.Delete",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 5,
                            ClaimType = "API.Permission",
                            ClaimValue = "Roles.View",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 6,
                            ClaimType = "API.Permission",
                            ClaimValue = "Roles.Create",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 7,
                            ClaimType = "API.Permission",
                            ClaimValue = "Roles.Edit",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 8,
                            ClaimType = "API.Permission",
                            ClaimValue = "Roles.Delete",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 9,
                            ClaimType = "API.Permission",
                            ClaimValue = "Roles.AssignPermission",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 10,
                            ClaimType = "API.Permission",
                            ClaimValue = "Roles.WithdrawPermission",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 11,
                            ClaimType = "API.Permission",
                            ClaimValue = "Coupons.View",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 12,
                            ClaimType = "API.Permission",
                            ClaimValue = "Coupons.Create",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 13,
                            ClaimType = "API.Permission",
                            ClaimValue = "Coupons.Edit",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 14,
                            ClaimType = "API.Permission",
                            ClaimValue = "Coupons.Delete",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 15,
                            ClaimType = "API.Permission",
                            ClaimValue = "Licenses.View",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 16,
                            ClaimType = "API.Permission",
                            ClaimValue = "Licenses.Create",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 17,
                            ClaimType = "API.Permission",
                            ClaimValue = "Licenses.Edit",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 18,
                            ClaimType = "API.Permission",
                            ClaimValue = "Licenses.Delete",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 20,
                            ClaimType = "API.Permission",
                            ClaimValue = "Products.View",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 21,
                            ClaimType = "API.Permission",
                            ClaimValue = "Products.Create",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 22,
                            ClaimType = "API.Permission",
                            ClaimValue = "Products.Edit",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 23,
                            ClaimType = "API.Permission",
                            ClaimValue = "Products.Delete",
                            RoleId = new Guid("e24e99e7-00e4-4007-a042-565eac12d96d")
                        },
                        new
                        {
                            Id = 19,
                            ClaimType = "API.Permission",
                            ClaimValue = "Coupons.View",
                            RoleId = new Guid("4d0f77eb-2ad5-4b43-848e-826cd32d684b")
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
                        },
                        new
                        {
                            UserId = new Guid("2c0e21cf-4845-4b0f-a653-6b7c414af2f9"),
                            RoleId = new Guid("4d0f77eb-2ad5-4b43-848e-826cd32d684b")
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

            modelBuilder.Entity("AppyNox.Services.Sso.Domain.Entities.ApplicationRole", b =>
                {
                    b.HasOne("AppyNox.Services.Sso.Domain.Entities.Company", "Company")
                        .WithMany("Roles")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("AppyNox.Services.Sso.Domain.Entities.ApplicationUser", b =>
                {
                    b.HasOne("AppyNox.Services.Sso.Domain.Entities.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Sso.Domain.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Sso.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Sso.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Sso.Domain.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppyNox.Services.Sso.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("AppyNox.Services.Sso.Domain.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AppyNox.Services.Sso.Domain.Entities.Company", b =>
                {
                    b.Navigation("Roles");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}