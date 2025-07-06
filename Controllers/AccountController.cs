using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TwitterClone_API.DataAccess.Repo;
using TwitterClone_API.Models;
using TwitterClone_API.Models.DTOs;

namespace TwitterClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRepository<AppUser> userRepo;
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration config;

        public AccountController(IRepository<AppUser> userRepo, UserManager<AppUser> userManager, IConfiguration config)
        {
            this.userRepo=userRepo;
            this.userManager=userManager;
            this.config=config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(userRegister.Email);
            if (user!= null)
            {
                return BadRequest("User already exists");
            }
            var User = new AppUser()
            {
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                DateOfBirth = userRegister.DateOfBirth,
                ProfilePictureUrl = userRegister.ProfilePictureUrl,
                PhoneNumber = userRegister.PhoneNumber,
                CreatedAt = DateTime.Now
            };
            var result = await userManager.CreateAsync(User, userRegister.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            };
            return Ok(new { message = "User registered successfully" });
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
