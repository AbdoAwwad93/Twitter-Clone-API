using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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
    public class CommentController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public CommentController(UserManager<AppUser> userManager,IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.userManager=userManager;
            this.unitOfWork=unitOfWork;
            this.mapper=mapper;
        }
        [HttpPost("AddComment/{tweetId}")]
        public async Task<IActionResult> AddComment(int tweetId, AddCommentDTO commentDTO)
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
            if (tweet==null)
            {
                response.SetResponse(false, "Tweet not found");
                return NotFound(response);
            }
            var comment = mapper.Map<Comment>(commentDTO);
            user.Comments.Add(comment);
            tweet.Comments.Add(comment);
            unitOfWork.Comments.Add(comment);
            unitOfWork.Save();
            response.SetResponse(true, "Comment added successfully");
            return Ok(new { response, comment.Id }); //for testing
        }

        [HttpPost("EditComment/{commentId}")]
        public async Task<IActionResult> EditComment(int commentId, AddCommentDTO commentDTO)
        {
            var response = new GeneralResponse();
            if (!ModelState.IsValid)
            {
                response.SetResponse(false, "Comment is not valid");
                return BadRequest(response);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var comment = unitOfWork.Comments.GetById(commentId);
            if (comment==null)
            {
                response.SetResponse(false, "Comment not found");
                return NotFound(response);
            }
            mapper.Map(commentDTO, comment);
            unitOfWork.Comments.Update(comment);
            unitOfWork.Save();
            response.SetResponse(true, "Comment edited successfully");
            return Ok(response);
        }
        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var comment = unitOfWork.Comments.GetById(commentId);
            if (comment == null)
            {
                response.SetResponse(false, "Comment not found");
                return NotFound(response);
            }
            if (user.UserName != comment.User.UserName)
            {
                response.SetResponse(false, "Unauthorized");
                return BadRequest(response);
            }
            unitOfWork.Comments.delete(comment);
            unitOfWork.Save();
            response.SetResponse(true, "Comment deleted successfully");
            return Ok(response);
        }
        [HttpGet("GetCommentsByTweetId/{tweetId}")]
        public async Task<IActionResult> GetCommentsByTweetId(int tweetId)
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
            var comments = tweet.Comments?.ToList() ?? new List<Comment>();
            var commentDTOs = mapper.Map<List<CommentDTO>>(comments);
            response.SetResponse(true, commentDTOs);
            return Ok(response);
        }
       
        [HttpPost("Like/{commentId}")]
        public async Task<IActionResult> Like(int commentId)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var comment = unitOfWork.Comments.GetById(commentId);
            if (comment == null)
            {
                response.SetResponse(false, "Comment not found");
                return NotFound(response);
            }
            if (comment.Likes.Any(l => l.UserId == user.Id))
            {
                response.SetResponse(false, "You already liked this comment");
                return BadRequest(response);
            }
            var like = new LikedComment
            {
                UserId = userId,
                CommentId= commentId
            };
            comment.Likes.Add(like);
            unitOfWork.LikedComments.Add(like);
            unitOfWork.Save();
            response.SetResponse(true, "Comment liked successfully");
            return Ok(response);
        }
        [HttpPost("Unlike/{commentId}")]
        public async Task<IActionResult> Unlike(int commentId)
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var comment = unitOfWork.Comments.GetById(commentId);
            if (comment == null)
            {
                response.SetResponse(false, "Comment not found");
                return NotFound(response);
            }
            var like = comment.Likes.FirstOrDefault(l => l.UserId == user.Id);
            if (like == null)
            {
                response.SetResponse(false, "You have not liked this comment");
                return BadRequest(response);
            }
            comment.Likes.Remove(like);
            unitOfWork.LikedComments.delete(like);
            unitOfWork.Save();
            response.SetResponse(true, "Comment unliked successfully");
            return Ok(response);
        }
    }
}
