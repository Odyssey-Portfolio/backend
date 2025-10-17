using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Services
{
    public interface IUserService
    {
        Task<ServiceResponse> Login(LoginRequest request);
        Task<ServiceResponse> Register(RegisterRequest request);
        Task<ServiceResponse> Update(UpdateUserDetailsRequest request);
        Task<ServiceResponse> UpdateAvatar(UpdateUserAvatarRequest request);
        Task<ServiceResponse> Logout();
    }
}
