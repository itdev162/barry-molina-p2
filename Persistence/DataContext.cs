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
            modelBuilder.Entity<User>().HasData(
                new User { Id = new Guid("8ef3044c-7254-414c-8e79-9971d672091b"), Name = "Barry", Email = "Barry@gmail.com", Password = "newpassword"},
                new User { Id = new Guid("dd93ffaa-97df-44e3-81e3-5caf8c42abc2"), Name = "Mr. Roboto", Email = "roboto@gmail.com", Password = "youllneverguessthis"}
            );
        }

    }
}
