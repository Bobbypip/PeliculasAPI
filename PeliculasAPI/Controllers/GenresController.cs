using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : CustomBaseController
    {
        // Soy el basado
        public GenresController(
            ApplicationDbContext context,
            IMapper mapper)
            : base(context, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            return await Get<Genre, GenreDTO>();
        }

        [HttpGet("{id:int}", Name = "getGenre")]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            return await Get<Genre, GenreDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreationDTO)
        {
            return await Post<GenreCreationDTO, Genre, GenreDTO>(genreCreationDTO, "getGenre");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreationDTO)
        {
            return await Put<GenreCreationDTO, Genre>(id, genreCreationDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Genre>(id);
        }

    }
}
