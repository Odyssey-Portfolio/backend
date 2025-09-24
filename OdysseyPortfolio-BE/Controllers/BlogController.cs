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
    public class BlogController : Controller
    {
        private IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs([FromQuery] GetBlogsRequest request)
        {
            request.UserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var result = await _blogService.Get(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{blogId}")]
        public async Task<IActionResult> GetBlogById([FromRoute] string blogId)
        {
            GetBlogByIdRequest request = new GetBlogByIdRequest()
            {
                Id = blogId,
                UserRole = User.FindFirst(ClaimTypes.Role)?.Value
            };         
            var result = await _blogService.GetById(request);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromForm] CreateBlogRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;             
            var result = await _blogService.Create(request);
            return StatusCode(result.StatusCode, result);
        }
        
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> UpdateBlog([FromForm] UpdateBlogRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _blogService.Update(request);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete]
        public async Task<IActionResult> DeleteBlog([FromForm] DeleteBlogRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _blogService.Delete(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
