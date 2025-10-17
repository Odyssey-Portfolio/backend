using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OdysseyPortfolio_Libraries.Constants;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Services.Implementations.UserService
{
    public class UserService : IUserService
    {
        private LoginService _loginService;
        private RegisterService _registerService;
        private LogoutService _logoutService;
        private UpdateService _updateService;
        private UpdateUserAvatarService _updateUserAvatarService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        public UserService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<UserService> logger,            
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            InitializeServices();
        }
        public async Task<ServiceResponse> Login(LoginRequest request)
        {
            var result = await _loginService.Handle(request);
            return result;
        }
        public async Task<ServiceResponse> Register(RegisterRequest request)
        {
            var result = await _registerService.Handle(request);
            return result;
        }
        public async Task<ServiceResponse> Logout()
        {
            var result = await _logoutService.Handle();
            return result;
        }

        public async Task<ServiceResponse> Update(UpdateUserDetailsRequest request)
        {
            return await _updateService.Handle(request);
        }

        public async Task<ServiceResponse> UpdateAvatar(UpdateUserAvatarRequest request)
        {
            return await _updateUserAvatarService.Handle(request);
        }

        private void InitializeServices()
        {            
            _loginService = new LoginService(_userManager, _roleManager, _configuration,_logger);
            _registerService = new RegisterService(_userManager, _roleManager, _configuration,_mapper, _logger);
            _updateService = new UpdateService(_userManager, _roleManager, _configuration, _mapper, _logger);
            _logoutService = new LogoutService(_httpContextAccessor,_logger);
            _updateUserAvatarService = new UpdateUserAvatarService(_userManager, _roleManager, _configuration, _mapper, _logger);
        }

     
    }
}
