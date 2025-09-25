using AutoMapper;
using EM.Core.Models.Entities;
using EM.Models;
using EM.Models.Entities;
using EM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepo, IMapper mapper)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
        }

        public UserModel Login(LoginViewModel login, out string roleName)
        {
            AspNetUser user = _accountRepo.Login(login.Email, login.Password, out roleName);
            return _mapper.Map<UserModel>(user);

        }

        public bool Register(RegisterViewModel user)
        {
            return _accountRepo.Register(_mapper.Map<AspNetUser>(user), user.Password);
        }
        public UserModel GetByEmail(string email)
        {
            AspNetUser user = _accountRepo.GetByEmail(email);
            return _mapper.Map<UserModel>(user);
        }
    }
}
