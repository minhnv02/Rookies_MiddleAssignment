using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MidAssignment.Application.Services.Impl;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Interfaces;
using MidAssignment.Infrastructure.Helpers;
using MidAssignment.Infrastructure.UnitOfWork;
using MidAssignment.Application.Configurations;

namespace MidAssignment.Application
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<ICryptographyHelper, CryptographyHelper>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBorrowingService, BorrowingService>();
            services.AddScoped<IBorrowingDetailService, BorrowingDetailService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<ICartService, CartService>();
        }
    }
}
