using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Services
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<NotebookDTO,Notebook>().ReverseMap();
            CreateMap<Track,TrackDTO>().ReverseMap();   
        }
    }
}
