using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly ITrackRepository repository;        
        private readonly IMapper mapper;
        public TracksController(ApplicationDbContext context, ITrackRepository repository, IMapper mapper)
        {
            this.repository = repository;            
            this.mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<SysDataTablePager<TrackDTO>>> Get(int page, int itemPerPage, string sortColumn, string sortType)
        {
            var queryable = repository.GetAll();
            await HttpContext.InsertParametersPagintionInHelper(queryable);
            var tracks = mapper.Map<List<TrackDTO>>(await queryable.ToListAsync());
            int totalItems = tracks.Count;
            tracks = Sort(sortColumn, sortType, tracks).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();

            var tracksPaged = new SysDataTablePager<TrackDTO>(tracks, totalItems, itemPerPage, page);
            return tracksPaged;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrackDTO>> Get(int id)
        {
            var track = await repository.Get(id);

            if (track == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<TrackDTO>(track);            
            return dto;
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] TrackCreationDTO trackCreationDTO)
        {
            var track = await repository.Get(id);

            if (track == null)
            {
                return NotFound();

            }

            track = mapper.Map(trackCreationDTO, track);

            await repository.Update(track);
            return NoContent();
        }
        private List<TrackDTO> Sort(string sortColumn, string sortType, List<TrackDTO> list)
        {
            switch (sortColumn)
            {
                case "id":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Id).ToList();
                    else
                        return list.OrderBy(x => x.Id).ToList();
                case "called":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Called).ToList();
                    else
                        return list.OrderBy(x => x.Called).ToList();
                case "uncalled":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.UnCalled).ToList();
                    else
                        return list.OrderBy(x => x.UnCalled).ToList();
                case "date":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Date).ToList();
                    else
                        return list.OrderBy(x => x.Date).ToList();
                default:
                    return list.OrderByDescending(x => x.Id).ToList();
            }
        }
    }
}
