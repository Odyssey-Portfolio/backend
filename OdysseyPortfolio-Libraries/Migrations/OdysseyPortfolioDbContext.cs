using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OdysseyPortfolio_Libraries.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Migrations
{
    public class OdysseyPortfolioDbContext : IdentityDbContext<User>
    {
        public OdysseyPortfolioDbContext(DbContextOptions<OdysseyPortfolioDbContext> options)
                : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Initializes default values of the Identity Entity (User). 
            //This includes Id.
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.HasMany(e => e.Blogs)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired(false);
                user.HasMany(e => e.Comments)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired(false);
                user.HasMany(e => e.CommentLikes)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired(false);
            });

            modelBuilder.Entity<Blog>(blog =>
            {
                blog.HasMany(e => e.Comments)
                    .WithOne(e => e.Blog)
                    .HasForeignKey(e => e.BlogId)
                    .IsRequired(false);
                blog.HasMany(e => e.Comments)
                    .WithOne(e => e.Blog)
                    .HasForeignKey(e => e.BlogId)
                    .IsRequired(false);
            });

            modelBuilder.Entity<Comment>(blog =>
            {
                blog.HasMany(e => e.CommentLikes)
                    .WithOne(e => e.Comment)
                    .HasForeignKey(e => e.CommentId)
                    .IsRequired(false);
            });
        }
    }
}