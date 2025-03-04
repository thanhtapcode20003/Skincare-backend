using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCare_Data;
using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Routine;
using System.Threading.Tasks;

public class SkinCareRoutineService
{
    private readonly SkinCare_DBContext _context;
    private readonly ILogger<SkinCareRoutineService> _logger;

    public SkinCareRoutineService(SkinCare_DBContext context, ILogger<SkinCareRoutineService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<SkinCareRoutine>> GetAllSkinCareRoutinesAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all SkinCareRoutines");

            var routines = await _context.SkinCareRoutines.ToListAsync();
            return routines;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching SkinCareRoutines: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<SkinCareRoutine> GetSkinCareRoutineByIdAsync(string routineId)
    {
        try
        {
            _logger.LogInformation("Fetching SkinCareRoutine with ID: {RoutineId}", routineId);

            var routine = await _context.SkinCareRoutines
                .FirstOrDefaultAsync(r => r.RoutineId == routineId);

            if (routine == null)
            {
                _logger.LogWarning("SkinCareRoutine not found with ID: {RoutineId}", routineId);
                return null;
            }

            return routine;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching SkinCareRoutine with ID {RoutineId}: {ErrorMessage}", routineId, ex.Message);
            throw;
        }
    }

    public async Task<SkinCareRoutine> CreateSkinCareRoutineAsync(CreateSkinCareRoutineDto createRoutineDto)
    {
        try
        {
            _logger.LogInformation("Creating new SkinCareRoutine");

            if (string.IsNullOrEmpty(createRoutineDto.Description))
            {
                throw new Exception("Description is required!");
            }
            if (string.IsNullOrEmpty(createRoutineDto.Type))
            {
                throw new Exception("Type is required!");
            }

            int routineCount = await _context.SkinCareRoutines.CountAsync() + 1;
            string routineId = $"R{routineCount:D3}"; 

            if (await _context.SkinCareRoutines.AnyAsync(r => r.RoutineId == routineId))
            {
                throw new Exception("RoutineId generation conflict! Please try again.");
            }

            // Tạo đối tượng SkinCareRoutine từ DTO
            var routine = new SkinCareRoutine
            {
                RoutineId = routineId,
                Description = createRoutineDto.Description,
                Type = createRoutineDto.Type
            };

            _context.SkinCareRoutines.Add(routine);
            await _context.SaveChangesAsync();

            return routine;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SkinCareRoutine: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<SkinCareRoutine> UpdateSkinCareRoutineAsync(string routineId, UpdateSkinCareRoutineDto updateRoutineDto)
    {
        try
        {
            _logger.LogInformation("Updating SkinCareRoutine with ID: {RoutineId}", routineId);

            var existingRoutine = await _context.SkinCareRoutines.FindAsync(routineId);
            if (existingRoutine == null)
            {
                throw new Exception("SkinCareRoutine not found!");
            }

            existingRoutine.Description = updateRoutineDto.Description ?? existingRoutine.Description;
            existingRoutine.Type = updateRoutineDto.Type ?? existingRoutine.Type;

            await _context.SaveChangesAsync();

            return existingRoutine;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SkinCareRoutine with ID {RoutineId}: {ErrorMessage}", routineId, ex.Message);
            throw;
        }
    }

    public async Task<bool> DeleteSkinCareRoutineAsync(string routineId)
    {
        try
        {
            _logger.LogInformation("Deleting SkinCareRoutine with ID: {RoutineId}", routineId);

            var routine = await _context.SkinCareRoutines.FindAsync(routineId);
            if (routine == null)
            {
                _logger.LogWarning("SkinCareRoutine not found with ID: {RoutineId}", routineId);
                return false;
            }

            _context.SkinCareRoutines.Remove(routine);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting SkinCareRoutine with ID {RoutineId}: {ErrorMessage}", routineId, ex.Message);
            throw;
        }
    }
}