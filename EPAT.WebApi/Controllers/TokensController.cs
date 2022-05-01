using EPAT.Core.Entities;
using EPAT.Core.Interfaces;
using EPAT.Infrasctructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EPAT.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        public IConfiguration _configuration;
        IAccountService _accountService;

        public TokensController(IConfiguration config, IAccountService accountService)
        {
            _configuration = config;
            _accountService = accountService;

        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginInfo loginInfo)
        {
            if (loginInfo != null && loginInfo.username != null && loginInfo.password != null)
            {
                //var user = await GetUser(_userData.username, _userData.password);
               
                var account = _accountService.Login(loginInfo);

                if (account != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("account_id", account.account_id.ToString()),
                        new Claim("account_name", account.account_name),
                        new Claim("username", account.username),

                    };


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

    }
}

