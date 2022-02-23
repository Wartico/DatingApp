using API.Entities;
using API.Extensions;
using API.Models;
using AutoMapper;
using System.Linq;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberResponse>()
                .ForMember(dest => dest.PhotoUrl, opt => opt
                    .MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt
                .MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoResponse>();

            CreateMap<MemberUpdateRequest, AppUser>();

            CreateMap<NewRegisterRequest, AppUser>();

            CreateMap<Message, MessageResponse>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt
                    .MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt
                    .MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}
