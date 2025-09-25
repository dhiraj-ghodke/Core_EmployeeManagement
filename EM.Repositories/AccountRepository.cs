using EM.Core;
using EM.Core.Models;
using EM.Core.Models.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EM.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly EmployeeDBContext _dbContext;

        public AccountRepository(EmployeeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AspNetUser Login(string email, string password, out string roleName)
        {
            roleName = "";
            AspNetUser user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Email.Equals(email));
            if (user != null && PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                roleName = (from r in _dbContext.AspNetRoles
                            join ur in _dbContext.UserRoles
                            on r.Id equals ur.RoleId
                            where ur.UserId.Equals(user.Id)
                            select r.Name).First();

                return user;
            }

            return user;
        }

        public bool Register(AspNetUser user, string password)
        {
            try
            {
                user.UserName = user.Email;
                user.Id = Guid.NewGuid().ToString();
                user.PasswordHash = PasswordHasher.HashPassword(password);
                _dbContext.AspNetUsers.Add(user);
                _dbContext.SaveChanges();

                UserRole uRole = new UserRole() { UserId = user.Id, RoleId = "6" };
                _dbContext.UserRoles.Add(uRole);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public AspNetUser GetByEmail(string email)
        {
            AspNetUser user = new AspNetUser();
            if (!String.IsNullOrEmpty(email))
            {
                user = _dbContext.AspNetUsers.AsNoTracking().FirstOrDefault(e => e.Email == email);
                return user;
            }
            return user;
        }
    }
}
