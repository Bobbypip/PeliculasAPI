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
            CreateMap<Actor, ActorCreationDTO>().ReverseMap().ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<Actor, ActorPatchDTO>().ReverseMap();
        }
    }
}