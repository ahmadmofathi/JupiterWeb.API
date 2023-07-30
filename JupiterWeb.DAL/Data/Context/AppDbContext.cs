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

            builder.Entity<User>()
        .HasMany(u => u.Requests)
        .WithOne(r => r.UserSentBy)
        .HasForeignKey(r => r.userSentById)
        .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JupiterTask>()
                .HasMany(t => t.Requests)
                .WithOne(r => r.JupiterTask)
                .HasForeignKey(r => r.TaskID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Request>()
                .HasOne(r => r.UserSentBy)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.userSentById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Request>()
                .HasOne(r => r.UserSentTo)
                .WithMany()
                .HasForeignKey(r => r.userSentToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Request>()
                .HasOne(r => r.JupiterTask)
                .WithMany(t => t.Requests)
                .HasForeignKey(r => r.TaskID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<List<string>>().HasNoKey();
            

        }
        public DbSet<User> User => Set<User>();
        public DbSet<JupiterTask> Tasks => Set<JupiterTask>();

        public DbSet<Request> Requests => Set<Request>();
        
    }
}