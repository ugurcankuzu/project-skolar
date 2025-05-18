using System.Linq.Expressions;

namespace project_onlineClassroom.Interfaces
{
    public interface IGenericInterface<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}
