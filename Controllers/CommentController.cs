using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Security.Claims;
using TwitterClone_API.DataAccess.Repo;
using TwitterClone_API.Models.AppModels;
using TwitterClone_API.Models.DTOs;
using TwitterClone_API.Models.Response;

namespace TwitterClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IRepository<Tweet> tweetRepo;
        private readonly IRepository<Comment> commentRepo;
        private readonly IMapper mapper;

        public CommentController(UserManager<AppUser> userManager, IRepository<Tweet> tweetRepo,
            IRepository<Comment> commentRepo, IMapper mapper)
        {
            this.userManager=userManager;
            this.tweetRepo=tweetRepo;
            this.commentRepo=commentRepo;
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
            var tweet = tweetRepo.GetById(tweetId);
            if (tweet==null)
            {
                response.SetResponse(false, "Tweet not found");
                return NotFound(response);
            }
            var comment = mapper.Map<Comment>(commentDTO);
            user.Comments.Add(comment);
            tweet.Comments.Add(comment);
            commentRepo.Add(comment);
            commentRepo.Save();
            response.SetResponse(true, "Comment added successfully");
            return Ok(new { response, comment.Id }); //for testing
        }

        [HttpPost("EditComment/{coomentId}")]
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
            var comment = commentRepo.GetById(commentId);
            if (comment==null)
            {
                response.SetResponse(false, "Comment not found");
                return NotFound(response);
            }
            mapper.Map(commentDTO, comment);
            commentRepo.Update(comment);
            commentRepo.Save();
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
            var comment = commentRepo.GetById(commentId);
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
            commentRepo.delete(comment);
            commentRepo.Save();
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
            var tweet = tweetRepo.GetById(tweetId);
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
        [HttpGet("MyReplies")]
        public async Task<IActionResult> MyReplies()
        {
            var response = new GeneralResponse();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.SetResponse(false, "Unauthorized");
                return Unauthorized(response);
            }
            var comments = user.Comments?.ToList() ?? new List<Comment>();
            var commentDTOs = mapper.Map<List<CommentDTO>>(comments);
            response.SetResponse(true, commentDTOs);
            return Ok(response);
        }
    }
}
