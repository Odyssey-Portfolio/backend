using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OdysseyPortfolio_Libraries.Constants;
using OdysseyPortfolio_Libraries.DTOs;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace OdysseyPortfolio_Libraries.Services.Implementations.UserService
{
    public class LoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private LoginRequest? _request;
        private string _jwtTokenString;
        private User _user;
        private ILogger _logger;
        private IList<string> _userRoles;

        public LoginService(UserManager<User> userManager,
                            RoleManager<IdentityRole> roleManager,
                            IConfiguration configuration,
                            ILogger logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<ServiceResponse> Handle(LoginRequest request)
        {
            try
            {
                _request = request;
                bool isUserValid = await CheckUserValidity();
                if (!isUserValid) return InvalidCredentialsResponse();
                await GenerateJwtToken();
                return LoginSuccessResponse();
            }
            catch (Exception ex)
            {
                return InternalServerErrorResponse(ex);
            }
        }
        private async Task<bool> CheckUserValidity()
        {
            _user = await _userManager.FindByEmailAsync(_request.Email);
            if (_user != null && await _userManager.CheckPasswordAsync(_user, _request.Password))
                return true;
            return false;
        }
        private async Task GenerateJwtToken()
        {
            _userRoles = await _userManager.GetRolesAsync(_user);
            var authClaims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Name, _user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            foreach (var userRole in _userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var token = GetToken(authClaims);
            _jwtTokenString = new JwtSecurityTokenHandler().WriteToken(token);
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {            
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddHours(AuthConstants.TOKEN_TIMEOUT_IN_HOURS),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        private ServiceResponse LoginSuccessResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.SUCCESS,
                Message = "Successfully logged in.",
                ReturnData =
                    new LoggedInUserDto()
                    {
                        Id = _user.Id,
                        Name = _user.Name,
                        Roles = _userRoles.ToArray(),
                        Token = _jwtTokenString,
                    }
            };
        }
        private ServiceResponse InvalidCredentialsResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.BAD_REQUEST,
                Message = "Either the email or password is invalid. Please try again."
            };
        }
        private ServiceResponse InternalServerErrorResponse(Exception ex)
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.INTERNAL_SERVER_ERROR,
                Message = $"Something went wrong on the server side. {ex.StackTrace.ToString()}"
            };
        }
    }
}
