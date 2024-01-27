using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.DTO.User;
using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Services
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            //Track
            CreateMap<Track, TrackDTO>().ReverseMap();
            CreateMap<Track, TrackCreationDTO>().ReverseMap();
            CreateMap<Track, SearchDTO>()
                .ForMember(x => x.Title, dto => dto.MapFrom(prop => prop.Desc))
                .ForMember(x => x.Link, dto => dto.MapFrom(prop => $"/tracks/edit/{prop.Id}"));

            //Dashboard
            CreateMap<Dashboard,DashboardDTO>().ReverseMap();
            CreateMap<DashboardSummary, DashboardSummaryDTO>().ReverseMap();
            CreateMap<MonthlyData, MonthlyDataDTO>().ReverseMap();

            //User
            CreateMap<RegisterDTO,UserCredentials>().ReverseMap();
        }
    }
}
