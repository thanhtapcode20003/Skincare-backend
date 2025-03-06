using Microsoft.Extensions.Logging;
using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Skin;
using SkinCare_Data.IRepositories;
using SkinCare_Service.IService;
using System.Threading.Tasks;

namespace SkinCare_Service
{
    public class SkinTypeService : ISkinTypeService
    {
        private readonly ISkinTypeRepository _repository;
        private readonly ILogger<SkinTypeService> _logger;

        public SkinTypeService(ISkinTypeRepository repository, ILogger<SkinTypeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<SkinType>> GetAllSkinTypesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all SkinTypes");

                var skinTypes = await _repository.GetAllAsync();
                return skinTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching SkinTypes: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<SkinType> GetSkinTypeByIdAsync(string skinTypeId)
        {
            try
            {
                _logger.LogInformation("Fetching SkinType with ID: {SkinTypeId}", skinTypeId);

                var skinType = await _repository.GetByIdAsync(skinTypeId);
                if (skinType == null)
                {
                    _logger.LogWarning("SkinType not found with ID: {SkinTypeId}", skinTypeId);
                    return null;
                }

                return skinType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching SkinType with ID {SkinTypeId}: {ErrorMessage}", skinTypeId, ex.Message);
                throw;
            }
        }

        public async Task<SkinType> CreateSkinTypeAsync(CreateSkinTypeDto createSkinTypeDto)
        {
            try
            {
                _logger.LogInformation("Creating new SkinType");

                if (string.IsNullOrEmpty(createSkinTypeDto.SkinTypeName))
                {
                    throw new Exception("SkinTypeName is required!");
                }

                if (!string.IsNullOrEmpty(createSkinTypeDto.RoutineId))
                {
                    if (!await _repository.RoutineExistsAsync(createSkinTypeDto.RoutineId))
                    {
                        throw new Exception("RoutineId does not exist!");
                    }
                }

                string skinTypeId = null;
                int maxRetries = 5; 
                for (int attempt = 0; attempt < maxRetries; attempt++)
                {
                    
                    int maxSkinTypeNumber = await _repository.GetMaxSkinTypeIdNumberAsync();
                    int newSkinTypeNumber = maxSkinTypeNumber + 1;
                    skinTypeId = $"SK{newSkinTypeNumber:D3}"; 

                 
                    if (!await _repository.ExistsAsync(skinTypeId))
                    {
                        break; 
                    }

               
                    await Task.Delay(100); 
                    if (attempt == maxRetries - 1)
                    {
                        throw new Exception("SkinTypeID generation conflict after multiple attempts! Please try again later.");
                    }
                }

                // Tạo đối tượng SkinType từ DTO
                var skinType = new SkinType
                {
                    SkinTypeId = skinTypeId,
                    SkinTypeName = createSkinTypeDto.SkinTypeName,
                    Description = createSkinTypeDto.Description,
                    RoutineId = createSkinTypeDto.RoutineId
                };

                await _repository.AddAsync(skinType);
                await _repository.SaveChangesAsync();

                return skinType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating SkinType: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<SkinType> UpdateSkinTypeAsync(string skinTypeId, UpdateSkinTypeDto updateSkinTypeDto)
        {
            try
            {
                _logger.LogInformation("Updating SkinType with ID: {SkinTypeId}", skinTypeId);

                var existingSkinType = await _repository.GetByIdAsync(skinTypeId);
                if (existingSkinType == null)
                {
                    throw new Exception("SkinType not found!");
                }

                existingSkinType.SkinTypeName = updateSkinTypeDto.SkinTypeName ?? existingSkinType.SkinTypeName;
                existingSkinType.Description = updateSkinTypeDto.Description ?? existingSkinType.Description;

                if (!string.IsNullOrEmpty(updateSkinTypeDto.RoutineId))
                {
                    if (!await _repository.RoutineExistsAsync(updateSkinTypeDto.RoutineId))
                    {
                        throw new Exception("RoutineId does not exist!");
                    }
                    existingSkinType.RoutineId = updateSkinTypeDto.RoutineId;
                }
                else
                {
                    existingSkinType.RoutineId = null; 
                }

                await _repository.UpdateAsync(existingSkinType);
                await _repository.SaveChangesAsync();

                return existingSkinType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating SkinType with ID {SkinTypeId}: {ErrorMessage}", skinTypeId, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteSkinTypeAsync(string skinTypeId)
        {
            try
            {
                _logger.LogInformation("Deleting SkinType with ID: {SkinTypeId}", skinTypeId);

                var skinType = await _repository.GetByIdAsync(skinTypeId);
                if (skinType == null)
                {
                    _logger.LogWarning("SkinType not found with ID: {SkinTypeId}", skinTypeId);
                    return false;
                }

                await _repository.DeleteAsync(skinType);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting SkinType with ID {SkinTypeId}: {ErrorMessage}", skinTypeId, ex.Message);
                throw;
            }
        }
    }
}