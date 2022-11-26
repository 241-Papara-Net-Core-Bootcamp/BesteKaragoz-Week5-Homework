
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeUser.Core.Interfaces
{
    public interface IRepository<T> where T : class

    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
