using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
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

        [HttpGet]
        public async Task<ActionResult<SysDataTablePager<SearchDTO>>> Get(int page, int itemPerPage, string term)
        {
            var queryable = repository.GetAll();
            var searchResults = mapper.Map<List<SearchDTO>>(await queryable.Where(x => x.Desc.Contains(term)).ToListAsync());
          
            int totalItems = searchResults.Count;
            searchResults = searchResults.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();

            var searchResultPaged = new SysDataTablePager<SearchDTO>(searchResults, totalItems, itemPerPage, page);
            return searchResultPaged;
        }
    }
}
