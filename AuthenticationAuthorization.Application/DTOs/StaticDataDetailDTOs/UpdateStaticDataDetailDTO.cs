using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.StaticDataDetailDTOs
{
    public class UpdateStaticDataDetailDTO
    {
        public string DetailName { get; set; } = null!;
        public string DetailDesc { get; set; } = null!;
        public int TypeId { get; set; }
    }
}
