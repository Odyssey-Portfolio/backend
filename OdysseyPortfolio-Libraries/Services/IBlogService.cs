using OdysseyPortfolio_Libraries.Payloads.Request;
using OdysseyPortfolio_Libraries.Payloads.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Services
{
    public interface IBlogService
    {
        Task<ServiceResponse> Get(GetBlogsRequest request);
        Task<ServiceResponse> GetById(GetBlogByIdRequest request);
        Task<ServiceResponse> Create(CreateBlogRequest request);
        Task<ServiceResponse> Update(UpdateBlogRequest request);
        Task<ServiceResponse> Delete(DeleteBlogRequest request);
    }
}
