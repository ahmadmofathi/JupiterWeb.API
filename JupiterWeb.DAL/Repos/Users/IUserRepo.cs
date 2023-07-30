using JupiterWeb.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.DAL.Repos.Users
{
    public interface IUserRepo
    {
        List<User> GetUsers();
        User? GetById(string id);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        int SaveChanges();
    }
}
