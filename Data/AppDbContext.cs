using Microsoft.EntityFrameworkCore;
using Project_Management_System.Models;
using Project_Management_System.Models.ManyRelations;

namespace Project_Management_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskUser>()
                .HasKey(tu => new { tu.TaskId, tu.UserId }); // Composite Primary Key


            modelBuilder.Entity<ProjectUser>().HasKey((pu) => new { pu.ProjectId, pu.UserId });

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {

                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }


        }

    }
}
