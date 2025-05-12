using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Options
{
    public class JwtOption
    {
        public const string SectionName = "JWT";
        public string Secret { get; set; } = null!;
        public string ValidIssuer { get; set; } = null!;
        public string ValidAudience { get; set; } = null!;
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidity { get; set; }
    }
}
