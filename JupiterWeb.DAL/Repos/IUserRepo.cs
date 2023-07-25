using JupiterWeb.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.DAL
{
    public interface IUserRepo
    {
        List<User> GetUsers();
        User? GetById(int id);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        int SaveChanges();
    }
}
