using MeterReaderAPI.Entities;

namespace MeterReaderAPI.Services
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();

        Task<T> Get(int id);

        Task<bool> Update(T entity);
    }
}
