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

        public DbSet<Like> Likes { get; set; }

        public DbSet<Comment> Comments { get; set; }

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
            modelBuilder.Entity<Like>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Comment>()
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
            modelBuilder.Entity<Like>()
              .Property(e => e.Id)
              .ValueGeneratedOnAdd();
            modelBuilder.Entity<Comment>()
             .Property(e => e.Id)
             .ValueGeneratedOnAdd();



            modelBuilder.Entity<User>()
                .HasMany(x => x.Collections)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Item>()
                .HasMany(x => x.Tags)
                .WithOne(x => x.Item)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                 .HasMany(x => x.Like)
                 .WithOne(x => x.Item)
                 .HasForeignKey(x => x.ItemId)
                 .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<Collection>()
                .HasMany(x => x.Items)
                .WithOne(x => x.Collection)
                .HasForeignKey(x => x.CollectionId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Like>()
            //   .HasOne(x => x.Item)
            //   .WithMany(x => x.Like)
            //   .HasForeignKey(x => x.ItemId)
            //   .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<Like>()
              .HasOne(x => x.User)
              .WithOne(x => x.Like);
              
            modelBuilder.Entity<Comment>()
                .HasOne(x => x.Item)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ItemId);
               



            modelBuilder.Entity<Comment>()
                 .HasOne(x => x.User)
                 .WithMany(x => x.Comments)
                 .HasForeignKey(x => x.UserId);
                 


        }
    }
}
