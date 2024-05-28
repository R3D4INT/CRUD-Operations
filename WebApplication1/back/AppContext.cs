using System.Configuration;
using System.Data.Entity;
using back.Models;

namespace back
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDBContext() : base(ConfigurationManager.ConnectionStrings["HospitalDbConnectionString"].ConnectionString)
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}