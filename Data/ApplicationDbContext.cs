using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyBookshelfApp.Models;

namespace MyBookshelfApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplicationUser user = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794577",
                Id = "00000000-ffff-ffff-ffff-ffffffffffff"
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            modelBuilder.Entity<Genre>().HasData(
                           new Genre()
                           {
                               Id = 1,
                               Title = "Fantasy"
                           },
                           new Genre()
                           {
                               Id = 2,
                               Title = "Sci-Fi"
                           },
                           new Genre()
                           {
                               Id = 3,
                               Title = "Mystery"
                           },
                            new Genre()
                            {
                                Id = 4,
                                Title = "Romance"
                            },
                            new Genre()
                            {
                                Id = 5,
                                Title = "Historical"
                            },
                            new Genre()
                            {
                                Id = 6,
                                Title = "Non-Fiction"
                            },
                            new Genre()
                            {
                                Id = 7,
                                Title = "Fiction"
                            }
                       );

        }

    }

}
