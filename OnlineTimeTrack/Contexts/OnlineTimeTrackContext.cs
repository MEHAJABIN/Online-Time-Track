using Microsoft.EntityFrameworkCore;
using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Contexts
{
    public class OnlineTimeTrackContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }

        public OnlineTimeTrackContext(DbContextOptions<OnlineTimeTrackContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder md)
        {
            md.Entity<User>().ToTable("Users");
            md.Entity<Project>().ToTable("Projects");

            md.Entity<User>().HasData(new User
            {
               UserID = 3,
               FullName = "Uncle",
               Email = "uncle.bob@gmail.com",
               Dob= new DateTime(1979, 04, 25),
               ContactNumber = "999-888-7777"

            }, new User
            {
                UserID = 4,
                FullName = "Jan",
                Email = "jan.kirsten@gmail.com",
                Dob = new DateTime(1981, 07, 13),
                ContactNumber = "111-222-3333"

            });
        }
    }
}




