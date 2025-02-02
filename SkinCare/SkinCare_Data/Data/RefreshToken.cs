using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class RefreshToken
    {
        public int Id { get; set; } // Khóa chính
        public string Token { get; set; } // Refresh token
        public string UserId { get; set; } // ID của người dùng
        public DateTime ExpiryDate { get; set; } // Thời gian hết hạn
    }
}
