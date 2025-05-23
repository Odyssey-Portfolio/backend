﻿using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromForm] CreateBlogRequest request)
        {
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;             
            var result = await _blogService.Create(request);
            return StatusCode(result.StatusCode, result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetBlogs([FromQuery] GetBlogsRequest request)
        {
            var result = await _blogService.Get(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
