using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Helpers
{
    public static class HttpUtils
    {
        public static void SetTokensInsideCookie(SetTokensInsideCookieOptions options)
        {
            var context = options.HttpContext;
            var tokenType = options.TokenType;
            var token = options.Token;
            switch (tokenType)
            {
                case TokenTypes.ACCESS_TOKEN:
                    context.Response.Cookies.Append("accessToken", token,
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddHours(3),
                            HttpOnly = true,
                            IsEssential = true,
                            Secure = true,
                            Domain = "",
                            SameSite = SameSiteMode.None
                        });
                    break;

                case TokenTypes.REFRESH_TOKEN:
                    context.Response.Cookies.Append("refreshToken", token,
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddDays(7),
                            HttpOnly = true,
                            IsEssential = true,
                            Secure = true,
                            Domain = "",
                            SameSite = SameSiteMode.None
                        });
                    break;
                default:
                    throw new ArgumentException($"Unknown token type: {tokenType}");
            }

        }
        public class SetTokensInsideCookieOptions
        {
            public string Token { get; set; }
            public string TokenType { get; set; }
            public HttpContext HttpContext { get; set; }
        }
        public static class TokenTypes
        {
            public const string ACCESS_TOKEN = "accessToken";
            public const string REFRESH_TOKEN = "refreshToken";
        }
    }
}
