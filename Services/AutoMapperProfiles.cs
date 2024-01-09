using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.DTO.Track;
using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Services
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Track, SearchDTO>()
                .ForMember(x => x.Title, dto => dto.MapFrom(prop => prop.Desc))
                .ForMember(x => x.Link, dto => dto.MapFrom(prop => $"/tracks/edit/{prop.Id}"));
            CreateMap<Track,TrackDTO>().ReverseMap();
            CreateMap<Track, TrackCreationDTO>().ReverseMap();
            CreateMap<Track,EditTrackDTO>().ReverseMap();


            CreateMap<Notebook, ListItemDTO>()
               .ForMember(x => x.Text, dto => dto.MapFrom(prop => prop.Number.ToString()))
               .ForMember(x => x.Value, dto => dto.MapFrom(prop => prop.Number));
            
            CreateMap<Dashboard,DashboardDTO>().ReverseMap();
            CreateMap<DashboardSummary, DashboardSummaryDTO>().ReverseMap();
            CreateMap<MonthlyData, MonthlyDataDTO>().ReverseMap();
        }
    }
}
