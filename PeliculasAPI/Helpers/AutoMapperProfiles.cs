using AutoMapper;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Genre
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Genre, GenreCreationDTO>().ReverseMap();

            //Actor
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<Actor, ActorCreationDTO>()
                .ReverseMap()
                .ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<Actor, ActorPatchDTO>().ReverseMap();

            //Movie
            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<Movie, MovieCreationDTO>()
                .ReverseMap()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));
            CreateMap<Movie, MoviePatchDTO>().ReverseMap();
        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();
            if (movieCreationDTO.GenresIds == null) { return result; }
            foreach (var id in movieCreationDTO.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }

            return result;
        }

        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActors>();
            if (movieCreationDTO.Actors == null) { return result; }
            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new MoviesActors() { ActorId = actor.ActorId, Character = actor.Character });
            }

            return result;
        }
    }
}