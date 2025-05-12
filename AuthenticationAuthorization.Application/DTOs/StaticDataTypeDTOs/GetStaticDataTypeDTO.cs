using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs
{
    public class GetStaticDataTypeDTO
    {
        public int Id { get; set; }
        public string TypeTitle { get; set; } = null!;
        public string TypeDescription { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
