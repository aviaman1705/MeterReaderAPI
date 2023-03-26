using AutoMapper;

namespace MeterReaderAPI.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        //public AutoMapperProfiles(GeometryFactory geometryFactory)
        //{
        //    CreateMap<GenreDTO, Genre>().ReverseMap();
        //    CreateMap<GenreCreationDTO, Genre>();

        //    CreateMap<ActorDTO, Actor>().ReverseMap();
        //    CreateMap<ActorCreationDTO, Actor>();

        //    CreateMap<MovieTheater, MovieTheaterDTO>()
        //        .ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
        //        .ForMember(x => x.Longitude, dto => dto.MapFrom(prop => prop.Location.X));

        //    CreateMap<MovieTheaterCreationDTO, MovieTheater>()
        //        .ForMember(x => x.Location, x => x.MapFrom(dto =>
        //        geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));


        //    CreateMap<MovieCreationDTO, Movie>()
        //        .ForMember(x => x.Poster, options => options.Ignore())
        //        .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
        //        .ForMember(x => x.MovieTheatersMovies, options => options.MapFrom(MapMovieTheatersMovies))
        //        .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMovieActors));


        //    CreateMap<Movie, MovieDTO>()
        //        .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
        //        .ForMember(x => x.MovieTheaters, options => options.MapFrom(MapMovieTheatersMovies))
        //         .ForMember(x => x.Actors, options => options.MapFrom(MapMoviesActors));
        //}
    }
}
