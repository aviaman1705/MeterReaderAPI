using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.DTO.Notebook;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotebooksController : ControllerBase
    {
        private readonly INotebookRepository repository;
        private readonly IMapper mapper;
        public NotebooksController(ApplicationDbContext context, INotebookRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("employee")]
        public async Task<ActionResult<SysDataTablePager<NotebookDTO>>> GetRecord(int page, int sizePerPage, string sortField, string sortOrder, string? search)
        {
            List<NotebookDTO> notebooks = new List<NotebookDTO>();
            int totalItems = 0;

            var queryable = repository.GetAll();
            await HttpContext.InsertParametersPagintionInHelper(queryable);
            notebooks = mapper.Map<List<NotebookDTO>>(await queryable.ToListAsync());

            if (!string.IsNullOrEmpty(search))
            {
                notebooks = notebooks.Where(m => m.Number.ToString().Contains(search)).ToList();
                totalItems = notebooks.Count;
                notebooks = Sort(sortField, sortOrder, notebooks).Skip((page - 1) * sizePerPage).Take(sizePerPage).ToList();
            }
            else
            {
                totalItems = notebooks.Count;
                notebooks = Sort(sortField, sortOrder, notebooks).Skip((page - 1) * sizePerPage).Take(sizePerPage).ToList();
            }

            var notebooksPaged = new SysDataTablePager<NotebookDTO>(notebooks, totalItems, sizePerPage, page);
            return notebooksPaged;
        }

        [HttpGet("")]
        public async Task<ActionResult<SysDataTablePager<NotebookDTO>>> Get(int page, int itemPerPage, string sortColumn, string sortDirection, string? search)
        {
            try
            {
                List<NotebookDTO> notebooks = new List<NotebookDTO>();
                int totalItems = 0;

                var queryable = repository.GetAll();
                await HttpContext.InsertParametersPagintionInHelper(queryable);
                notebooks = mapper.Map<List<NotebookDTO>>(await queryable.ToListAsync());

                if (!string.IsNullOrEmpty(search))
                {
                    notebooks = notebooks.Where(m => m.Number.ToString().Contains(search)).ToList();

                    totalItems = notebooks.Count;
                    notebooks = Sort(sortColumn, sortDirection, notebooks).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
                }
                else
                {
                    totalItems = notebooks.Count;
                    notebooks = Sort(sortColumn, sortDirection, notebooks).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
                }

                var streetsPaged = new SysDataTablePager<NotebookDTO>(notebooks, totalItems, itemPerPage, page);
                return streetsPaged;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNotebooks")]
        public ActionResult<ListItemDTO[]> GetNotebooks()
        {
            var notebooks = repository.GetAll().OrderBy(x => x.Number).Select(x => new ListItemDTO() { Text = x.Number.ToString(), Value = x.Id }).ToList();

            if (notebooks == null)
            {
                return NotFound();
            }

            var model = mapper.Map<ListItemDTO[]>(notebooks);
            return model;
        }

        [HttpGet("{id:int}")]
        public ActionResult<NotebookDTO> Get(int id)
        {
            var notebook = repository.Get(id);

            if (notebook == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<NotebookDTO>(notebook);
            return dto;
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromForm] EditNotebookDTO notebookDTO)
        {
            var notebook = repository.Get(id);

            if (notebook == null)
            {
                return NotFound();

            }

            notebook = mapper.Map<Notebook>(notebookDTO);

            repository.Update(notebook);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] NotebookCreationDTO notebookCreationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    return BadRequest(allErrors);
                }

                var notebook = mapper.Map<Notebook>(notebookCreationDTO);
                repository.Add(notebook);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return BadRequest("אירעה שגיאה בשרת.");
            }
        }

        private List<NotebookDTO> Sort(string sortColumn, string sortDirection, List<NotebookDTO> list)
        {
            switch (sortColumn)
            {
                case "id":
                    if (sortDirection == "asc")
                        return list.OrderBy(x => x.Id).ToList();
                    else
                        return list.OrderByDescending(x => x.Id).ToList();
                case "number":
                    if (sortDirection == "asc")
                        return list.OrderBy(x => x.Number).ToList();
                    else
                        return list.OrderByDescending(x => x.Number).ToList();
                default:
                    return list.OrderBy(x => x.Number).ToList();
            }
        }
    }
}
