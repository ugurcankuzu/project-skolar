using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Util
{

    public class JoinCodeGenerator : IJoinCodeGenerator
    {
        private readonly AppDbContext _context;
        public JoinCodeGenerator(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> GenerateJoinCodeAsync()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string joinCode;
            do
            {
                joinCode = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (await _context.Classes.AnyAsync(c => c.JoinCode == joinCode));
            return joinCode;
        }
    }
}
