using AutoMapper;
using MidAssignment.Application.Models.Responses;
using MidAssignment.Domain.Entities;

namespace MidAssignment.Application.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Book, BookResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => src.Ratings))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.BorrowingDetails, opt => opt.MapFrom(src => src.BorrowingDetails))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings.Count == 0 ? 0 : src.Ratings.Average(r => r.Rate)));

            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));

            CreateMap<Rating, RatingResponse>()
                .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));

            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));

            CreateMap<Borrowing, BorrowingResponse>()
                .ForMember(dest => dest.BorrowingDetails, opt => opt.MapFrom(src => src.BorrowingDetails))
                .ForMember(dest => dest.RequestorName, opt => opt.MapFrom(src => src.Requestor.Name))
                .ForMember(dest => dest.ApproverName, opt => opt.MapFrom(src => src.Approver.Name));

            CreateMap<BorrowingDetail, BorrowingDetailResponse>()
                .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Book.Image))
                .ForMember(dest => dest.DaysForBorrow, opt => opt.MapFrom(src => src.Book.DaysForBorrow))
                .ForMember(dest => dest.RequestorName, opt => opt.MapFrom(src => src.Borrowing.Requestor.Name));
        }
    }
}
