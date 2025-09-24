using AutoMapper;
using OdysseyPortfolio_Libraries.DTOs;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Payloads.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateBlogRequest, Entities.Blog>()
                .ForMember(blog => blog.Image, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Blog, GetBlogDto>()
                .ReverseMap();
            CreateMap<Blog, GetBlogDtoAdmin>()
                .ReverseMap();
            CreateMap<Blog, GetBlogByIdDto>()
               .ReverseMap();
            CreateMap<RegisterRequest, Entities.User>()
                .ReverseMap();
            CreateMap<UpdateBlogRequest, Entities.Blog>()
                .ForMember(blog => blog.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
