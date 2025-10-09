using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OdysseyPortfolio_Libraries.Constants;
using OdysseyPortfolio_Libraries.DTOs;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using AutoMapper;

namespace OdysseyPortfolio_Libraries.Services.Implementations.UserService
{
    public class RegisterService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private RegisterRequest? _request;
        private User _user;

        public RegisterService(UserManager<User> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IConfiguration configuration,
                               IMapper mapper, 
                               ILogger logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(RegisterRequest request)
        {
            try
            {
                _request = request;

                if (await CheckIfEmailExists())
                {
                    return EmailAlreadyExistsResponse();
                }

                await CreateUser();
                await AssignDefaultRole();

                return RegisterSuccessResponse();
            }
            catch (Exception ex)
            {
                return InternalServerErrorResponse(ex);
            }
        }

        private async Task<bool> CheckIfEmailExists()
        {
            var existingUser = await _userManager.FindByEmailAsync(_request.Email);
            return existingUser != null;
        }

        private async Task CreateUser()
        {
            _user = _mapper.Map<User>(_request);
            _user.UserName = _request?.Email;
            _user.NumberOfCommentsLeft = CommentConstants.USER_DAILY_COMMENT_LIMIT;
            var result = await _userManager.CreateAsync(_user, _request!.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        private async Task AssignDefaultRole()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            await _userManager.AddToRoleAsync(_user, UserRoles.User);
        }

        private ServiceResponse RegisterSuccessResponse()
        {
            return new ServiceResponse
            {
                StatusCode = ResponseCodes.SUCCESS,
                Message = "Successfully registered."                
            };
        }

        private ServiceResponse EmailAlreadyExistsResponse()
        {
            return new ServiceResponse
            {
                StatusCode = ResponseCodes.CONFLICT,
                Message = "This email is already registered. Please try logging in or use a different email."
            };
        }

        private ServiceResponse InternalServerErrorResponse(Exception ex)
        {
            return new ServiceResponse
            {
                StatusCode = ResponseCodes.INTERNAL_SERVER_ERROR,
                Message = $"Something went wrong on the server side. {ex.StackTrace}"
            };
        }
    }
}
