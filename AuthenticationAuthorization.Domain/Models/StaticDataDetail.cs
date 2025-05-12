using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class StaticDataDetail : BaseEntity
    {
        public string DetailName { get; set; } = null!;
        public string DetailDesc { get; set; } = null!;

        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public StaticDataType StaticDataType { get; set; } = null!;

    }
}
