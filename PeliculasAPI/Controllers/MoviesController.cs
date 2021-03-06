﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entities;
using PeliculasAPI.Helpers;
using PeliculasAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Logging;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : CustomBaseController
    {
        private readonly IFilesStorage _filesStorage;
        private readonly ILogger<MoviesController> _logger;
        private readonly string container = "movies";

        public MoviesController
            (
            ApplicationDbContext context,
            IMapper mapper,
            IFilesStorage filesStorage,
            ILogger<MoviesController> logger
            )
            : base(context, mapper)
        {
            _filesStorage = filesStorage;
            _logger = logger;
        }

        // GET: MoviesController
        [HttpGet]
        public async Task<ActionResult<IndexMoviesDTO>> Get()
        {
            var top = 5;
            var today = DateTime.Today;

            var commingReleases = await _context.Movies
                .Where(x => x.ReleaseDate > today)
                .OrderBy(x => x.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var onCinemas = await _context.Movies
                .Where(x => x.OnCinemas)
                .Take(top)
                .ToListAsync();

            var result = new IndexMoviesDTO();
            result.FutureReleases = _mapper.Map<List<MovieDTO>>(commingReleases);
            result.OnCinemas = _mapper.Map<List<MovieDTO>>(onCinemas);

            return result;

            //var movies = await _context.Movies.ToListAsync();
            //return _mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDTO>>> Filter([FromQuery] FilterMoviesDTO filterMoviesDTO)
        {
            var moviesQueryable = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(filterMoviesDTO.Title))
            {
                moviesQueryable = moviesQueryable.Where(x => x.Title.Contains(filterMoviesDTO.Title));
            }

            if (filterMoviesDTO.OnCinemas)
            {
                moviesQueryable = moviesQueryable.Where(x => x.OnCinemas);
            }

            if (filterMoviesDTO.FutureReleases)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate > today);
            }

            if (filterMoviesDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesGenres.Select(y => y.GenreId)
                    .Contains(filterMoviesDTO.GenreId));
            }

            if (!string.IsNullOrEmpty(filterMoviesDTO.SortField))
            {
                var orderType = filterMoviesDTO.AscendingOrder ? "ascending" : "descending";

                try
                {
                    // String Interpolation
                    moviesQueryable = moviesQueryable.OrderBy($"{filterMoviesDTO.SortField} {orderType}");
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }

                #region OrderBy with if
                //if (filterMoviesDTO.SortField == "title")
                //{
                //    if (filterMoviesDTO.AscendingOrder)
                //    {
                //        moviesQueryable = moviesQueryable.OrderBy(x => x.Title);
                //    }
                //    else
                //    {
                //        moviesQueryable = moviesQueryable.OrderByDescending(x => x.Title);
                //    }
                //}
                #endregion
            }

            await HttpContext.InsertPaginationParameters(moviesQueryable,
                filterMoviesDTO.recordsQuantityPerPage);

            var movies = await moviesQueryable
                .Paginate(filterMoviesDTO.Pagination)
                .ToListAsync();

            return _mapper.Map<List<MovieDTO>>(movies);
        }

        // GET: MoviesController/5
        [HttpGet("{id}", Name = "getMovie")]
        public async Task<ActionResult<MovieDetailsDTO>> Get(int id)
        {
            var movie = await _context.Movies
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.MoviesActors = movie.MoviesActors.OrderBy(x => x.Sort).ToList();

            return _mapper.Map<MovieDetailsDTO>(movie);
        }

        // POST: MoviesController/Create
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreationDTO movieCreationDTO)
        {
            var entity = _mapper.Map<Movie>(movieCreationDTO);

            if (movieCreationDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                    entity.Poster = await _filesStorage.SaveFile(content, extension, container, movieCreationDTO.Poster.ContentType);
                }
            }

            AsignActrorsOrder(entity);

            _context.Add(entity);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<MovieDTO>(entity);

            return new CreatedAtRouteResult("getMovie", new { id = entity.Id }, dto);
        }

        private void AsignActrorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Sort = i;
                }
            }
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movieDB = await _context.Movies
                .Include(x => x.MoviesActors)
                .Include(x => x.MoviesGenres)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movieDB == null) { return NotFound(); }

            movieDB = _mapper.Map(movieCreationDTO, movieDB);

            if (movieCreationDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                    movieDB.Poster = await _filesStorage.EditFile(content, extension, container, movieDB.Poster, movieCreationDTO.Poster.ContentType);
                }
            }

            AsignActrorsOrder(movieDB);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            return await Patch<Movie, MoviePatchDTO>(id, patchDocument);
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Movie>(id);
        }
    }
}
