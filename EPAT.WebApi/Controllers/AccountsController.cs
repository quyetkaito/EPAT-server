using EPAT.Core.Entities;
using EPAT.Core.Interfaces;
using EPAT.Core.Interfaces.Base;
using EPAT.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPAT.WebApi.Controllers
{
    /// <summary>
    /// API cho tài khoản
    /// </summary>
    /// Author: quyetkaio (28/04/2022)
    public class AccountsController : EPatBaseController<Account>
    {
        IAccountService _accountService;
        public AccountsController(IAccountService accountService) : base(accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// API đăng nhập theo tài khoản mật khẩu.
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(LoginInfo loginInfo )
        {
            var res = _accountService.Login(loginInfo);
            return Ok(res);
        }
    }
}
