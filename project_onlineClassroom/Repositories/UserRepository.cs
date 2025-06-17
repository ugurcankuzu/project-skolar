using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;


        // IUserRepository operations
        /// <summary>
        /// Returns all users in the database.
        /// </summary>
        /// <returns><see cref="Task{List{User}}"/></returns>
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        /// <summary>
        /// Returns a user by their ID. If the user does not exist, returns null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Task{User?}"/></returns>
        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }
        /// <summary>
        /// Adds a new user to the database and saves changes asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns><see cref="Task{User}"/></returns>
        public async Task<User> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Finds a user asynchronously based on the provided predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns><see cref="Task{User?}"/></returns>
        public async Task<User?> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.FirstOrDefaultAsync(predicate);
        }
        /// <summary>
        /// Retrieves a user by their email address. If the user does not exist, returns null.
        /// </summary>
        /// <param name="email"></param>
        /// <returns><see cref="Task{User?}"/></returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        /// <summary>
        /// Updates an existing user in the database and saves changes asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        public async Task Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Deletes a user from the database and saves changes asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        public async Task Delete(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByProviderKey(string providerKey, string authProvider = "Google")
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ProviderKey == providerKey && u.AuthProvider == authProvider);
        }
    }
}
