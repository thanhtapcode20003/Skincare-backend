using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class User
    {
        [Key]
        public string UserId { get; set; }

        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        public string? SkinTypeId { get; set; }
        [ForeignKey(nameof(SkinTypeId))]
        public SkinType SkinType { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int? Point { get; set; }
        public DateTime CreateAt { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Quizz> Quizzes { get; set; }
    }
}
