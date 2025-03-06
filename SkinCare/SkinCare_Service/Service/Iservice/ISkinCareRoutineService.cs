using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Routine;
using System.Threading.Tasks;

namespace SkinCare_Service.IService
{
    public interface ISkinCareRoutineService
    {
        Task<List<SkinCareRoutine>> GetAllSkinCareRoutinesAsync();
        Task<SkinCareRoutine> GetSkinCareRoutineByIdAsync(string routineId);
        Task<SkinCareRoutine> CreateSkinCareRoutineAsync(CreateSkinCareRoutineDto createRoutineDto);
        Task<SkinCareRoutine> UpdateSkinCareRoutineAsync(string routineId, UpdateSkinCareRoutineDto updateRoutineDto);
        Task<bool> DeleteSkinCareRoutineAsync(string routineId);
    }
}