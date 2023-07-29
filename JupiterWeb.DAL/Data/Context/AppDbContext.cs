using JupiterWeb.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace JupiterWeb.API.Data
{
    public class AppDbContext :IdentityDbContext<User, IdentityRole<string>, string>
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.WhatsApp)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Branch)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .IsUnique();

            });
            builder.Entity<JupiterTask>(entity =>
            {
                entity.ToTable("JupiterTask");

                entity.Property(e => e.Name)
                                    .HasMaxLength(100)
                                    .IsRequired();

                entity.Property(e => e.IsDone);

                entity.Property(e => e.TaskPoints)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Link)
                    .HasMaxLength(500);

                entity.Property(e => e.Deadline)
                    .HasColumnType("datetime2");

                entity.HasOne(e => e.UserAssignedBy)
                    .WithMany(e => e.AssignedByTasks)
                    .HasForeignKey(e => e.AssignedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UserAssignedTo)
                    .WithMany(e => e.AssignedToTasks)
                    .HasForeignKey(e => e.AssignedToId)
                    .OnDelete(DeleteBehavior.Restrict);
            
            });
        }
        public DbSet<User> User => Set<User>();
        public DbSet<JupiterTask> Task => Set<JupiterTask>();
        
    }
}