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
    public class UpdateService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private UpdateUserDetailsRequest? _request;
        private User _user;

        public UpdateService(UserManager<User> userManager,
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

        public async Task<ServiceResponse> Handle(UpdateUserDetailsRequest request)
        {
            try
            {
                _request = request;
                if (!await CheckUserValidity()) return InvalidUserResponse();
                if (await CheckIfEmailExists()) return EmailAlreadyExistsResponse();
                await UpdateUser();
                return RegisterSuccessResponse();
            }
            catch (Exception ex)
            {
                return InternalServerErrorResponse(ex);
            }
        }

        private async Task<bool> CheckUserValidity()
        {
            _user = await _userManager.FindByIdAsync(_request.UserId);
            if (_user != null) return true;
            return false;
        }
        private async Task<bool> CheckIfEmailExists()
        {
            var existingUser = await _userManager.FindByEmailAsync(_request.Email);            
            if (existingUser.Id == _user.Id && 
                existingUser.Email == _user.Email) 
                return false;
            return true;
        }
        private async Task UpdateUser()
        {
            _mapper.Map(_request, _user);
            await HandlePasswordUpdate();
            await HandleUserUpdate();
        }
        private async Task HandlePasswordUpdate()
        {
            if (String.IsNullOrEmpty(_request.OldPassword) || String.IsNullOrEmpty(_request.NewPassword)) return;
            var updatePasswordResult = await _userManager
                .ChangePasswordAsync(_user, _request.OldPassword, _request.NewPassword);
            if (!updatePasswordResult.Succeeded)
                throw new Exception(string.Join("; ", updatePasswordResult.Errors.Select(e => e.Description)));

        }
        private async Task HandleUserUpdate()
        {
            var updateUserResult = await _userManager.UpdateAsync(_user);
            if (!updateUserResult.Succeeded)
            {
                throw new Exception(string.Join("; ", updateUserResult.Errors.Select(e => e.Description)));
            }
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

        private ServiceResponse InvalidUserResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.BAD_REQUEST,
                Message = "The logged in user does not exist in the database. Please try again."
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
