using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(destination => destination.PhotoUrl, options => options.MapFrom(source => 
                    source.Photos!.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(destination => destination.AssociatedProjects, options => options.MapFrom(source => 
                    source.AssociatedProjects!.Select(p => p.ProjectName)));
            CreateMap<Project, ProjectDto>()
                .ForMember(destination => destination.CreatorName, options => options.MapFrom(source => 
                    source.Creator.UserName))
                .ForMember(destination => destination.Contributors, options => options.MapFrom(source => 
                    source.Contributors!.Select(u => u.KnownAs)));
            CreateMap<Photo, PhotoDto>();
            CreateMap<Photo, PhotoForApprovalDto>()
                .ForMember(destination => destination.UserName, options => options.MapFrom(source => 
                    source.AppUser.UserName));
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(destination => destination.SenderPhotoUrl, options => options.MapFrom(source => 
                    source.Sender!.Photos!.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(destination => destination.RecipientPhotoUrl, options => options.MapFrom(source => 
                    source.Recipient!.Photos!.FirstOrDefault(x => x.IsMain)!.Url));
        }
    }
}