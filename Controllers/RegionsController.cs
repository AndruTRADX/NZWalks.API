using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger) : ControllerBase
    {
        private readonly IRegionRepository regionRepository = regionRepository;
        private readonly IMapper mapper = mapper;
        private readonly ILogger<RegionsController> logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAll Regions action method was invoked");

            var regionDomainModel = await regionRepository.GetAllAsync();

            logger.LogInformation($"Finished GetAll Regions request with data: {JsonSerializer.Serialize(regionDomainModel)}");

            var regionsDto = mapper.Map<List<RegionDTO>>(regionDomainModel);

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var regionDomainModel = await regionRepository.GetByIdAsync(id);

            if (regionDomainModel == null) {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDto);
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionDTO addRegionDTO)
        {
            var regionDomainModelToCreate = mapper.Map<Region>(addRegionDTO);

            var regionDomainModel = await regionRepository.CreateAsync(regionDomainModelToCreate);

            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AddRegionDTO addRegionDTO)
        {
            var regionDomainModelToUpdate = mapper.Map<Region>(addRegionDTO);
                var regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModelToUpdate);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}