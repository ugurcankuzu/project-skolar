using System.Linq.Expressions;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User entity);
        Task<User?> FindAsync(Expression<Func<User, bool>> predicate);
        Task<User?> GetByEmailAsync(string email);
        Task Update(User entity);
        Task Delete(User entity);
    }
}
