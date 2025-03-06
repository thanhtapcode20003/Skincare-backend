using SkinCare_Data.Data;
using SkinCare_Data.DTO.Login;
using SkinCare_Data.DTO.Register;
using System.Threading.Tasks;

namespace SkinCare_Service.IService
{
    public interface IAuthService
    {
        Task<(string Token, string RefreshToken)> LoginAsync(string email, string password);
        Task<bool> RegisterUser(RegisterRequest request);
    }
}