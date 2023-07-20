using Microsoft.EntityFrameworkCore;
using JupiterWeb.API.Data.Models;

namespace JupiterWeb.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=JupiterDB;Trusted_Connection=True;");
            }
        }
        public DbSet<User> Users { get; set; }
        
    }
}