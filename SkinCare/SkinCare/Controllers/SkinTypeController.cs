using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Skin;
using SkinCare_Service.IService; 
using System.Threading.Tasks;

[Route("api/skin-types")]
[ApiController]
public class SkinTypeController : ControllerBase
{
    private readonly ISkinTypeService _skinTypeService; 
    private readonly ILogger<SkinTypeController> _logger;

    public SkinTypeController(ISkinTypeService skinTypeService, ILogger<SkinTypeController> logger)
    {
        _skinTypeService = skinTypeService;
        _logger = logger;
    }

    
    [HttpGet]
    public async Task<ActionResult<List<SkinType>>> GetSkinTypes()
    {
        try
        {
            _logger.LogInformation("Fetching all SkinTypes");

            var skinTypes = await _skinTypeService.GetAllSkinTypesAsync();
            return Ok(skinTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching SkinTypes: {ErrorMessage}", ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<SkinType>> GetSkinType(string id)
    {
        try
        {
            _logger.LogInformation("Fetching SkinType with ID: {SkinTypeId}", id);

            var skinType = await _skinTypeService.GetSkinTypeByIdAsync(id);
            if (skinType == null)
            {
                _logger.LogWarning("SkinType not found with ID: {SkinTypeId}", id);
                return NotFound(new { message = "SkinType not found" });
            }

            return Ok(skinType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching SkinType with ID {SkinTypeId}: {ErrorMessage}", id, ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    [HttpPost("create")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<SkinType>> CreateSkinType(CreateSkinTypeDto createSkinTypeDto)
    {
        try
        {
            _logger.LogInformation("Creating new SkinType");

            if (string.IsNullOrEmpty(createSkinTypeDto.SkinTypeName))
            {
                return BadRequest(new { message = "SkinTypeName is required" });
            }

            var createdSkinType = await _skinTypeService.CreateSkinTypeAsync(createSkinTypeDto);
            return Ok(createdSkinType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SkinType: {ErrorMessage}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("edit/{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateSkinType(string id, UpdateSkinTypeDto updateSkinTypeDto)
    {
        try
        {
            _logger.LogInformation("Updating SkinType with ID: {SkinTypeId}", id);

            
            var updatedSkinType = await _skinTypeService.UpdateSkinTypeAsync(id, updateSkinTypeDto);
            return Ok(updatedSkinType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SkinType with ID {SkinTypeId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteSkinType(string id)
    {
        try
        {
            _logger.LogInformation("Deleting SkinType with ID: {SkinTypeId}", id);

            var result = await _skinTypeService.DeleteSkinTypeAsync(id);
            if (!result)
            {
                return NotFound(new { message = "SkinType not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting SkinType with ID {SkinTypeId}: {ErrorMessage}", id, ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }
}