using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
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
        public async Task<ActionResult<List<TrackDTO>>> Get([FromQuery] PaginationDTO paginationDTO, string sortColumn, string sortDir, string search = "")
        {
            IQueryable<Track> queryable;

            if (!string.IsNullOrEmpty(search))
            {
                queryable = repository.GetAll()
                 .Where(m => m.Called.ToString().Contains(search)
                           || m.Date.ToString().Contains(search)
                           || m.Desc.Contains(search)
                           || m.UnCalled.ToString().Contains(search));
            }
            else
            {
                queryable = repository.GetAll();
            }

            await HttpContext.InsertParametersPagintionInHelper(queryable);
            var sortingQueryable = SortFunction(sortColumn, sortDir, queryable);
            var tracks = await sortingQueryable.Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<TrackDTO>>(tracks);
        }

        private IQueryable<Track>? SortFunction(string sortColumn, string sortDir, IQueryable<Track>? list)
        {
            switch (sortColumn)
            {
                case "id":
                    if (sortDir == "desc")
                        list = list.OrderByDescending(c => c.Id);
                    else
                        list = list.OrderBy(c => c.Id);
                    break;
                case "called":
                    if (sortDir == "desc")
                        list = list.OrderByDescending(c => c.Called);
                    else
                        list = list.OrderBy(c => c.Called);
                    break;
                case "unCalled":
                    if (sortDir == "desc")
                        list = list.OrderByDescending(c => c.UnCalled);
                    else
                        list = list.OrderBy(c => c.UnCalled);
                    break;
                case "desc":
                    if (sortDir == "desc")
                        list = list.OrderByDescending(c => c.Desc);
                    else
                        list = list.OrderBy(c => c.Desc);
                    break;
                case "date":
                    if (sortDir == "desc")
                        list = list.OrderByDescending(c => c.Date);
                    else
                        list = list.OrderBy(c => c.Date);
                    break;
            }

            return list;
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

