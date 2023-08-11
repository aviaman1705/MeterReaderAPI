using MeterReaderAPI.DTO;
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

        public void Add(Track item)
        {
            _context.Tracks.Add(item);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            bool isDeleted = false;
            Track entity = _context.Tracks.FirstOrDefault(p => p.Id == id);
            if (entity != null)
            {
                _context.Tracks.Remove(entity);
                _context.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public Track Get(int id)
        {
            return _context.Tracks.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Track> GetAll()
        {
            return _context.Tracks;
        }

        public Dashboard GetDashboardData()
        {
            Dashboard dashboard = new Dashboard();            
            dashboard.DashboardSummary.Called = _context.Tracks.Sum(x => x.Called);
            dashboard.DashboardSummary.UnCalled = _context.Tracks.Sum(x => x.UnCalled);
            dashboard.DashboardSummary.MonthlyCalled = _context.Tracks.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month).Sum(x => x.Called);
            dashboard.DashboardSummary.MonthlyUnCalled = _context.Tracks.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month).Sum(x => x.UnCalled);

            dashboard.MonthlyData = _context.Tracks
           .GroupBy(x => new { x.Date.Year, x.Date.Month })
           .Select(g => new MonthlyData()
           {
               Date = $"{g.Key.Month}/{g.Key.Year}",
               Called = g.Sum(x => x.Called),
               UnCalled = g.Sum(x => x.UnCalled)
           }).ToList();

            return dashboard;
        }

        public bool Update(Track entity)
        {
            var track = _context.Tracks.FirstOrDefault(x => x.Id == entity.Id);

            if (track != null)
            {
                track.Called = entity.Called;
                track.UnCalled = entity.UnCalled;
                track.Desc = entity.Desc;
                track.Date = entity.Date;
                track.CreatedDate = DateTime.Now;

                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
