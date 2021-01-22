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
       // public DbSet<Topic> Topics { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Collection>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Item>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Tag>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Collection>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Item>()
               .Property(e => e.Id)
               .ValueGeneratedOnAdd();
            modelBuilder.Entity<Tag>()
              .Property(e => e.Id)
              .ValueGeneratedOnAdd();


            modelBuilder.Entity<User>()
                .HasMany(x => x.Collections)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

           modelBuilder.Entity<Item>()
                .HasMany(x => x.Tags)
                .WithOne(x => x.Item)
                .HasForeignKey(x => x.ItemId);

        }
    }
}
