using System.Linq.Expressions;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IUserRepository
    {
        // IUserRepository operations
        /// <summary>
        /// Returns all users in the database.
        /// </summary>
        /// <returns><see cref="Task{List{User}}"/></returns>
        Task<List<User>> GetAllAsync();
        /// <summary>
        /// Returns a user by their ID. If the user does not exist, returns null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Task{User?}"/></returns>
        Task<User?> GetByIdAsync(int id);
        /// <summary>
        /// Adds a new user to the database and saves changes asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns><see cref="Task{User}"/></returns>
        Task<User> AddAsync(User entity);
        /// <summary>
        /// Finds a user asynchronously based on the provided predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns><see cref="Task{User?}"/></returns>
        Task<User?> FindAsync(Expression<Func<User, bool>> predicate);
        /// <summary>
        /// Retrieves a user by their email address. If the user does not exist, returns null.
        /// </summary>
        /// <param name="email"></param>
        /// <returns><see cref="Task{User?}"/></returns>
        Task<User?> GetByEmailAsync(string email);
        /// <summary>
        /// Updates an existing user in the database and saves changes asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        Task Update(User entity);
        /// <summary>
        /// Deletes a user from the database and saves changes asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        Task Delete(User entity);
        Task<User?> GetByProviderKey(string providerKey, string authProvider = "Google");
    }
}
