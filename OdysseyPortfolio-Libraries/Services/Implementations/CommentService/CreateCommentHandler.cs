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
                await HandleInvalidOutOfCommentLimitTime();
                if (_user.NumberOfCommentsLeft > 0) return await HandleWithCommentLimit();
                return await HandleOutOfCommentLimit();
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
        private async Task HandleInvalidOutOfCommentLimitTime()
        {
            var outOfCommentLimitTime = _user.OutOfCommentLimitTime;
            if (outOfCommentLimitTime != DateTime.MinValue) return;
            _user.OutOfCommentLimitTime = DateTime.Now.ToUniversalTime();
            await _userManager.UpdateAsync(_user);
        }
        private async Task<ServiceResponse> HandleWithCommentLimit()
        {
            await DeductCommentLimit();
            MapCommentRequestToComment();
            SaveComment();
            return CreateCommentSuccessResponse();
        }
        private async Task<ServiceResponse> HandleOutOfCommentLimit()
        {
            if (OneDayHasElapsed())
            {
            /*
                * Prevents OutOfCommentLimitTime from becoming old, breaking OneDayHasElapsed 
                * function. Example:
                * OutOfCommentLimitTime equals Sep 30, and currentDate = Oct 2.
                => Oct 2/3/4/5... - Sep 30 > 1 (always true) => Date is always refilled.              
            */
                _user.OutOfCommentLimitTime = DateTime.Now.ToUniversalTime();
                await _userManager.UpdateAsync(_user);
                await RefillCommentLimit();
                await DeductCommentLimit();
                MapCommentRequestToComment();
                SaveComment();
                return CreateCommentSuccessResponse();

            }
            _user.OutOfCommentLimitTime = DateTime.Now.ToUniversalTime();
            await _userManager.UpdateAsync(_user);
            return OutOfCommentLimitResponse();
        }
        private bool OneDayHasElapsed()
        {
            var outOfCommentLimitTime = _user.OutOfCommentLimitTime;
            var currentTime = DateTime.Now;
            return currentTime - outOfCommentLimitTime >= TimeSpan.FromDays(1);
        }
        private async Task RefillCommentLimit()
        {
            _user.NumberOfCommentsLeft = CommentConstants.USER_DAILY_COMMENT_LIMIT;
            await _userManager.UpdateAsync(_user);
        }
        
        private async Task DeductCommentLimit()
        {
            _user.NumberOfCommentsLeft--;
            await _userManager.UpdateAsync(_user);
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

        private ServiceResponse OutOfCommentLimitResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.BAD_REQUEST,
                Message = "Sorry but you have reached the comment limit for today (5). Please try again tomorrow!"
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
