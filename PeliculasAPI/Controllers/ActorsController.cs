using AutoMapper;
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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : CustomBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesStorage _filesStorage;
        private readonly string container = "actors";

        public ActorsController(
            ApplicationDbContext context,
            IMapper mapper,
            IFilesStorage filesStorage
            ): base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _filesStorage = filesStorage;
        }

        // GET: api/<ActorsController>
        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            return await Get<Actor, ActorDTO>(paginationDTO);
        }

        // GET api/<ActorsController>/5
        [HttpGet("{id}", Name = "getActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            return await Get<Actor, ActorDTO>(id);
        }

        // POST api/<ActorsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var entity = _mapper.Map<Actor>(actorCreationDTO);

            if (actorCreationDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreationDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreationDTO.Photo.FileName);
                    entity.Photo = await _filesStorage.SaveFile(content, extension, container, actorCreationDTO.Photo.ContentType);
                }
            }

            _context.Add(entity);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<ActorDTO>(entity);

            return new CreatedAtRouteResult("getActor", new { id = entity.Id }, dto);
        }

        // PUT api/<ActorsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actorDB = await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actorDB == null) { return NotFound(); }

            actorDB = _mapper.Map(actorCreationDTO, actorDB);

            if (actorCreationDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreationDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreationDTO.Photo.FileName);
                    actorDB.Photo = await _filesStorage.EditFile(content, extension, container, actorDB.Photo, actorCreationDTO.Photo.ContentType);
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            return await Patch<Actor, ActorPatchDTO>(id, patchDocument);
        }

        // DELETE api/<ActorsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Actor>(id);
        }
    }
}
