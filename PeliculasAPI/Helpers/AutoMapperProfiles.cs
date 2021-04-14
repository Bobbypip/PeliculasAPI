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

            //MovieTheater
            CreateMap<MovieTheater, MovieTheaterDTO>().ReverseMap();
            CreateMap<MovieTheaterCreationDTO, MovieTheater>().ReverseMap();

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
            CreateMap<MovieDetailsDTO, Movie>()
                .ReverseMap()
                .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.Actors, options => options.MapFrom(MapMoviesActors));
            CreateMap<Movie, MoviePatchDTO>().ReverseMap();
        }

        private List<ActorMovieDetailDTO> MapMoviesActors(Movie movie, MovieDetailsDTO movieDetailsDTO)
        {
            var result = new List<ActorMovieDetailDTO>();
            if (movie.MoviesActors == null) { return result; }
            foreach (var actorMovie in movie.MoviesActors)
            {
                result.Add(new ActorMovieDetailDTO
                {
                    ActorId = actorMovie.ActorId,
                    Character = actorMovie.Character,
                    NamePerson = actorMovie.Actor.Name
                });
            }

            return result;
        }

        private List<GenreDTO> MapMoviesGenres(Movie movie, MovieDetailsDTO movieDetailsDTO)
        {
            var result = new List<GenreDTO>();
            if (movie.MoviesGenres == null) { return result; }
            foreach (var genreMovie in movie.MoviesGenres)
            {
                result.Add(new GenreDTO() { Id = genreMovie.GenreId, Name = genreMovie.Genre.Name });
            }

            return result;
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