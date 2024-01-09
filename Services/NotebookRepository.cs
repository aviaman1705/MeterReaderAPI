using MeterReaderAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterReaderAPI.Services
{
    public class NotebookRepository : INotebookRepository
    {
        private readonly ApplicationDbContext _context;

        public NotebookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Notebook item)
        {
            _context.Notebooks.Add(item);
            _context.SaveChanges();
        }
        public Notebook Get(int id)
        {
            return _context.Notebooks.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Notebook> GetAll()
        {
            return _context.Notebooks.Include(x => x.Tracks);
        }

        public bool Update(Notebook entity)
        {
            Notebook notebook = _context.Notebooks.FirstOrDefault(x => x.Id == entity.Id);
            if (notebook != null)
            {
                notebook.Number = entity.Number;
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            bool isDeleted = false;
            Notebook notebook = _context.Notebooks.FirstOrDefault(p => p.Id == id);
            if (notebook != null)
            {
                _context.Notebooks.Remove(notebook);
                _context.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }
    }
}
