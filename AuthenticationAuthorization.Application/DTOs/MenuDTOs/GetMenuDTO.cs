using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.MenuDTOs
{
    public class GetMenuDTO
    {
        public int Id { get; set; }
        public string MenuName { get; set; } = null!;

        public int? ParentId { get; set; }

        public string? Icon { get; set; }

        public string? Route { get; set; }

        public bool IsUser { get; set; } = false;

        public int SortOrder { get; set; }
    }
}
