using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OdysseyPortfolio_Libraries.Constants;
using OdysseyPortfolio_Libraries.Entities;
using OdysseyPortfolio_Libraries.Migrations;
using OdysseyPortfolio_Libraries.Repositories;
using OdysseyPortfolio_Libraries.Services;
using OdysseyPortfolio_Libraries.Services.Implementations.BlogService;
using OdysseyPortfolio_Libraries.Services.Implementations.CommentLikeService;
using OdysseyPortfolio_Libraries.Services.Implementations.CommentService;
using OdysseyPortfolio_Libraries.Services.Implementations.UserService;

namespace OdysseyPortfolio_BE.Extensions
{

    public static class ServiceExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var environment = config["ASPNETCORE_ENVIRONMENT"];
            services.AddDbContext<OdysseyPortfolioDbContext>(
                options => options.UseNpgsql(config[$"ConnectionStrings:{environment}"]));
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICommentLikeService, CommentLikeService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
        public static IServiceCollection AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy(name: ServiceExtensionsConstants.ODYSSEY_PORTFOLIO_LOCAL_CORS,
                    policy =>
                        {
                            policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000",
                                "https://odyssey-portfolio.vercel.app")
                            .AllowAnyHeader().AllowAnyMethod()
                            .AllowCredentials();
                        })
            );

            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
            }).AddEntityFrameworkStores<OdysseyPortfolioDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = config["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(config["JWT:SigningKey"]))
                };
                //Retrieves Cookies on receiving request from Frontend
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                        if (!string.IsNullOrEmpty(accessToken))
                            ctx.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    new string[] { }
                }
            });
            });
            return services;
        }
    }

}

