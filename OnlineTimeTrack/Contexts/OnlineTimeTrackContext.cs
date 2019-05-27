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

         

        }

        internal static Task AddAsync(object project)
        {
            throw new NotImplementedException();
        }
    }
}




