using SkinCare_Data.DTO.Dashboard;
using SkinCare_Data.Repositories;
using SkinCare_Service.Service.Iservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Service.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardDTO> GetDashboardOverviewAsync()
        {
            return await _dashboardRepository.GetDashboardOverviewAsync();
        }
    }
}
