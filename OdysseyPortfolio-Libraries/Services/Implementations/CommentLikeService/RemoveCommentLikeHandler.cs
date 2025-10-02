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
    public class RemoveCommentLikeHandler
    {
        private readonly UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;        
        private RemoveCommentLikeRequest? _request;        
        private User? _user;
        public RemoveCommentLikeHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;            
        }
        public async Task<ServiceResponse> Handle(RemoveCommentLikeRequest request)
        {
            try
            {
                _request = request;
                bool isUserValid = await CheckUserValidity();
                if (!isUserValid) return InvalidUserResponse();
                RemoveCommentLike();
                return RemoveCommentLikeSuccessResponse();
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
    
        private void RemoveCommentLike()
        {
            var commentLike = _unitOfWork.CommentLikeRepository.GetByID(_request.CommentLikeId);
            _unitOfWork.CommentLikeRepository.Delete(commentLike);
            _unitOfWork.Save();
        }
        private ServiceResponse RemoveCommentLikeSuccessResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.CREATED,
                Message = $"Successfully removed Comment Like with ID: {_request.CommentLikeId}.",
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
