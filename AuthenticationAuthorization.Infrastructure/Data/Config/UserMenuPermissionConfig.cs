using AuthenticationAuthorization.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Infrastructure.Data.Config
{
    public class UserMenuPermissionConfiguration : IEntityTypeConfiguration<UserMenuPermission>
    {
        public void Configure(EntityTypeBuilder<UserMenuPermission> builder)
        {
            builder.HasKey(ump => new { ump.RoleId, ump.MenuPermissionId });

            builder.HasOne(ump => ump.Role)
                   .WithMany(r => r.UserMenuPermissions)
                   .HasForeignKey(ump => ump.RoleId);
        }
    }
}
