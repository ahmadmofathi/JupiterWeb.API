using JupiterWeb.API.Data;
using JupiterWeb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterWeb.BL
{
    public  class UserManager : IUserManager
    {
        private readonly IUserRepo _userRepo;

        public UserManager(IUserRepo userRepo ) {
            _userRepo = userRepo;
        }
        public IEnumerable<UserReadDTO> GetUsers()
        { 
            IEnumerable<User> usersDB = _userRepo.GetUsers();
            return usersDB.Select(u => new UserReadDTO
            {   
                Name = u.Name,
                Address = u.Address,
                WhatsApp = u.WhatsApp,
                UserName = u.UserName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                Branch = u.Branch,
                GetEmployedAt = u.GetEmployedAt,
                Role = u.Role,
            });
        }
        public string Add(UserAddDTO userRequest)
        {
            User user = new User
            {
                Name= userRequest.Name,
                UserName= userRequest.UserName,
                Address = userRequest.Address,
                WhatsApp= userRequest.WhatsApp,
                PhoneNumber = userRequest.PhoneNumber,
                BasicSalary = userRequest.BasicSalary,
                PasswordHash = userRequest.Password,
                Email = userRequest.Email,
                Branch = userRequest.Branch,
                Role = userRequest.Role,
            };
            _userRepo.Add(user);
            _userRepo.SaveChanges();
            return user.Id;
        }

        public bool Delete(int id)
        {
            User? user = _userRepo.GetById(id);
            if (user == null) {
                return false;
            }
            _userRepo.Delete(user);
            _userRepo.SaveChanges();
            return true;
        }

        public UserReadDTO GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(UserUpdateDTO userRequest)
        {
            User? user = _userRepo.GetById(userRequest.Id);
            if (user == null)
            {
                return false;
            }
            user.Name = userRequest.Name;
            user.UserName = userRequest.UserName;
            user.Address = userRequest.Address;
            user.WhatsApp = userRequest.WhatsApp;
            user.PhoneNumber = userRequest.PhoneNumber;
            user.Branch = userRequest.Branch;
            user.Role = userRequest.Role;
            user.Email = userRequest.Email;
            user.PasswordHash = userRequest.Password;
            user.BasicSalary = userRequest.BasicSalary;

            _userRepo.Update(user);
            _userRepo.SaveChanges();
            return true;

        }
    }
}
