using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
                .ForMember(destination => destination.Age, options => options.MapFrom(source =>
                source.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(destination => destination.SenderPhotoUrl, options => options.MapFrom(source => 
                    source.Sender!.Photos.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(destination => destination.RecipientPhotoUrl, options => options.MapFrom(source => 
                    source.Recipient!.Photos.FirstOrDefault(x => x.IsMain)!.Url));
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}