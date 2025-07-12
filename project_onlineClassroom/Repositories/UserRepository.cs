using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_onlineClassroom.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A <see cref="Task{List{User}}"/> containing all users.</returns>
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Retrieves a user by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the user to retrieve.</param>
        /// <returns>The found <see cref="User"/> entity.</returns>
        /// <exception cref="UserNotFoundException">Thrown if no user with the specified ID is found.</exception>
        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new UserNotFoundException($"User with ID {id} not found.");
            return user;
        }

        /// <summary>
        /// Adds a new user to the database and saves the changes.
        /// </summary>
        /// <param name="entity">The <see cref="User"/> entity to be created.</param>
        /// <returns>The created <see cref="User"/> entity with its new database-generated ID.</returns>
        public async Task<User> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Finds a single user asynchronously based on a predicate.
        /// </summary>
        /// <param name="predicate">The expression to filter the users.</param>
        /// <returns>A <see cref="Task{User}"/> containing the first user that matches the criteria, or <c>null</c> if no user is found.</returns>
        public async Task<User?> FindAsync(Expression<Func<User, bool>> predicate)
        {
            User? result = await _context.Users.FirstOrDefaultAsync(predicate);
            return result;
        }

        /// <summary>
        /// Finds a user by their email address.
        /// This method is primarily used in authentication flows where non-existence is a valid state, not an error.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>A <see cref="Task{User}"/> containing the found user, or <c>null</c> if no user with the specified email exists.</returns>
        public async Task<User?> FindByEmailAsync(string email)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        /// <summary>
        /// Updates an existing user in the database and saves the changes.
        /// This method assumes the entity has been retrieved and modified in the service layer.
        /// </summary>
        /// <param name="entity">The <see cref="User"/> entity with updated values.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a user from the database and saves the changes.
        /// This method assumes the entity has been retrieved and verified in the service layer.
        /// </summary>
        /// <param name="entity">The <see cref="User"/> entity to be deleted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Delete(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds a user by their external authentication provider key (e.g., Google's unique subject ID).
        /// </summary>
        /// <param name="providerKey">The unique identifier from the external provider.</param>
        /// <param name="authProvider">The name of the authentication provider (e.g., "Google").</param>
        /// <returns>A <see cref="Task{User}"/> containing the found user, or <c>null</c> if no user is linked to that provider key.</returns>
        public async Task<User?> FindByProviderKey(string providerKey, string authProvider = "Google")
        {
            // The explicit cast to User? is not necessary but can improve clarity.
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.ProviderKey == providerKey && u.AuthProvider == authProvider);
            return user;
        }
    }
}