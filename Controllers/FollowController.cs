using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterClone_API.DataAccess.Repo;
using TwitterClone_API.DataAccess.Repo.UnitOfWork;
using TwitterClone_API.Models.AppModels;
using TwitterClone_API.Models.DTOs;
using TwitterClone_API.Models.Response;

namespace TwitterClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public FollowController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
        {
            this.unitOfWork=unitOfWork;
            this.userManager=userManager;
            this.mapper=mapper;
        }
        [HttpPost("followUser/{userName}")]
        public async Task<IActionResult> Follow(string userName)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var FollowerUser = await userManager.FindByIdAsync(userId);
            if (FollowerUser == null)
            {
                response.SetResponse(false, "User not found");
                return NotFound(response);
            }
            var followedUser = await userManager.FindByNameAsync(userName);
            if (followedUser==null)
            {
                response.SetResponse(false, "User you want to follow Not found");
                return NotFound(response);
            }
            if (FollowerUser.UserName==userName)
            {
                response.SetResponse(false, "You cannot follow yourself");
                return BadRequest(response);
            }
            bool IsFollowed =unitOfWork.Follows.IsFollowing(userId,followedUser.Id);
            if (IsFollowed)
            {
                response.SetResponse(false, "You are already following this user");
                return BadRequest(response);
            }
            var follow = new Follow
            {
                FollowerId = FollowerUser.Id,
                FollowedId = followedUser.Id,
                FollowedAt = DateTime.Now
            };
            unitOfWork.Follows.Add(follow);
            unitOfWork.Save();
            response.SetResponse(true, "You are now following " + userName);
            return Ok(response);
        }
        [HttpGet("GetFollowers/{useraName}")]
        public async Task<IActionResult> GetFollowers(string useraName)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var searchUser = await userManager.FindByNameAsync(useraName);
            if (searchUser == null)
            {
                response.SetResponse(false, "User not found");
                return NotFound(response);
            }
            if (!unitOfWork.Follows.HasFollowers(searchUser))
            {
                response.SetResponse(false, "No followers");
                return Ok(response);
            }
            var follwers = unitOfWork.Follows.GetFollowers(searchUser.Id);
            var follwersDto = mapper.Map<List<FollowerDTO>>(follwers);
            response.SetResponse(true, follwersDto);
            return Ok(response);
        }
        [HttpGet("GetFollowings/{userName}")]
        public async Task<IActionResult> GetFollowings(string userName)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var searchUser = await userManager.FindByNameAsync(userName);
            if (searchUser == null)
            {
                response.SetResponse(false, "User not found");
                return NotFound(response);
            }
            if (!unitOfWork.Follows.HasFollowing(searchUser))
            {
                response.SetResponse(false, "No followings");
                return Ok(response);
            }
            var followings = unitOfWork.Follows.GetFollowings(searchUser.Id);
            var followingsDto = mapper.Map<List<FollowerDTO>>(followings);
            response.SetResponse(true, followingsDto);
            return Ok(response);
        }
        [HttpPost("UnFollow/{userName}")]
        public async Task<IActionResult> UnFollow(string userName)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var followedUser = await userManager.FindByNameAsync(userName);
            if (followedUser == null)
            {
                response.SetResponse(false, "User you want to unfollow Not found");
                return NotFound(response);
            }
            if (user.UserName == userName)
            {
                response.SetResponse(false, "You cannot unfollow yourself");
                return BadRequest(response);
            }
            bool IsFollowed = unitOfWork.Follows.IsFollowed(user,followedUser.Id);
            if (!IsFollowed)
            {
                response.SetResponse(false, "You are not following this user");
                return BadRequest(response);
            }
            var follow = user.Followings.FirstOrDefault(f => f.FollowedId == followedUser.Id);
            if (follow != null)
            {
                unitOfWork.Follows.delete(follow);
                unitOfWork.Save();
                response.SetResponse(true, "You have unfollowed " + userName);
                return Ok(response);
            }
            response.SetResponse(false, "Unfollowing failed");
            return BadRequest(response);
        }
        [HttpGet("FollowCount/{userName}")]
        public async Task<IActionResult> FollowCount(string userName)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            var searchuser = await userManager.FindByNameAsync(userName);
            if (searchuser == null)
            {
                response.SetResponse(false, "User not found");
                return NotFound(response);
            }
            var followersCount = searchuser.Followers.Count;
            var followingsCount = searchuser.Followings.Count;
            response.SetResponse(true, new { followersCount, followingsCount });
            return Ok(response);
        }
        [HttpGet("getMutualFollowers/{userName}")]
        public async Task<IActionResult> MutualFollowrs(string userName)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.Name)!.Value;
            var user = await userManager.FindByNameAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            if (user.UserName == userName)
            {
                response.SetResponse(false, "You cannot search your own mutual followers");
                return BadRequest(response);
            }
            var searchUser = await userManager.FindByNameAsync(userName);
            if (searchUser == null)
            {
                response.SetResponse(false, "User not found");
                return NotFound(response);
            }
            var mutualFollowers = unitOfWork.Follows.GetMutualFollowers(userId, searchUser.Id);
            if (!mutualFollowers.Any())
            {
                response.SetResponse(false, "No mutual followers found");
                return Ok(response);
            }
            var mutualFollowersDto = mapper.Map<List<FollowerDTO>>(mutualFollowers);
            response.SetResponse(true, mutualFollowersDto);
            return Ok(response);
        }
    }
}