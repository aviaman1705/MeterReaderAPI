using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Track, TrackDTO>()
                .ForMember(x => x.Date, dto => dto.MapFrom(prop => prop.Date.ToString("MM/dd/yyyy")));

            CreateMap<Track, SearchDTO>()
                .ForMember(x => x.Title, dto => dto.MapFrom(prop => prop.Desc))
                .ForMember(x => x.Url, dto => dto.MapFrom(prop => $"/tracks/edit/{prop.Id}"));
        }
    }
}
