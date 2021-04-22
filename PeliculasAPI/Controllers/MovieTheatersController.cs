using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [Route("api/movietheaters")]
    [ApiController]
    public class MovieTheatersController : CustomBaseController
    {
        private readonly GeometryFactory _geometryFactory;
        public MovieTheatersController
            (
            ApplicationDbContext context,
            IMapper mapper,
            GeometryFactory geometryFactory
            )
            : base(context, mapper)
        {
            _geometryFactory = geometryFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieTheaterDTO>>> Get()
        {
            return await Get<MovieTheater, MovieTheaterDTO>();
        }

        [HttpGet("{id:int}", Name = "getMovieTheater")]
        public async Task<ActionResult<MovieTheaterDTO>> Get(int id)
        {
            return await Get<MovieTheater, MovieTheaterDTO>(id);
        }

        [HttpGet("closers")]
        public async Task<ActionResult<List<MovieTheaterCloseDTO>>> Closers
            (
                [FromQuery] MovieTheaterCloseFilterDTO filter
            )
        {
            var userLocation = _geometryFactory.CreatePoint(new Coordinate(filter.Longitude, filter.Latitude));

            var movieTheaters = await _context.MovieTheaters
                .OrderBy(x => x.Location.Distance(userLocation))
                .Where(x => x.Location.IsWithinDistance(userLocation, filter.DistanceInKM * 1000))
                .Select(x => new MovieTheaterCloseDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Latitude = x.Location.Y,
                    Longitude = x.Location.X,
                    DistanceInMts = Math.Round(x.Location.Distance(userLocation))
                })
                .ToListAsync();

            return movieTheaters;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MovieTheaterCreationDTO movieTheaterCreationDTO)
        {
            return await Post<MovieTheaterCreationDTO, MovieTheater, MovieTheaterDTO>(movieTheaterCreationDTO, "getMovieTheater");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] MovieTheaterCreationDTO movieTheaterCreationDTO)
        {
            return await Put<MovieTheaterCreationDTO, MovieTheater>(id, movieTheaterCreationDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<MovieTheater>(id);
        }
    }
}
