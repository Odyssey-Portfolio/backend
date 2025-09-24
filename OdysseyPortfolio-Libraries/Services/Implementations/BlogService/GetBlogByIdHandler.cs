using AutoMapper;
using Azure.Core;
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
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Services.Implementations.BlogService
{
    public class GetBlogByIdHandler
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private GetBlogByIdRequest? _request;
        private Blog? _blog;
        private GetBlogByIdDto? _getBlogById;

        public GetBlogByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResponse> Handle(GetBlogByIdRequest request)
        {
            try
            {
                _request = request;
                GetBlogById();
                if (_getBlogById == null) return NoBlogFoundResponse();
                return GetBlogByIdSuccessResponse();
            }
            catch (Exception ex)
            {
                return InternalServerErrorResponse(ex);
            }
        }
        private void GetBlogById()
        {
            _blog = _unitOfWork.BlogRepository.GetByID(_request!.Id);
            if (_request.UserRole != UserRoles.Admin && _blog.IsDeleted) return;
            _getBlogById = _mapper.Map<GetBlogByIdDto>(_blog);
        }

        private ServiceResponse NoBlogFoundResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.NOT_FOUND,
                Message = "No blog was found with the given Id.",
                ReturnData = null!
            };
        }

        private ServiceResponse GetBlogByIdSuccessResponse()
        {
            return new ServiceResponse()
            {
                StatusCode = ResponseCodes.SUCCESS,
                Message = "Successfully retrieved the blog by Id",
                ReturnData = _getBlogById
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
