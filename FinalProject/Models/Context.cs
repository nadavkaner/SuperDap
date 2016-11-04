using System.Data.Entity;
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
                .Entity<Computer>()
                .HasRequired<Store>(s => s.Store)
                .WithMany(s => s.Computers)
                .HasForeignKey(s => s.StoreId)
                .WillCascadeOnDelete();
            modelBuilder
                .Entity<Store>()
                .HasOptional(x => x.MostPopularComputer);

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }

        public DbSet<Computer> Computers { get; set; }
        public DbSet<Store> Stores { get; set; }
    }
}