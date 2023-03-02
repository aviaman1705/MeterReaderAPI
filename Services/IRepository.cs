using MeterReaderAPI.Entities;
using System.Runtime.CompilerServices;

namespace MeterReaderAPI.Services
{
    public interface IRepository<T>
    {
        void Create(T entity);
        IQueryable<T> GetAll();

        Task<T> Get(int id);

        Task<bool> Update(T entity);
    }
}
