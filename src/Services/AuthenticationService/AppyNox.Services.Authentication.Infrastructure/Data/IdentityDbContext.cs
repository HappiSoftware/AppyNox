﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppyNox.Services.Authentication.Infrastructure.Data
{
    public class IdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public IdentityDbContext()
        {

        }
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Get environment
                string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                Console.WriteLine($"Environment: {environment}");
                // Build config
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AppyNox.Services.Authentication.WebAPI"))
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
                var connectionString = config.GetConnectionString("Default");
                Console.WriteLine($"Connection String: {connectionString}");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<IdentityUser>();

            string adminUserId = Guid.NewGuid().ToString();
            string adminRoleId = Guid.NewGuid().ToString();
            // Create Roles
            var adminRole = new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" };
            modelBuilder.Entity<IdentityRole>().HasData(adminRole);

            // Create Users
            var adminUser = new IdentityUser { Id = adminUserId, UserName = "admin", NormalizedUserName = "ADMIN", Email = "admin@email.com", NormalizedEmail = "ADMIN@EMAIL.COM", EmailConfirmed = true, PasswordHash = hasher.HashPassword(null, "Admin@123") };
            modelBuilder.Entity<IdentityUser>().HasData(adminUser);

            // Add Role Claims
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(
                new IdentityRoleClaim<string> { Id = 1, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Users.View" },
                new IdentityRoleClaim<string> { Id = 2, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Create" },
                new IdentityRoleClaim<string> { Id = 3, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Edit" },
                new IdentityRoleClaim<string> { Id = 4, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Delete" },

                new IdentityRoleClaim<string> { Id = 5, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.View" },
                new IdentityRoleClaim<string> { Id = 6, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.Create" },
                new IdentityRoleClaim<string> { Id = 7, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.Edit" },
                new IdentityRoleClaim<string> { Id = 8, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.Delete" },
                new IdentityRoleClaim<string> { Id = 9, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.AssignPermission" },
                new IdentityRoleClaim<string> { Id = 10, RoleId = adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.WithdrawPermission" }
            );

            // Add User Roles
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = adminRoleId, UserId = adminUserId }
            );
        }
    }
}
