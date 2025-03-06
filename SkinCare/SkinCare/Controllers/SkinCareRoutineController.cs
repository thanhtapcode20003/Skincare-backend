using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Routine;
using SkinCare_Service.IService; 
using System.Threading.Tasks;

[Route("api/skin-care-routines")]
[ApiController]
public class SkinCareRoutineController : ControllerBase
{
    private readonly ISkinCareRoutineService _routineService; 
    private readonly ILogger<SkinCareRoutineController> _logger;

    public SkinCareRoutineController(ISkinCareRoutineService routineService, ILogger<SkinCareRoutineController> logger)
    {
        _routineService = routineService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<SkinCareRoutine>>> GetSkinCareRoutines()
    {
        try
        {
            _logger.LogInformation("Fetching all SkinCareRoutines");

            var routines = await _routineService.GetAllSkinCareRoutinesAsync();
            return Ok(routines);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching SkinCareRoutines: {ErrorMessage}", ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SkinCareRoutine>> GetSkinCareRoutine(string id)
    {
        try
        {
            _logger.LogInformation("Fetching SkinCareRoutine with ID: {RoutineId}", id);

            var routine = await _routineService.GetSkinCareRoutineByIdAsync(id);
            if (routine == null)
            {
                _logger.LogWarning("SkinCareRoutine not found with ID: {RoutineId}", id);
                return NotFound(new { message = "SkinCareRoutine not found" });
            }

            return Ok(routine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching SkinCareRoutine with ID {RoutineId}: {ErrorMessage}", id, ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    [HttpPost("create")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<SkinCareRoutine>> CreateSkinCareRoutine(CreateSkinCareRoutineDto createRoutineDto)
    {
        try
        {
            _logger.LogInformation("Creating new SkinCareRoutine");

            if (string.IsNullOrEmpty(createRoutineDto.Description))
            {
                return BadRequest(new { message = "Description is required" });
            }
            if (string.IsNullOrEmpty(createRoutineDto.Type))
            {
                return BadRequest(new { message = "Type is required" });
            }

            var createdRoutine = await _routineService.CreateSkinCareRoutineAsync(createRoutineDto);
            return Ok(createdRoutine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SkinCareRoutine: {ErrorMessage}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("edit/{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateSkinCareRoutine(string id, UpdateSkinCareRoutineDto updateRoutineDto)
    {
        try
        {
            _logger.LogInformation("Updating SkinCareRoutine with ID: {RoutineId}", id);

            
            var updatedRoutine = await _routineService.UpdateSkinCareRoutineAsync(id, updateRoutineDto);
            return Ok(updatedRoutine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SkinCareRoutine with ID {RoutineId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteSkinCareRoutine(string id)
    {
        try
        {
            _logger.LogInformation("Deleting SkinCareRoutine with ID: {RoutineId}", id);

            var result = await _routineService.DeleteSkinCareRoutineAsync(id);
            if (!result)
            {
                return NotFound(new { message = "SkinCareRoutine not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting SkinCareRoutine with ID {RoutineId}: {ErrorMessage}", id, ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }
}