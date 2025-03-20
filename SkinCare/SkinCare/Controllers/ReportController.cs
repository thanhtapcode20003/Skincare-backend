using Microsoft.AspNetCore.Mvc;
using SkinCare_Service.IService;
using SkinCare_Service.Service.Iservice;
using System.Threading.Tasks;

namespace SkinCare_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueReport([FromQuery] string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return BadRequest("Type parameter is required. Use 'day', 'month', or 'year'.");
            }

            var result = await _reportService.GetRevenueReportAsync(type);
            return Ok(result);
        }

        [HttpGet("order-status")]
        public async Task<IActionResult> GetOrderStatusReport()
        {
            var result = await _reportService.GetOrdersByStatusReportAsync();
            return Ok(result);
        }
    }
}
