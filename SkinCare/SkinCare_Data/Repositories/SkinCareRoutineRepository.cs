using Microsoft.EntityFrameworkCore;
using SkinCare_Data.Data;
using SkinCare_Data.IRepositories;
using System.Threading.Tasks;

namespace SkinCare_Data.Repositories
{
    public class SkinCareRoutineRepository : ISkinCareRoutineRepository
    {
        private readonly SkinCare_DBContext _context;

        public SkinCareRoutineRepository(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<List<SkinCareRoutine>> GetAllAsync()
        {
            return await _context.SkinCareRoutines.ToListAsync();
        }

        public async Task<SkinCareRoutine> GetByIdAsync(string routineId)
        {
            return await _context.SkinCareRoutines.FirstOrDefaultAsync(r => r.RoutineId == routineId);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.SkinCareRoutines.CountAsync();
        }

        public async Task<bool> ExistsAsync(string routineId)
        {
            return await _context.SkinCareRoutines.AnyAsync(r => r.RoutineId == routineId);
        }

        public async Task AddAsync(SkinCareRoutine routine)
        {
            await _context.SkinCareRoutines.AddAsync(routine);
        }

        public Task UpdateAsync(SkinCareRoutine routine)
        {
            _context.SkinCareRoutines.Update(routine);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(SkinCareRoutine routine)
        {
            _context.SkinCareRoutines.Remove(routine);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}