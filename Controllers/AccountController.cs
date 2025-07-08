using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TwitterClone_API.DataAccess.Repo;
using TwitterClone_API.Models.AppModels;
using TwitterClone_API.Models.DTOs;
using TwitterClone_API.Models.Response;

namespace TwitterClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration config;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager, IConfiguration config,IMapper mapper)
        {
            this.userManager=userManager;
            this.config=config;
            this.mapper=mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(userRegister.Email);
            var response = new GeneralResponse();
            if (user != null)
            {
                response.SetResponse(false,"User already exists");
                return BadRequest(response);
            }
            var User = mapper.Map<AppUser>(userRegister);
            var result = await userManager.CreateAsync(User, userRegister.Password);
            if (!result.Succeeded)
            {
                response.SetResponse(false,result.Errors.Select(e => e.Description).ToList());
                return BadRequest(response);
            };
            response.SetResponse(true,"User registered successfully");
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login(UserLoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = userManager.FindByEmailAsync(loginDTO.Email).Result;
            if (user!=null)
            {
                var isExist = await userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (isExist)
                {
                    var Claims = new List<Claim>()
                    {
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim(ClaimTypes.Name,user.UserName!)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]!));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: config["JWT:Issuer"],
                        signingCredentials: creds,
                        expires: DateTime.Now.AddMonths(12),
                        claims: Claims
                     );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
            }
            return BadRequest("Wrong Email Or Password");
        }
    }
}
