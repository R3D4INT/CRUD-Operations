using System.Configuration;
using System.Data.Entity;
using back.Models;

namespace back
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDBContext() : base("Server=DESKTOP-U1OM050\\SQLEXPRESS01;Database=Hospital;Trusted_Connection=True;TrustServerCertificate=True")
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}