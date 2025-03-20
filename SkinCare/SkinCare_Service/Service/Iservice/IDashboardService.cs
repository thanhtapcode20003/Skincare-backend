using SkinCare_Data.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Service.Service.Iservice
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardOverviewAsync();
    }
}
