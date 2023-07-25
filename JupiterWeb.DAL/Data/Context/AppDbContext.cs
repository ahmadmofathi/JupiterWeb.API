using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        }
        public DbSet<User> User => Set<User>();
        
    }
}