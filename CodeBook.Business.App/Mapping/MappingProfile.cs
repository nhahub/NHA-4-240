using System;
using System.Linq;
using AutoMapper;
using CodeBook.Business.App.DTOs;
using CodeBook.Models.App;

namespace CodeBook.Business.App.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // map notification to notificationDTO
            CreateMap<Notification, NotificationDTO>()
               .ForMember(dest => dest.Type,
                          opt => opt.MapFrom(src => src.Type.ToString()))
               .ForMember(dest => dest.IsSeen,
                          opt => opt.MapFrom(src => src.IsSeen))
               .ForMember(dest => dest.DateCreated,
                          opt => opt.MapFrom(src => src.DateCreated));

            CreateMap<Report, ReportDTO>()
                .ForMember(dest => dest.Status,
                            opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.AuthorUsername,
                           opt => opt.MapFrom(src => src.Author.UserName));

            CreateMap<User, UserProfileResponse>()
                .ForMember(dest => dest.UserName,
                           opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Bio,
                           opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.AvatarUrl,
                            opt => opt.MapFrom(src => src.AvatarUrl))
                .ForMember(dest => dest.JoinedAt,
                            opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.Id,
                            opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Role,
                            opt => opt.MapFrom(src => src.Role));

            CreateMap<UpdateProfileDto, User>()
                .ForMember(dest => dest.Bio,
                           opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.AvatarUrl,
                           opt => opt.MapFrom(src => src.AvatarUrl));

            CreateMap<Community, CommunityDto>()
                .ForMember(dest => dest.communityId,
                            opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerId,
                            opt => opt.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description,
                            opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DateCreated,
                            opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.memberscount,
                            opt => opt.MapFrom(src => src.Members.Count))
                .ForMember(dest => dest.IconURL,
                            opt => opt.MapFrom(src => src.IconURL));

            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.LikeCount,
               opt => opt.MapFrom(src => src.LikeCount));

            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.AuthorUsername,
               opt => opt.MapFrom(src => src.Author.UserName)) 
                .ForMember(dest => dest.AuthorId,
               opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.CommunityName,
               opt => opt.MapFrom(src => src.Community != null
                                        ? src.Community.Name
                                        : null));

        }

    }
}
