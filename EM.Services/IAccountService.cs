using EM.Models;
using EM.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Services
{
    public interface IAccountService
    {
        bool Register(RegisterViewModel user);
        UserModel Login(LoginViewModel login, out string roleName);
        UserModel GetByEmail(string email);
    }
}
