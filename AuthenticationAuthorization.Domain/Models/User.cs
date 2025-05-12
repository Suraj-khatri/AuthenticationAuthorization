using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class User : BaseEntity
    {
        [MaxLength(50)]
        public string UserName { get; set; } = null!;

        [MaxLength(100)]
        public string UserPassword { get; set; } = null!;

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public bool IsTemporary { get; set; }

        public DateTime? LastPasswordChangedOn { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
