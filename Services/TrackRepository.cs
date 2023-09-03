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
            var query = _context.Tracks;

            dashboard.DashboardSummary.Called = query.Sum(x => x.Called);
            dashboard.DashboardSummary.UnCalled = query.Sum(x => x.UnCalled);
            dashboard.DashboardSummary.MonthlyCalled = query.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month).Sum(x => x.Called);
            dashboard.DashboardSummary.MonthlyUnCalled = query.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month).Sum(x => x.UnCalled);

            if (dashboard.DashboardSummary.MonthlyUnCalled > 0)
                dashboard.DashboardSummary.MonthlyUncalledPercentage = Math.Round((double)(100 * dashboard.DashboardSummary.MonthlyUnCalled) / dashboard.DashboardSummary.MonthlyCalled, 2);
            dashboard.DashboardSummary.TotalUncalledPercentage = Math.Round((double)(100 * dashboard.DashboardSummary.UnCalled) / dashboard.DashboardSummary.Called, 2);

            dashboard.MonthlyData = query
                .OrderBy(x => x.Date)
                .GroupBy(x => new { x.Date.Month, x.Date.Year })
           .Select(g => new MonthlyData()
           {
               Date = $"{g.Key.Month}/{g.Key.Year}",
               Called = g.Sum(x => x.Called),
               UnCalled = g.Sum(x => x.UnCalled)
           })
           .ToList();

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

        #region ofType
        //ofType מאפשר לך לסנן לפי סוג מסויים (int,string object...)
        //IList mixedList = new ArrayList();
        //mixedList.Add(0);
        //mixedList.Add("One");
        //mixedList.Add("Two");
        //mixedList.Add(3);
        //mixedList.Add(new Student() { StudentID = 1, StudentName = "Bill" });

        //var stringResult = from s in mixedList.OfType<string>()
        //                   select s;

        //var intResult = from s in mixedList.OfType<int>()
        //                select s;
        #endregion

        #region thenBy
        //thenBy מאפשר לך לעשות תת סידור אחר הסידור הראשי
        //var tracks = repository.GetAll();

        //if (tracks == null)
        //{
        //    return NotFound();
        //}

        //var thenByResult = tracks.OrderBy(s => s.UnCalled).ThenBy(s => s.Date).Take(5);

        //var thenByDescResult = tracks.OrderBy(s => s.UnCalled).ThenByDescending(s => s.Date).Take(5);

        //return new { thenByResult, thenByDescResult };
        #endregion

        //groupBy
        #region
        //groupBy מחזיר קבוצה של אלמנטים מהאוסף הנתון, בהתבסס סוג של מפתח
        //var tracks = repository.GetAll();


        //List<int> keys = new List<int>();
        //List<Test> list = new List<Test>();

        //var groupedResult1 = from s in tracks
        //                    group s by s.UnCalled;

        //foreach (var trackGroup in groupedResult1)
        //{
        //    Test obj = new Test();
        //    obj.Key = trackGroup.Key;
        //    obj.Count = trackGroup.Count();

        //    list.Add(obj);
        //}

        //var groupedResult2 = tracks.GroupBy(s => s.UnCalled);

        //foreach (var trackGroup in groupedResult2)
        //{
        //    Test obj = new Test();
        //    obj.Key = trackGroup.Key;
        //    obj.Count = trackGroup.Count();

        //    list.Add(obj);
        //}

        //return new { list };

        #endregion

        //join
        #region
        //join מבצע פעולה על שני מערכים, פנימי וחיצוני,ומחזיר מערך משני המערכים
        //IList<string> strList1 = new List<string>() { "One", "Two", "Three", "Four", "Six" };

        //IList<string> strList2 = new List<string>() { "One", "Two", "Five", "Six" };

        //var innerJoin = strList1.Join(strList2,
        //                      str1 => str1,
        //                      str2 => str2,
        //                      (str1, str2) => str1);

        //return innerJoin.ToList();

        //var tracksList1 = repository.GetAll().OrderBy(x => x.Called).Take(40);

        //var tracksList2 = repository.GetAll().OrderBy(x => x.Desc).Take(40);

        //var innerJoinResult = tracksList1.Join(tracksList2,
        //    str1 => str1,
        //    str2 => str2,
        //    (str1, str2) => str1);

        //var innerJoin = tracksList1.Join(
        //    tracksList2,
        //    track => track.Desc,
        //    track2 => track2.Desc,
        //    (track, track2) => new
        //    {
        //        Desc = track.Desc,
        //        Date = track2.Date
        //    }).OrderBy(x=>x.Desc).ToList();

        //return innerJoin;

        #endregion

        //groupJoin
        #region
        //join מבצע פעולה על שני מערכים, פנימי וחיצוני,ומחזיר מערך משני המערכים
        //var strList1 = repository.GetAll().OrderBy(x => x.Desc).Take(50);

        //var strList2 = repository.GetAll().OrderBy(x => x.Date).Take(50);

        //var innerJoin = strList1.GroupJoin(strList2,
        //                      str1 => str1.UnCalled,
        //                      str2 => str2.UnCalled,
        //                      (str1, str2) => new
        //                      {
        //                          A = str2,
        //                          B = $"{str1.UnCalled} אי קריאות"
        //                      }).ToList();

        //return innerJoin;

        #endregion

        //all
        //var strList1 = repository.GetAll().All(s => s.Called > 0);

        //aggregate
        #region
        //IList<String> strList = new List<String>() { "One", "Two", "Three", "Four", "Five" };

        //var commaSeperatedString = repository.GetAll().ToList().Aggregate((s1, s2) => s1 + ", " + s2);
        //return commaSeperatedString;

        //string commaSeperatedDescNames = repository.GetAll().ToList().Aggregate<Track, string>(
        //                            "Student Names: ",  // seed value
        //                            (str, s) => str += s.Desc + ",");

        //return commaSeperatedDescNames;

        //string commaSeparatedStudentNames = repository.GetAll().ToList().Aggregate<Track, string, string>(
        //                                String.Empty, // seed value
        //                                (str, s) => str += s.Desc + ",", // returns result using seed value, String.Empty goes to lambda expression as str
        //                                str => str.Substring(0, str.Length - 1)); // result selector that removes last comma

        //return commaSeparatedStudentNames;
        #endregion
    }
}





