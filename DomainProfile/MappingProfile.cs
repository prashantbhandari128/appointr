using Appointr.DTO;
using Appointr.Persistence.Entities;
using AutoMapper;

namespace Appointr.DomainProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Define object-to-object mapping here
            CreateMap<Activity, ActivityDto>();
            CreateMap<ActivityDto, Activity>();
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<AppointmentDto, Appointment>();
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            CreateMap<Visitor, VisitorDto>();
            CreateMap<VisitorDto, Visitor>();
            CreateMap<Officer, OfficerDto>();
            CreateMap<OfficerDto, Officer>();
        }
    }
}
