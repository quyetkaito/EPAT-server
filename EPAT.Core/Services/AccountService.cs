using EPAT.Core.Entities;
using EPAT.Core.Interfaces;
using EPAT.Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAT.Core.Services
{
    /// <summary>
    /// Nghiệp vụ cho account
    /// </summary>
    /// Author: quyetkaito (29/04/2022)
    public class AccountService : BaseService<Account>,IAccountService
    {
        IAccountRepository _repository;
        public AccountService(IAccountRepository repository) : base(repository)
        {
            _repository  = repository;

        }
    }
}
