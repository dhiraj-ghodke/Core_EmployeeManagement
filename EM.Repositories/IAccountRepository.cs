
using EM.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Repositories
{
    public interface IAccountRepository
    {
        bool Register(AspNetUser user,string password);
        AspNetUser Login(String email, string password, out string roleName);
        AspNetUser GetByEmail(string email);
    }
}
