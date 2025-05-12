using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class StaticDataType : BaseEntity
    {
        public string TypeTitle { get; set; } = null!;
        public string TypeDescription { get; set; } = null!;
        public bool IsActive { get; set; }
        public List<StaticDataDetail> StaticDataDetails { get; set; }
    }
}
