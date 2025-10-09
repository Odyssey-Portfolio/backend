using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using OdysseyPortfolio_Libraries.Repositories;
using OdysseyPortfolio_Libraries.Services.Implementations.BlogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Services.Implementations.CommentService
{
    public class CommentService : ICommentService
    {
        private CreateCommentHandler _createCommentHandler;
        private GetCommentsHandler _getCommentsHandler;
        private readonly UserManager<User>? _userManager;
        private IUnitOfWork? _unitOfWork;
        private IMapper? _mapper;
        public CommentService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            InitializeServices();
        }
        public async Task<ServiceResponse> Create(CreateCommentRequest request)
        {
            var result = await _createCommentHandler.Handle(request);
            return result;
        }
        public async Task<ServiceResponse> Get(GetCommentsRequest request)
        {
            var result = await _getCommentsHandler.Handle(request);
            return result;
        }
        private void InitializeServices()
        {
            _createCommentHandler = new CreateCommentHandler(_unitOfWork, _userManager, _mapper);
            _getCommentsHandler = new GetCommentsHandler(_unitOfWork, _userManager, _mapper);
        }

  
    }
}
