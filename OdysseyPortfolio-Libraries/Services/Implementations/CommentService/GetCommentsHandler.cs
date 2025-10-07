using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using OdysseyPortfolio_Libraries.Constants;
using OdysseyPortfolio_Libraries.DTOs.Comment;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace OdysseyPortfolio_Libraries.Services.Implementations.CommentService
{
    public class GetCommentsHandler
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private GetCommentsRequest? _request;
        private IEnumerable<Comment> _comments;
        private int _totalPages;
        private List<GetCommentsDto> _commentsDto;
        private User? _user;
        public GetCommentsHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ServiceResponse> Handle(GetCommentsRequest request)
        {
            try
            {
                _request = request;
                if (_request.BlogId == null) return BlogIdNotProvidedResponse();
                GetCommentsBySearchParams();
                ApplyPagination();
                await MapCommentsToGetComments();
                return GetCommentsSuccessResponse();
            }
            catch (Exception ex)
            {
                return InternalServerErrorResponse(ex);
            }
        }

        private void GetCommentsBySearchParams()
        {
            _comments = _unitOfWork.CommentRepository.Get(comment =>
                comment.BlogId.Equals(_request.BlogId));
            _comments = _comments.OrderByDescending(b => b.Id);
            _totalPages = (int)Math.Ceiling((double)_comments.Count() / _request.PageSize);
        }
        private void ApplyPagination()
        {
            _comments = _comments
               .Skip((_request.PageNumber - 1) * _request.PageSize)
               .Take(_request.PageSize)
               .ToList();
        }
        private async Task MapCommentsToGetComments()
        {
            _commentsDto = new List<GetCommentsDto>();
            foreach (var comment in _comments)
            {
                var commentDto = _mapper.Map<GetCommentsDto>(comment);
                commentDto.CommentId = comment.Id;
                commentDto.ElapsedTime = Utils.GetTimeAgo(comment.CreatedAt, DateTime.Now);
                var user = await _userManager.FindByIdAsync(comment.UserId);
                commentDto.UserName = user?.UserName;
                commentDto.CommentLikeDto = await GetCommentLikeFromComment(comment);
                _commentsDto.Add(commentDto);
            }
        }
        private async Task<CommentLikeDto> GetCommentLikeFromComment(Comment comment)
        {
            var commentLikes = _unitOfWork.CommentLikeRepository.Get(like => like.CommentId == comment.Id);
            var commentLikedByUser = commentLikes.FirstOrDefault(like => like.UserId == _request.UserId);
            return new CommentLikeDto
            {
                Liked = commentLikedByUser == null ? false : true,
                Likes = commentLikes.Count()
            };
        }
        private ServiceResponse GetCommentsSuccessResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.SUCCESS,
                Message = "Successfully retrieved all comments of the blog.",
                ReturnData = new
                {
                    pageNumber = _request?.PageNumber,
                    pageSize = _request?.PageSize,
                    totalPages = _totalPages,
                    comments = _commentsDto
                }
            };

        }
        private ServiceResponse BlogIdNotProvidedResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.BAD_REQUEST,
                Message = "BlogId is not provided, please try again.",
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
