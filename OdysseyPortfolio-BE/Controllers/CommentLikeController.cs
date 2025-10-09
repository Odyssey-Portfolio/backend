using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdysseyPortfolio_Libraries.Constants;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Services;
using System.Security.Claims;

namespace OdysseyPortfolio_BE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentLikeController : Controller
    {
        private ICommentLikeService _commentLikeService;
        public CommentLikeController(ICommentLikeService commentService)
        {
            _commentLikeService = commentService;
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> AddCommentLike([FromBody] AddCommentLikeRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _commentLikeService.Add(request);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete]        
        public async Task<IActionResult> RemoveCommentLike([FromBody] RemoveCommentLikeRequest  request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _commentLikeService.Remove(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
