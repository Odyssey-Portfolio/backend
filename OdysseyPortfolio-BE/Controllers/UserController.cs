using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OdysseyPortfolio_Libraries.DTOs;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Helpers;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Services;
using static OdysseyPortfolio_Libraries.Helpers.HttpUtils;
using static OdysseyPortfolio_Libraries.Services.Implementations.UserService.LoginService;
using LoginRequest = OdysseyPortfolio_Libraries.Payloads.Request.LoginRequest;
using RegisterRequest = OdysseyPortfolio_Libraries.Payloads.Request.RegisterRequest;

namespace OdysseyPortfolio_BE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.Login(request);
            var loggedInUser = result.ReturnData as LoggedInUserDto;
            if (loggedInUser != null)
            {
                SetTokensInsideCookie(new SetTokensInsideCookieOptions()
                {
                    HttpContext = HttpContext,
                    Token = loggedInUser.Token,
                    TokenType = TokenTypes.ACCESS_TOKEN
                });
            }
            loggedInUser.Token = null;
            return StatusCode(result.StatusCode, result);
        }
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.Register(request);
            return StatusCode(result.StatusCode, result);
        }

        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var result = await _userService.Logout();
            return StatusCode(result.StatusCode, result);
        }
    }
}
