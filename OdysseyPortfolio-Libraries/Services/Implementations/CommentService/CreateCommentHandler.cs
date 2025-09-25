using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OdysseyPortfolio_Libraries.Constants;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Helpers;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using OdysseyPortfolio_Libraries.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Services.Implementations.CommentService
{
    public class CreateCommentHandler
    {
        private readonly UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private CreateCommentRequest? _request;
        private Comment? _comment;
        private User? _user;
        public CreateCommentHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ServiceResponse> Handle(CreateCommentRequest request)
        {
            try
            {
                _request = request;
                bool isUserValid = await CheckUserValidity();
                if (!isUserValid) return InvalidUserResponse();
                MapCommentRequestToComment();
                SaveComment();
                return CreateCommentSuccessResponse();
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
        private async void MapCommentRequestToComment()
        {
            _comment = _mapper.Map<Comment>(_request);
            _comment.DisabledReason = "";
            _comment.CreatedAt = DateTime.Now.ToUniversalTime();
            _comment.Id = EntityUtils.GenerateEntityId<Comment>();                        
        }
        private void SaveComment()
        {
            _unitOfWork.CommentRepository.Insert(_comment);
            _unitOfWork.Save();
        }
        private ServiceResponse CreateCommentSuccessResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.CREATED,
                Message = "Successfully created a Comment.",
            };
        }

        private ServiceResponse InvalidUserResponse()
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
                Message = $"Something went wrong on the server side. {ex.Message}"
            };
        }
    }
}
