using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TwitterClone_API.DataAccess.Repo;
using TwitterClone_API.Models.AppModels;
using TwitterClone_API.Models.DTOs;
using TwitterClone_API.Models.Response;

namespace TwitterClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        public ProfileController(UserManager<AppUser> userManager, IMapper mapper)
        {
            this.userManager=userManager;
            this.mapper=mapper;
        }

        [HttpGet("MyProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            var response = new GeneralResponse();
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var profileDto = mapper.Map<ProfileDTO>(user);
            response.SetResponse(true, profileDto);
            return Ok(response);
        }

        [HttpGet("Search/Profile/{useranme}")]
        public async Task<IActionResult> SearchAboutProfile(string useranme)
        {
            var SearchUser = await userManager.FindByNameAsync(useranme);
            var response = new GeneralResponse();
            if (SearchUser==null)
            {
                response.SetResponse(false, "User not found");
                return NotFound(response);
            }
            var signedUserName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (signedUserName==useranme)
            {
                response.SetResponse(false, "You cannot search your own profile");
                return BadRequest(response);
            }
            var profileDto = mapper.Map<SearchProfileDTO>(SearchUser);
            response.SetResponse(true, profileDto);
            return Ok(response);
        }
        [HttpPost("EditMyProfile")]
        public async Task<IActionResult> EditMyProfile(ProfileDTO profileDTO)
        {
            var response = new GeneralResponse();
            if (!ModelState.IsValid)
            {
                response.SetResponse(false, "Invalid data");
                return BadRequest(response);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            mapper.Map(profileDTO, user);
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.SetResponse(false, result.Errors.Select(e => e.Description).ToList());
                return BadRequest(response);
            }
            response.SetResponse(true, "Profile updated successfully");
            return Ok(response);
        }
        [HttpDelete("DeleteMyProfile")]
        public async Task<IActionResult> DeleteMyProfile(UserLoginDTO userData)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var currentUser = await userManager.FindByIdAsync(userId);
            var userFromRequest = await userManager.FindByEmailAsync(userData.Email);
            var response = new GeneralResponse();
            if (currentUser == null)
            {
                response.SetResponse(false, "Unauthorized or User not found");
                return Unauthorized(response);
            }
            if (userFromRequest != null&&userFromRequest.Email==currentUser.Email)
            {
                var IsExist = await userManager.CheckPasswordAsync(userFromRequest, userData.Password);
                if (IsExist)
                {
                    var result = await userManager.DeleteAsync(userFromRequest);
                    if (!result.Succeeded)
                    {
                        response.SetResponse(false, result.Errors.Select(e => e.Description).ToList());
                        return BadRequest(response);
                    }
                    response.SetResponse(true, "Profile deleted successfully");
                    return Ok(response);
                }
            }
            response.SetResponse(false, "Invalid Email OR Password");
            return BadRequest(response);
        }
    }
}