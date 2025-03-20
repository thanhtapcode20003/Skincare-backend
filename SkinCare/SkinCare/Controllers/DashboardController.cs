using Microsoft.AspNetCore.Mvc;
using SkinCare_Service.IService;
using SkinCare_Service.Service.Iservice;
using System.Threading.Tasks;

namespace SkinCare_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            var result = await _dashboardService.GetDashboardOverviewAsync();
            return Ok(result);
        }
    }
}
