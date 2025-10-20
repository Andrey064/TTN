using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTNApp
{
    public class ApplicationContext :  DbContext
    {
        //public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        //{
        //    Database.EnsureCreated();
        //}

        public DbSet<Ttn> Ttns { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = .\\TTN.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ttn>().HasData(GetTTns());
            base.OnModelCreating(modelBuilder);
        }

        private Ttn[] GetTTns()
        {
            return new Ttn[]
            {
            new Ttn { Id = 1, Num = "111", Amount = 25, ProductName = "Щебень",  Car="MAN"},
            new Ttn { Id = 2, Num = "222", Amount = 25, ProductName = "Щебень",  Car="Volvo"},
            new Ttn { Id = 3, Num = "333", Amount = 25, ProductName = "Щебень",  Car="Kamaz"},
            new Ttn { Id = 4, Num = "444", Amount = 25, ProductName = "Щебень",  Car="Iveco"},
            };
        }
    }
}
