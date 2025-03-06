using SkinCare_Data.Data;
using System.Threading.Tasks;

namespace SkinCare_Data.IRepositories
{
    public interface ISkinCareRoutineRepository
    {
        Task<List<SkinCareRoutine>> GetAllAsync();
        Task<SkinCareRoutine> GetByIdAsync(string routineId);
        Task<int> GetCountAsync();
        Task<bool> ExistsAsync(string routineId);
        Task AddAsync(SkinCareRoutine routine);
        Task UpdateAsync(SkinCareRoutine routine);
        Task DeleteAsync(SkinCareRoutine routine);
        Task SaveChangesAsync();
    }
}