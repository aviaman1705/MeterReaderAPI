using MeterReaderAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeterReaderAPI.Services
{
    public class TrackRepository : ITrackRepository
    {
        private readonly ApplicationDbContext _context;

        public TrackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public Task<Track> Get(int id)
        {
            return _context.Tracks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public IQueryable<Track> GetAll()
        {
            return _context.Tracks.AsQueryable();
        }

        public async Task<bool> Update(Track entity)
        {
            var track = await _context.Tracks.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (track != null)
            {
                track.Called = entity.Called;
                track.UnCalled = entity.UnCalled;
                track.Desc = entity.Desc;
                track.Date = entity.Date;
                track.CreatedDate = DateTime.Now;

                return  true;
            }

            return  false;
        }

    }
}
