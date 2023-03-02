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
    public class NotebooksController : ControllerBase
    {
        private readonly INotebookRepository repository;
        private ApplicationDbContext context;
        private readonly IMapper mapper;
        public NotebooksController(ApplicationDbContext context, INotebookRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<SysDataTablePager<NotebookDTO>>> Get(int page, int itemPerPage, string sortColumn, string sortType)
        {
            var notebooks = mapper.Map<List<NotebookDTO>>(await context.Notebooks.ToListAsync());
            int totalItems = notebooks.Count;
            notebooks = Sort(sortColumn, sortType, notebooks).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();

            var notebooksPaged = new SysDataTablePager<NotebookDTO>(notebooks, totalItems, itemPerPage, page);
            return notebooksPaged;
        }

        private List<NotebookDTO> Sort(string sortColumn, string sortType, List<NotebookDTO> list)
        {
            switch (sortColumn)
            {
                case "id":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Id).ToList();
                    else
                        return list.OrderBy(x => x.Id).ToList();
                case "number":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Number).ToList();
                    else
                        return list.OrderBy(x => x.Number).ToList();
                case "streetName":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.StreetName).ToList();
                    else
                        return list.OrderBy(x => x.StreetName).ToList();
                default:
                    return list.OrderByDescending(x => x.Id).ToList();
            }
        }
    }
}
