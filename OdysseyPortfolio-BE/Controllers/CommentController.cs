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
    public class CommentController : Controller
    {
        private ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _commentService.Create(request);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet]        
        public async Task<IActionResult> GetComments([FromQuery] GetCommentsRequest request)
        {            
            var result = await _commentService.Get(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
