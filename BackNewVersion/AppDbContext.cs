using BackNewVersionModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BackNewVersion
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDBContext() : base("AppDBContext")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}