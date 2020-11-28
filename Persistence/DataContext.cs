using System;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Persistence
{
    public class DataContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<List> Lists { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var barry = new User { Id = new Guid("8ef3044c-7254-414c-8e79-9971d672091b"), Name = "Barry", Email = "Barry@gmail.com", Password = "newpassword"};
            var roboto = new User { Id = new Guid("dd93ffaa-97df-44e3-81e3-5caf8c42abc2"), Name = "Mr. Roboto", Email = "roboto@gmail.com", Password = "youllneverguessthis"};
            modelBuilder.Entity<User>().HasData(
                barry,
                roboto
            );
            // modelBuilder.Entity<List>().HasData(
            //     new List { _Id = new Guid("d699600b-594c-4eec-ab1e-80e4755c2fce"), User = barry, Title = "First List" },
            //     new List { _Id = new Guid("f3a6c2eb-b9ba-4c76-b9fd-34072fc9b40c"), User = barry, Title = "Second List" }
            // );
        }

    }
}
