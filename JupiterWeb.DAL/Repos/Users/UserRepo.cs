using JupiterWeb.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.DAL.Repos.Users
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }
        public List<User> GetUsers()
        {
            return _context.Set<User>().ToList();
        }
        public User? GetById(int id)
        {
            return _context.Set<User>().Find(id);
        }
        public void Add(User user)
        {
            _context.Set<User>().Add(user);
        }

        public void Delete(User user)
        {
            _context.Set<User>().Remove(user);
        }
        public void Update(User user)
        {
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }


    }
}
