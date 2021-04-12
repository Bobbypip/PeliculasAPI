﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CustomBaseController
            (
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>() where TEntity : class
        {
            var entities = await _context.Set<TEntity>().AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<List<TDTO>>(entities);

            return dtos;
        }

        protected async Task<ActionResult<TDTO>> Get<TEntity, TDTO>(int id) where TEntity : class, IId
        {
            var entity = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return _mapper.Map<TDTO>(entity);
        }

        protected async Task<ActionResult> Post<TCreation, TEntity, TReading>
            (
            TCreation creationDTO,
            string pathName
            )
            where TEntity : class, IId
        {
            var entity = _mapper.Map<TEntity>(creationDTO);
            _context.Add(entity);
            await _context.SaveChangesAsync();
            var readingDTO = _mapper.Map<TReading>(entity);

            return new CreatedAtRouteResult(pathName, new { id = entity.Id }, readingDTO);
        }

        protected async Task<ActionResult> Put<TCreation, TEntity>
            (
            int id,
            TCreation creationDTO
            )
            where TEntity : class, IId
        {
            var entity = _mapper.Map<TEntity>(creationDTO);
            entity.Id = id;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Delete<TEntity>(int id) where TEntity : class, IId, new()
        {
            var exist = await _context.Set<TEntity>().AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            _context.Remove(new TEntity() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}