using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Skin;
using System.Threading.Tasks;

namespace SkinCare_Service.IService
{
    public interface ISkinTypeService
    {
        Task<List<SkinType>> GetAllSkinTypesAsync();
        Task<SkinType> GetSkinTypeByIdAsync(string skinTypeId);
        Task<SkinType> CreateSkinTypeAsync(CreateSkinTypeDto createSkinTypeDto);
        Task<SkinType> UpdateSkinTypeAsync(string skinTypeId, UpdateSkinTypeDto updateSkinTypeDto);
        Task<bool> DeleteSkinTypeAsync(string skinTypeId);
    }
}