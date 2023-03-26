using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ITrackRepository repository;
        private readonly IMapper mapper;
        public SearchController(ITrackRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("example/{term}")]
        public async Task<ActionResult<List<SearchDTO>>> Get(string term)
        {
            var searchResults = await repository.GetAll().Where(x => x.Desc.Contains(term)).ToListAsync();

            if (searchResults == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<List<SearchDTO>>(searchResults);
            return dto;
        }
    }
}
