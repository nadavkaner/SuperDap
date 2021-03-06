﻿using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalProject.Models
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public Context()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DevelopmentTool>()
                .HasRequired(s => s.Company)
                .WithMany(s => s.DevelopmentTools)
                .WillCascadeOnDelete();
            modelBuilder
                .Entity<DevelopmentTool>()
                .HasMany(x => x.Comments);
            modelBuilder
                .Entity<Company>()
                .HasOptional(x => x.MostPopularDevelopmentTool);
            modelBuilder
                .Entity<Company>()
                .HasMany(x => x.RevenuePerYears);
            modelBuilder
                .Entity<Comment>()
                .HasRequired(x => x.User);
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }

        public DbSet<DevelopmentTool> DevelopmentTools { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}