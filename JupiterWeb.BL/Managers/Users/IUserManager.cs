using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public interface IUserManager
    {
        IEnumerable<UserReadDTO> GetUsers();
        UserReadDTO GetUser(string id);
        string Add(UserAddDTO user);
        bool Update(UserUpdateDTO user);
        bool Delete(string id);
    }
}
