using AutoMapper;
using TwitterClone_API.Models.AppModels;
using TwitterClone_API.Models.DTOs;

namespace TwitterClone_API.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, ProfileDTO>().ReverseMap();
            CreateMap<AppUser,SearchProfileDTO>().ReverseMap();
            CreateMap<AppUser, UserRegisterDTO>().ReverseMap();
            CreateMap<Tweet, TweetDTO>()
                .ForMember(des => des.LikesCount, option => option.MapFrom(src => src.Likes != null ? src.Likes.Count : 0))
                .ReverseMap();
            CreateMap<Tweet, TweetDTO>()
                .ForMember(des => des.CommentsCount, option => option.MapFrom(src => src.Comments != null ? src.Comments.Count : 0))
                .ReverseMap();
            CreateMap<Tweet, TweetDTO>()
                .ForMember(des => des.UserName, option => option.MapFrom(src => src.User.UserName))
                .ReverseMap();
            CreateMap<AddTweetDTO, Tweet>()
                .ForMember(des => des.CreatedAt, option => option.MapFrom(src => DateTime.Now)).ReverseMap();
            CreateMap<AddCommentDTO, Comment>()
                .ForMember(des => des.CreatedAt, option => option.MapFrom(src => DateTime.Now)).ReverseMap();
            CreateMap<Comment, CommentDTO>()
                .ForMember(des => des.LikesCount, option => option.MapFrom(src => src.Tweet.Likes != null ? src.Tweet.Likes.Count : 0));
            CreateMap<Comment, CommentDTO>()
                .ForMember(des => des.UserName, option => option.MapFrom(src => src.User.UserName)).ReverseMap();
            CreateMap<AppUser, FollowerDTO>()
                .ForMember(des => des.Id, option => option.MapFrom(src => src.Id)).ReverseMap();
        }
    }
}
