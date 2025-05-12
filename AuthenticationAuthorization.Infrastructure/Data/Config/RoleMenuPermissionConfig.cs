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
    public class RoleMenuPermissionConfiguration : IEntityTypeConfiguration<RoleMenuPermission>
    {
        public void Configure(EntityTypeBuilder<RoleMenuPermission> builder)
        {
            builder.HasKey(rmp => new { rmp.RoleId, rmp.MenuPermissionId });

            builder.HasOne(rmp => rmp.Role)
                   .WithMany(r => r.RoleMenuPermissions)
                   .HasForeignKey(rmp => rmp.RoleId);

            builder.HasOne(rmp => rmp.MenuPermission)
                   .WithMany(mp => mp.RoleMenuPermissions)
                   .HasForeignKey(rmp => rmp.MenuPermissionId);
        }
    }
}
