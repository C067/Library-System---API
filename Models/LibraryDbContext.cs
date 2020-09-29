using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_2_PRO670.Models
{
    public class LibraryDbContext : DbContext
    {
        //DbSet's for both Tables in the database. Library and Book
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Book> Books { get; set; }

        //Configure the database to use the library.db file
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source = library.db");
    }
}
