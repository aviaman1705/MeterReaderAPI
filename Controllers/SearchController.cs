using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ITrackRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<SearchController> logger;

        public SearchController(ITrackRepository repository, IMapper mapper, ILogger<SearchController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;   
        }

        [HttpGet]
        public async Task<ActionResult<SysDataTablePager<SearchDTO>>> Get(int page, int itemsPerPage, string term)
        {
            try
            {
                var queryable = repository.GetAll();
                var searchResults = mapper.Map<List<SearchDTO>>(await queryable.Where(x => x.Desc.Contains(term)).ToListAsync());

                int totalItems = searchResults.Count;
                searchResults = searchResults.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

                var searchResultPaged = new SysDataTablePager<SearchDTO>(searchResults, totalItems, itemsPerPage, page);
                logger.LogInformation($"Searches list retrived");
                return searchResultPaged;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
