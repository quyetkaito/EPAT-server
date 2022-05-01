//using EPAT.Core.Entities;
//using EPAT.Infrasctructure.Repository;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace EPAT.WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TokensController : ControllerBase
//    {
//        public IConfiguration _configuration;
//        AccountRepository _accountRepo;

//        //public TokenController(IConfiguration config)
//        //{
//        //    _configuration = config;

//        //}

//        public TokensController(AccountRepository accountRepo)
//        {
//            _accountRepo = accountRepo;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Post(Account _userData)
//        {
//            if (_userData != null && _userData.username != null && _userData.password != null)
//            {
//                var user = await GetUser(_userData.username, _userData.password);
//                var user = _accountRepo.

//                if (user != null)
//                {
//                    //create claims details based on the user information
//                    var claims = new[] {
//                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
//                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
//                        new Claim("UserId", user.UserId.ToString()),
//                        new Claim("DisplayName", user.DisplayName),
//                        new Claim("UserName", user.UserName),
//                        new Claim("Email", user.Email)
//                    };

//                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//                    var token = new JwtSecurityToken(
//                        _configuration["Jwt:Issuer"],
//                        _configuration["Jwt:Audience"],
//                        claims,
//                        expires: DateTime.UtcNow.AddMinutes(10),
//                        signingCredentials: signIn);

//                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
//                }
//                else
//                {
//                    return BadRequest("Invalid credentials");
//                }
//            }
//            else
//            {
//                return BadRequest();
//            }
//        }

//    }
//}

