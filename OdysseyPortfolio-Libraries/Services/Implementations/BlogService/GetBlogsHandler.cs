using AutoMapper;
using Microsoft.Identity.Client;
using OdysseyPortfolio_Libraries.Constants;
using OdysseyPortfolio_Libraries.DTOs;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using OdysseyPortfolio_Libraries.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Services.Implementations.BlogService
{
    public class GetBlogsHandler
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private GetBlogsRequest? _request;
        private IEnumerable<Blog>? _blogs;
        private List<object>? _getBlogs;

        public GetBlogsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResponse> Handle(GetBlogsRequest request)
        {
            try
            {
                _request = request;
                GetBlogsBySearchParams();
                if (_request.UserRole == UserRoles.Admin) return HandleBlogsForAdminUsers();
                return HandleBlogsForNonAdminUsers();
            }
            catch (Exception ex)
            {
                return InternalServerErrorResponse(ex);
            }
        }
        private ServiceResponse HandleBlogsForAdminUsers()
        {
            MapBlogsToGetBlogs<GetBlogDtoAdmin>();
            ApplyPagination();
            return GetBlogsSuccessResponse();
        }
        private ServiceResponse HandleBlogsForNonAdminUsers()
        {
            RemoveDeletedBlogsForNonAdminUsers();
            MapBlogsToGetBlogs<GetBlogDto>();
            ApplyPagination();
            return GetBlogsSuccessResponse();
        }

        private void GetBlogsBySearchParams()
        {
            if (_request?.Keyword == null) _blogs = _unitOfWork.BlogRepository.Get();
            else _blogs = _unitOfWork.BlogRepository.Get(blog =>
                blog.Title.ToLower().Contains(_request.Keyword));
            _blogs = _blogs.OrderByDescending(b => b.Id);
        }
        private void MapBlogsToGetBlogs<T>()
        {
            _getBlogs = new List<object>();
            foreach (var blog in _blogs)
            {
                var getBlog = _mapper.Map<T>(blog);
                _getBlogs.Add(getBlog);
            }
        }
        private void ApplyPagination()
        {
            _blogs = _blogs
               .Skip((_request.PageNumber - 1) * _request.PageSize)
               .Take(_request.PageSize)
               .ToList();
        }
        private void RemoveDeletedBlogsForNonAdminUsers()
        {
            if (_request.UserRole != UserRoles.Admin)
                _blogs = _blogs.Where(blog => !blog.IsDeleted);
        }
        private ServiceResponse GetBlogsSuccessResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.SUCCESS,
                Message = "Successfully retrieved all blogs.",
                ReturnData = _getBlogs
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
