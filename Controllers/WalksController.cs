﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController(IWalkRepository walkRepository,IMapper mapper) : ControllerBase
    {
        private readonly IWalkRepository walkRepository = walkRepository;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn, 
            [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, 
            [FromQuery] bool? isAscending,
            [FromQuery] int size = 32,
            [FromQuery] int offset = 1
        )
        {
            var walksDomainModel = await walkRepository.GetAllAsync(
                filterOn, 
                filterQuery, 
                sortBy, 
                isAscending ?? true, 
                size, 
                offset
            );
            return Ok(mapper.Map<List<WalkDTO>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walksDomainModel = await walkRepository.GetByIdAsync(id);

            if (walksDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walksDomainModel));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkDTO addWalkDTO)
        {
            var walkDomainModel = mapper.Map<Walk>(addWalkDTO);
            await walkRepository.CreateAsync(walkDomainModel);

            return Ok(mapper.Map<WalkDTO>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AddWalkDTO addWalkDTO)
        {
            var walkDomainModel = mapper.Map<Walk>(addWalkDTO);
            var walkUpdated = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkUpdated == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walkUpdated));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
