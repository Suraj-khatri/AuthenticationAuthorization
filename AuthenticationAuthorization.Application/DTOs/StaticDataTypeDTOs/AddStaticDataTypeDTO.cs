using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs
{
    public class AddStaticDataTypeDTO
    {
        public string TypeTitle { get; set; } = null!;
        public string TypeDescription { get; set; } = null!;
    }
}
