using Microsoft.EntityFrameworkCore;
using SkinCare_Data.Data;
using SkinCare_Data.IRepositories;
using System.Threading.Tasks;
using System.Linq;

namespace SkinCare_Data.Repositories
{
    public class SkinTypeRepository : ISkinTypeRepository
    {
        private readonly SkinCare_DBContext _context;

        public SkinTypeRepository(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<List<SkinType>> GetAllAsync()
        {
            return await _context.SkinTypes
                .Include(st => st.SkinCareRoutine)
                .ToListAsync();
        }

        public async Task<SkinType> GetByIdAsync(string skinTypeId)
        {
            return await _context.SkinTypes
                .Include(st => st.SkinCareRoutine)
                .FirstOrDefaultAsync(st => st.SkinTypeId == skinTypeId);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.SkinTypes.CountAsync();
        }

       
        public async Task<int> GetMaxSkinTypeIdNumberAsync()
        {
            var maxId = await _context.SkinTypes
                .Select(st => st.SkinTypeId)
                .OrderByDescending(id => id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxId))
            {
                return 0; 
            }

         
            if (int.TryParse(maxId.Replace("SK", ""), out int maxNumber))
            {
                return maxNumber;
            }

            return 0; 
        }

        public async Task<bool> ExistsAsync(string skinTypeId)
        {
            return await _context.SkinTypes.AnyAsync(st => st.SkinTypeId == skinTypeId);
        }

        public async Task<bool> RoutineExistsAsync(string routineId)
        {
            return await _context.SkinCareRoutines.AnyAsync(r => r.RoutineId == routineId);
        }

        public async Task AddAsync(SkinType skinType)
        {
            await _context.SkinTypes.AddAsync(skinType);
        }

        public Task UpdateAsync(SkinType skinType)
        {
            _context.SkinTypes.Update(skinType);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(SkinType skinType)
        {
            _context.SkinTypes.Remove(skinType);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}