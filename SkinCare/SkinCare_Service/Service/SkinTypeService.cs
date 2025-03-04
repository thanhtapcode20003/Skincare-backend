using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCare_Data;
using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Skin;
using System.Threading.Tasks;

public class SkinTypeService
{
    private readonly SkinCare_DBContext _context;
    private readonly ILogger<SkinTypeService> _logger;

    public SkinTypeService(SkinCare_DBContext context, ILogger<SkinTypeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Get all SkinTypes
    public async Task<List<SkinType>> GetAllSkinTypesAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all SkinTypes");

            var skinTypes = await _context.SkinTypes
                .Include(st => st.SkinCareRoutine)
                .ToListAsync();
            return skinTypes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching SkinTypes: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    // Get SkinType by ID
    public async Task<SkinType> GetSkinTypeByIdAsync(string skinTypeId)
    {
        try
        {
            _logger.LogInformation("Fetching SkinType with ID: {SkinTypeId}", skinTypeId);

            var skinType = await _context.SkinTypes
                .Include(st => st.SkinCareRoutine)
                .FirstOrDefaultAsync(st => st.SkinTypeId == skinTypeId);

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

    // Create SkinType (using CreateSkinTypeDto, tự động tạo SkinTypeId)
    public async Task<SkinType> CreateSkinTypeAsync(CreateSkinTypeDto createSkinTypeDto)
    {
        try
        {
            _logger.LogInformation("Creating new SkinType");

            if (string.IsNullOrEmpty(createSkinTypeDto.SkinTypeName))
            {
                throw new Exception("SkinTypeName is required!");
            }

            // Tự động tạo SkinTypeId (SK001, SK002, ...)
            int skinTypeCount = await _context.SkinTypes.CountAsync() + 1;
            string skinTypeId = $"SK{skinTypeCount:D3}"; // Ví dụ: SK001, SK002, ...

            // Kiểm tra SkinTypeId có trùng không (dù ít xảy ra vì tự động tạo, nhưng để an toàn)
            if (await _context.SkinTypes.AnyAsync(st => st.SkinTypeId == skinTypeId))
            {
                throw new Exception("SkinTypeId generation conflict! Please try again.");
            }

            // Kiểm tra RoutineId nếu có (nếu không null, phải tồn tại trong SkinCareRoutines)
            if (!string.IsNullOrEmpty(createSkinTypeDto.RoutineId))
            {
                var routine = await _context.SkinCareRoutines.FindAsync(createSkinTypeDto.RoutineId);
                if (routine == null)
                {
                    throw new Exception("RoutineId does not exist!");
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

            _context.SkinTypes.Add(skinType);
            await _context.SaveChangesAsync();

            return skinType;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SkinType: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    // Update SkinType (sử dụng UpdateSkinTypeDto, không cập nhật SkinTypeId)
    public async Task<SkinType> UpdateSkinTypeAsync(string skinTypeId, UpdateSkinTypeDto updateSkinTypeDto)
    {
        try
        {
            _logger.LogInformation("Updating SkinType with ID: {SkinTypeId}", skinTypeId);

            var existingSkinType = await _context.SkinTypes.FindAsync(skinTypeId);
            if (existingSkinType == null)
            {
                throw new Exception("SkinType not found!");
            }

            // Cập nhật các trường từ DTO, không cập nhật SkinTypeId
            existingSkinType.SkinTypeName = updateSkinTypeDto.SkinTypeName ?? existingSkinType.SkinTypeName;
            existingSkinType.Description = updateSkinTypeDto.Description ?? existingSkinType.Description;

            // Kiểm tra RoutineId nếu có (nếu không null, phải tồn tại trong SkinCareRoutines)
            if (!string.IsNullOrEmpty(updateSkinTypeDto.RoutineId))
            {
                var routine = await _context.SkinCareRoutines.FindAsync(updateSkinTypeDto.RoutineId);
                if (routine == null)
                {
                    throw new Exception("RoutineId does not exist!");
                }
                existingSkinType.RoutineId = updateSkinTypeDto.RoutineId;
            }
            else
            {
                existingSkinType.RoutineId = null; // Nếu không gửi RoutineId, đặt về null
            }

            await _context.SaveChangesAsync();

            return existingSkinType;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SkinType with ID {SkinTypeId}: {ErrorMessage}", skinTypeId, ex.Message);
            throw;
        }
    }

    // Delete SkinType
    public async Task<bool> DeleteSkinTypeAsync(string skinTypeId)
    {
        try
        {
            _logger.LogInformation("Deleting SkinType with ID: {SkinTypeId}", skinTypeId);

            var skinType = await _context.SkinTypes.FindAsync(skinTypeId);
            if (skinType == null)
            {
                _logger.LogWarning("SkinType not found with ID: {SkinTypeId}", skinTypeId);
                return false;
            }

            _context.SkinTypes.Remove(skinType);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting SkinType with ID {SkinTypeId}: {ErrorMessage}", skinTypeId, ex.Message);
            throw;
        }
    }
}