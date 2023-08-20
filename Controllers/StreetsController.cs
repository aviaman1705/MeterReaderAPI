using AutoMapper;
using MeterReaderAPI.DTO.Track;
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
    public class StreetsController : ControllerBase
    {
        private readonly IStreetRepository repository;
        private readonly IMapper mapper;
        public StreetsController(ApplicationDbContext context, IStreetRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<SysDataTablePager<StreetDTO>>> Get(int page, int itemPerPage, string sortColumn, string sortType, string? search)
        {
            try
            {
                List<StreetDTO> streets = new List<StreetDTO>();
                int totalItems = 0;

                var queryable = repository.GetAll();
                await HttpContext.InsertParametersPagintionInHelper(queryable);
                streets = mapper.Map<List<StreetDTO>>(await queryable.ToListAsync());

                if (!string.IsNullOrEmpty(search))
                {
                    streets = streets
                          .Where(m => m.Title.ToString().Contains(search)).ToList();

                    totalItems = streets.Count;
                    streets = Sort(sortColumn, sortType, streets).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
                }
                else
                {
                    totalItems = streets.Count;
                    streets = Sort(sortColumn, sortType, streets).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
                }

                var streetsPaged = new SysDataTablePager<StreetDTO>(streets, totalItems, itemPerPage, page);
                return streetsPaged;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private List<StreetDTO> Sort(string sortColumn, string sortType, List<StreetDTO> list)
        {
            switch (sortColumn)
            {
                case "title":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Title).ToList();
                    else
                        return list.OrderBy(x => x.Title).ToList();
                default:
                    return list.OrderByDescending(x => x.Id).ToList();
            }
        }

        //[HttpGet("{id:int}")]
        //public ActionResult<TrackDTO> Get(int id)
        //{
        //    var track = repository.Get(id);

        //    if (track == null)
        //    {
        //        return NotFound();
        //    }

        //    var dto = mapper.Map<TrackDTO>(track);
        //    return dto;
        //}
        //[HttpGet("GetDashboardData")]
        //public ActionResult<DashboardDTO> GetDashboardData()
        //{
        //    var dashboardData = repository.GetDashboardData();

        //    if (dashboardData == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = mapper.Map<DashboardDTO>(dashboardData);
        //    return model;
        //}

        //[HttpPut("{id:int}")]
        //public ActionResult Put(int id, [FromForm] TrackDTO trackDTO)
        //{
        //    var track = repository.Get(id);

        //    if (track == null)
        //    {
        //        return NotFound();

        //    }

        //    track = mapper.Map<Track>(trackDTO);

        //    repository.Update(track);
        //    return NoContent();
        //}

        //[HttpDelete("{id:int}")]
        //public ActionResult Delete(int id)
        //{
        //    bool isDeleted = repository.Delete(id);

        //    if (isDeleted)
        //    {
        //        return NoContent();
        //    }
        //    return NotFound();
        //}

        //[HttpPost]
        //public async Task<ActionResult<int>> Post([FromForm] TrackCreationDTO trackCreationDTO)
        //{
        //    try
        //    {
        //        var track = mapper.Map<Track>(trackCreationDTO);
        //        repository.Add(track);
        //        return track.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


    }
}
