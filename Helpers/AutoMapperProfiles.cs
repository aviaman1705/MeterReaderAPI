using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.DTO.Notebook;
using MeterReaderAPI.DTO.Track;
using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            //Track
            CreateMap<Track, TrackGridItem>();
            
            //Notebook
            CreateMap<Notebook, NotebookDTO>()
                .ForMember(dest => dest.TracksCount, opt => opt.MapFrom(src => src.Tracks.Count()));
            CreateMap<NotebookCreationDTO, Notebook>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number));
            CreateMap<EditNotebookDTO, Notebook>()
               .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number));
        }
    }
}
