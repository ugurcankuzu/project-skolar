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
        public UserRepository(AppDbContext ctx) => _context = ctx;


        // IUserRepository operations
        public async Task<User> AddAsync(User entity)
        {
            // Add the user to the database
            // Save Changes
            // Return the user
            // Handle exceptions
            try
            {
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                throw new Exception($"Error adding user to the database:\n {ex.Message}");
            }
        }

        public async Task Delete(User entity)
        {
            // Delete the user from the database
            try
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                throw new Exception($"Error deleting user from the database:\n {ex.Message}");
            }
        }

        public async Task<User> FindAsync(Expression<Func<User, bool>> predicate)
        {
            // Find the user based on the predicate

            User user = await _context.Users.FirstOrDefaultAsync(predicate) ?? throw new UserNotFoundException();
            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            // Find the user by email

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException("User not found with this email.");
            return user;

        }
        public async Task<List<User>> GetAllAsync()
        // Get all users from the database
        {

            List<User> users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            // Get a user by ID

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new UserNotFoundException("User not found with this ID.");
            return user;


        }

        public async Task Update(User entity)
        {
            // Update the user in the database
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

        }
    }
}
