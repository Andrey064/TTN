using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTNAppCore.Model;

namespace TTNAppCore.DataAccess
{
    public class TTNDbContext : DbContext
    {
        public TTNDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = TTN.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ttn>()
                .Property(f => f.Num)
                .IsRequired();

            //modelBuilder.Entity<Ttn>()
            //    .Property(f => f.Driver)
            //    .IsRequired();
        }

        public DbSet<Ttn> Ttns { get; set; }
        public DbSet<Driver> Drivers { get; set; }
    }
}
