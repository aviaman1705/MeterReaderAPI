using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.DTO.Track;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Xml.Linq;

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
        public async Task<ActionResult<SysDataTablePager<TrackGridItem>>> Get(int page, int itemPerPage, string sortColumn, string sortType, string? search)
        {
            List<TrackGridItem> tracks = new List<TrackGridItem>();
            int totalItems = 0;

            //1 טעינת האייטמים ממסד הנתונים
            var queryable = repository.GetAll();

            //2 מעדכן סה"כ אייטמים בהאדר הבקשה
            await HttpContext.InsertParametersPagintionInHelper(queryable);

            //המרת השאילתה למערך 3  
            tracks = mapper.Map<List<TrackGridItem>>(await queryable.ToListAsync());

            //4 בדיקה אם הוזן טקסט בשה חיפוש
            if (!string.IsNullOrEmpty(search))
            {
                tracks = tracks
                      .Where(m => m.Called.ToString().Contains(search)
                           || m.Date.ToString("dd/MM/yyyy").Contains(search)
                           || m.Desc.Contains(search)
                           || m.UnCalled.ToString().Contains(search)).ToList();

                totalItems = tracks.Count;
                tracks = Sort(sortColumn, sortType, tracks).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
            }
            else
            {
                totalItems = tracks.Count;
                tracks = Sort(sortColumn, sortType, tracks).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
            }
            
            var tracksPaged = new SysDataTablePager<TrackGridItem>(tracks, totalItems, itemPerPage, page);
            return tracksPaged;
        }

        [HttpGet("{id:int}")]
        public ActionResult<TrackDTO> Get(int id)
        {
            var track = repository.Get(id);

            if (track == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<TrackDTO>(track);
            return dto;
        }
        [HttpGet("GetDashboardData")]
        public ActionResult<DashboardDTO> GetDashboardData()
        {
            var dashboardData = repository.GetDashboardData();

            if (dashboardData == null)
            {
                return NotFound();
            }

            var model = mapper.Map<DashboardDTO>(dashboardData);
            return model;
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromForm] TrackDTO trackDTO)
        {
            var track = repository.Get(id);

            if (track == null)
            {
                return NotFound();

            }

            track = mapper.Map<Track>(trackDTO);

            repository.Update(track);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            bool isDeleted = repository.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] TrackCreationDTO trackCreationDTO)
        {
            try
            {
                var track = mapper.Map<Track>(trackCreationDTO);
                repository.Add(track);              
                return track.Id;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private List<TrackGridItem> Sort(string sortColumn, string sortType, List<TrackGridItem> list)
        {
            switch (sortColumn)
            {
                case "date":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Date).ToList();
                    else
                        return list.OrderBy(x => x.Date).ToList();
                case "desc":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Desc).ToList();
                    else
                        return list.OrderBy(x => x.Desc).ToList();
                case "called":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Called).ToList();
                    else
                        return list.OrderBy(x => x.Called).ToList();
                case "unCalled":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.UnCalled).ToList();
                    else
                        return list.OrderBy(x => x.UnCalled).ToList();
                default:
                    return list.OrderByDescending(x => x.Id).ToList();
            }
        }
    }
}
