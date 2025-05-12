using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class Menu : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string MenuName { get; set; } = null!;

        public int? ParentId { get; set; }

        [MaxLength(100)]
        public string? Icon { get; set; }

        [MaxLength(255)]
        public string? Route { get; set; }


        public int SortOrder { get; set; }
        public ICollection<MenuPermission> MenuPermissions { get; set; } = new List<MenuPermission>();


    }
}
