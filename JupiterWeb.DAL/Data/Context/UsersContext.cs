using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.DAL
{
    public class UsersContext : IdentityDbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) :base(options) { 
        
        }
    }
}
