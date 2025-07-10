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
    public class TweetController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public TweetController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
        {
            this.unitOfWork=unitOfWork;
            this.userManager=userManager;
            this.mapper=mapper;
        }
        [HttpPost("AddTweet")]
        public async Task<IActionResult> AddTweet(AddTweetDTO tweetDTO)
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

            var newTweet = mapper.Map<Tweet>(tweetDTO);
            user.Tweets?.Add(newTweet);

            unitOfWork.Tweets.Add(newTweet);
            unitOfWork.Save();
            response.SetResponse(true, "Tweet added successfully");
            return Ok(new { response, newTweet.Id }); //for testing
        }
        [HttpDelete("DeleteTweet/{tweetId}")]
        public async Task<IActionResult> DeleteTweet(int tweetId)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            var tweet = unitOfWork.Tweets.GetById(tweetId);
            if (tweet == null)
            {
                response.SetResponse(false, "Tweet not found");
                return NotFound(response);
            }
            if (user==null||user.UserName!=tweet.User.UserName)
            {
                response.SetResponse(false, "Unauthorized");
                return BadRequest(response);
            }
            unitOfWork.Tweets.delete(tweet);
            unitOfWork.Save();
            response.SetResponse(true, "Tweet deleted successfully");
            return Ok(response);
        }
        [HttpPost("EditTweet/{tweetId}")]
        public async Task<IActionResult> EditTweet(int tweetId, AddTweetDTO tweetDTO)
        {
            var response = new GeneralResponse();
            if (!ModelState.IsValid)
            {
                response.SetResponse(false, "Invalid data");
                return BadRequest(response);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            var tweet = unitOfWork.Tweets.GetById(tweetId);
            if (tweet == null)
            {
                response.SetResponse(false, "Tweet not found");
                return NotFound(response);
            }
            if (user==null||user.UserName!=tweet.User.UserName)
            {
                response.SetResponse(false, "Unauthorized");
                return BadRequest(response);
            }
            mapper.Map(tweetDTO, tweet);
            unitOfWork.Tweets.Update(tweet);
            unitOfWork.Save();
            response.SetResponse(true, "Tweet updated successfully");
            return Ok(response);
        }
        [HttpGet("GetAllTweets")]
        public async Task<IActionResult> GetAllTweets()
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var tweets = user.Followings.Select(f => f.FollowedUser.Tweets).ToList();
            var timeLine = mapper.Map<List<Tweet>>(tweets);
            return Ok(timeLine);
        }
        [HttpGet("GetTweet{tweetId}")]
        public async Task<IActionResult> GetTweetById(int tweetId)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var tweet = unitOfWork.Tweets.GetById(tweetId);
            if (tweet == null)
            {
                response.SetResponse(false, "Tweet not found");
                return NotFound(response);
            }
            var tweetDto = mapper.Map<TweetDTO>(tweet);
            response.SetResponse(true, tweetDto);
            return Ok(response);
        }
        [HttpPost("Like/{tweedId}")]
        public async Task<IActionResult> Like(int tweetId)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var tweet = unitOfWork.Tweets.GetById(tweetId);
            if (tweet == null)
            {
                response.SetResponse(false, "Tweet not found");
                return NotFound(response);
            }
            if (tweet.Likes.Any(l => l.UserId == userId))
            {
                response.SetResponse(false, "You have already liked this tweet");
                return BadRequest(response);
            }
            var like = new LikedTweet
            {
                UserId = userId,
                TweetId = tweetId,
            };
            tweet.Likes.Add(like);
            unitOfWork.LikedTweets.Add(like);
            unitOfWork.Save();
            response.SetResponse(true, "Tweet liked successfully");
            return Ok(response);
        }
        [HttpPost("Unlike/{tweetId}")]
        public async Task<IActionResult> Unlike(int tweetId)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var tweet = unitOfWork.Tweets.GetById(tweetId);
            if (tweet == null)
            {
                response.SetResponse(false, "Tweet not found");
                return NotFound(response);
            }
            var like = tweet.Likes.FirstOrDefault(l => l.UserId == userId);
            if (like == null)
            {
                response.SetResponse(false, "You have not liked this tweet");
                return BadRequest(response);
            }
            tweet.Likes.Remove(like);
            unitOfWork.LikedTweets.delete(like);
            unitOfWork.Save();
            response.SetResponse(true, "Tweet unliked successfully");
            return Ok(response);
        }
    }
}