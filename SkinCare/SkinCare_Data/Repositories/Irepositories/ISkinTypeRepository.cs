using SkinCare_Data.Data;
using System.Threading.Tasks;

namespace SkinCare_Data.IRepositories
{
    public interface ISkinTypeRepository
    {
        Task<List<SkinType>> GetAllAsync();
        Task<SkinType> GetByIdAsync(string skinTypeId);
        Task<int> GetCountAsync();
        Task<int> GetMaxSkinTypeIdNumberAsync(); 
        Task<bool> ExistsAsync(string skinTypeId);
        Task<bool> RoutineExistsAsync(string routineId);
        Task AddAsync(SkinType skinType);
        Task UpdateAsync(SkinType skinType);
        Task DeleteAsync(SkinType skinType);
        Task SaveChangesAsync();
    }
}