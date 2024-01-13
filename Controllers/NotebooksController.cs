﻿using AutoMapper;
using MeterReaderAPI.DTO.Notebook;
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
        private readonly IMapper mapper;
        public NotebooksController(ApplicationDbContext context, INotebookRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<SysDataTablePager<NotebookDTO>>> Get(int page, int itemPerPage, string sortColumn, string sortType, string? search)
        {
            List<NotebookDTO> Notebooks = new List<NotebookDTO>();
            int totalItems = 0;

            //1 טעינת האייטמים ממסד הנתונים
            var queryable = repository.GetAll();

            //2 מעדכן סה"כ אייטמים בהאדר הבקשה
            await HttpContext.InsertParametersPagintionInHelper(queryable);

            //המרת השאילתה למערך 3  
            Notebooks = mapper.Map<List<NotebookDTO>>(await queryable.ToListAsync());

            //4 בדיקה אם הוזן טקסט בשה חיפוש
            if (!string.IsNullOrEmpty(search))
            {
                Notebooks = Notebooks.Where(m => m.Number.ToString().Contains(search)).ToList();

                totalItems = Notebooks.Count;
                Notebooks = Sort(sortColumn, sortType, Notebooks).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
            }
            else
            {
                totalItems = Notebooks.Count;
                Notebooks = Sort(sortColumn, sortType, Notebooks).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
            }

            var NotebooksPaged = new SysDataTablePager<NotebookDTO>(Notebooks, totalItems, itemPerPage, page);
            return NotebooksPaged;
        }

        [HttpGet("{id:int}")]
        public ActionResult<NotebookDTO> Get(int id)
        {
            var Notebook = repository.Get(id);

            if (Notebook == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<NotebookDTO>(Notebook);
            return dto;
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromForm] NotebookDTO NotebookDTO)
        {
            var Notebook = repository.Get(id);

            if (Notebook == null)
            {
                return NotFound();

            }

            Notebook = mapper.Map<Notebook>(NotebookDTO);

            repository.Update(Notebook);
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
        public async Task<ActionResult<int>> Post([FromForm] AddNotebookDTO notebookDTO)
        {
            try
            {
                var Notebook = mapper.Map<Notebook>(notebookDTO);
                repository.Add(Notebook);
                return Notebook.Id;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private List<NotebookDTO> Sort(string sortColumn, string sortType, List<NotebookDTO> list)
        {
            switch (sortColumn)
            {
                case "number":
                    if (sortType == "desc")
                        return list.OrderByDescending(x => x.Number).ToList();
                    else
                        return list.OrderBy(x => x.Number).ToList();       
                default:
                    return list.OrderByDescending(x => x.Id).ToList();
            }
        }
    }
}