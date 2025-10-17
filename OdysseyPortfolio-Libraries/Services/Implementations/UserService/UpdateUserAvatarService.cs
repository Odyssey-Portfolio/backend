using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OdysseyPortfolio_Libraries.Constants;
using OdysseyPortfolio_Libraries.DTOs;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Helpers;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Services.Implementations.UserService
{
    public class UpdateUserAvatarService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private UpdateUserAvatarRequest? _request;
        private User _user;

        public UpdateUserAvatarService(UserManager<User> userManager,
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

        public async Task<ServiceResponse> Handle(UpdateUserAvatarRequest request)
        {
            try
            {
                _request = request;
                if (!await CheckUserValidity()) return InvalidUserResponse();
                if (!await IsFileWithinLimit()) return FileExceedsLimitResponse();
                await UpdateUser();
                return UpdateAvatarSuccessResponse();
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
        private async Task<bool> IsFileWithinLimit()
        {
            var fileLength = _request.Avatar.Length;
            if (fileLength <= FileConstants.MAX_IMAGE_SIZE_IN_BYTES) return true;            
            return false;            
        }
        private async Task UpdateUser()
        {
            _user.Avatar = await FileUtils.IFormFileToBase64(_request.Avatar);
            await _userManager.UpdateAsync(_user);  
        }        
        private ServiceResponse UpdateAvatarSuccessResponse()
        {
            return new ServiceResponse
            {
                StatusCode = ResponseCodes.SUCCESS,
                Message = "Successfully updated the avatar of the user."
            };
        }
        private ServiceResponse FileExceedsLimitResponse()
        {
            return new ServiceResponse
            {
                StatusCode = ResponseCodes.BAD_REQUEST,
                Message = $"This file exceeds the maximum allowed limit of 500 KBs."
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
