using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Collection> Collections { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

           

        //    modelBuilder.Entity<User>()
        //        .HasMany(x => x.Collections)
        //        .WithOne(x => x.User);

            
        //}
    }
}
