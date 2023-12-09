using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Tasky.Models;

namespace Tasky.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Project>? Projects { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
        public DbSet<ApplicationUserProject>? ApplicationUserProjects { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserProject>()
                .HasKey(e => new { e.UserId, e.ProjectId });

            modelBuilder.Entity<ApplicationUserProject>()
                .HasOne(e => e.User)
                .WithMany(e => e.Projects)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<ApplicationUserProject>()
                .HasOne(e => e.Project)
                .WithMany(e => e.ApplicationUsers)
                .HasForeignKey(e => e.ProjectId);

        }
    }
}