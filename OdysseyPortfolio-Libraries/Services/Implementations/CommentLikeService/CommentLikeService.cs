using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using OdysseyPortfolio_Libraries.Repositories;
using OdysseyPortfolio_Libraries.Services.Implementations.BlogService;
using OdysseyPortfolio_Libraries.Services.Implementations.CommentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Services.Implementations.CommentLikeService
{
    public class CommentLikeService : ICommentLikeService
    {
        private AddCommentLikeHandler _addCommentLikeHandler;
        private RemoveCommentLikeHandler _removeCommentLikeHandler;
        private readonly UserManager<User>? _userManager;
        private IUnitOfWork? _unitOfWork;
        private IMapper? _mapper;
        public CommentLikeService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            InitializeServices();
        }

        public async Task<ServiceResponse> Add(AddCommentLikeRequest request)
        {
            return await _addCommentLikeHandler!.Handle(request);
        }

        public async Task<ServiceResponse> Remove(RemoveCommentLikeRequest request)
        {
            return await _removeCommentLikeHandler!.Handle(request);
        }
        private void InitializeServices()
        {
            _addCommentLikeHandler = new AddCommentLikeHandler(_unitOfWork, _userManager, _mapper);
            _removeCommentLikeHandler = new RemoveCommentLikeHandler(_unitOfWork, _userManager);
        }
    }
}
