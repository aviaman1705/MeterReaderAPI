﻿using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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


        [HttpGet("{term}")]
        public async Task<ActionResult<List<SearchDTO>>> Get(string term)
        {           
            var queryable = repository.GetAll();
            var searchResults = mapper.Map<List<SearchDTO>>(repository.GetAll().Where(x => x.Desc.Contains(term))).ToList();

            if (searchResults == null)
            {
                return NotFound();
            }

            return searchResults;
        }
    }
}
