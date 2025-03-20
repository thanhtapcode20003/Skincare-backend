using System.Threading.Tasks;
using SkinCare_Data.DTO.Dashboard;

namespace SkinCare_Data.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardDTO> GetDashboardOverviewAsync();
    }
}
