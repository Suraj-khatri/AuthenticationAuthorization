using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Models
{
    public class UserToken : BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // Or int if using integer IDs
        public string LoginProvider { get; set; }  // Like "YourApp"
        public string TokenName { get; set; }     // Like "RefreshToken" or "RefreshTokenExpiry"
        public string TokenValue { get; set; }    // The actual token or expiry value
        public DateTime? Expiry { get; set; } // Optional separate expiry field
                                              


        // Helper property
        public bool IsExpired => Expiry.HasValue && DateTime.UtcNow > Expiry.Value;
    }
}
