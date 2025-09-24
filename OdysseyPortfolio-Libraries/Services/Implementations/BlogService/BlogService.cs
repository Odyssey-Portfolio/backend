using AutoMapper;
using Azure.Core;
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

namespace OdysseyPortfolio_Libraries.Services.Implementations.BlogService
{
    public class BlogService : IBlogService
    {
        private CreateBlogHandler? _createBlogHandler;
        private GetBlogsHandler? _getBlogsHandler;
        private GetBlogByIdHandler? _getBlogByIdHandler;
        private UpdateBlogHandler? _updateBlogHandler;
        private DeleteBlogHandler? _deleteBlogHandler;
        private readonly UserManager<User> _userManager;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BlogService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            InitializeServices();
        }
        public async Task<ServiceResponse> Create(CreateBlogRequest request)
        {
            var result = await _createBlogHandler.Handle(request);
            return result;
        }

        public async Task<ServiceResponse> Get(GetBlogsRequest request)
        {
            var result = await _getBlogsHandler.Handle(request);
            return result;
        }

        public async Task<ServiceResponse> GetById(GetBlogByIdRequest request)
        {
            var result = await _getBlogByIdHandler.Handle(request);
            return result;
        }
        public async Task<ServiceResponse> Update(UpdateBlogRequest request)
        {
            var result = await _updateBlogHandler.Handle(request);
            return result;
        }
        public async Task<ServiceResponse> Delete(DeleteBlogRequest request)
        {
            var result = await _deleteBlogHandler.Handle(request);
            return result;
        }

        private void InitializeServices()
        {
            _createBlogHandler = new CreateBlogHandler(_unitOfWork, _userManager, _mapper);
            _getBlogsHandler = new GetBlogsHandler(_unitOfWork, _mapper);
            _getBlogByIdHandler = new GetBlogByIdHandler(_unitOfWork, _mapper);
            _updateBlogHandler = new UpdateBlogHandler(_unitOfWork, _userManager, _mapper);
            _deleteBlogHandler = new DeleteBlogHandler(_unitOfWork, _userManager);
        }        
    }
}
