using MeterReaderAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterReaderAPI.Services
{
    public class InMemoryRepository : INotebookRepository
    {
        private readonly ApplicationDbContext _context;

        public InMemoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Notebook> Get(int id)
        {
            return _context.Notebooks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<bool> Update(Notebook entity)
        {
            throw new NotImplementedException();
        }

        Task<List<Notebook>> IRepository<Notebook>.GetAll()
        {
            return _context.Notebooks.ToListAsync(); 
        }
    }
}
