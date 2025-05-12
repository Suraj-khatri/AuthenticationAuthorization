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
    public class MenuPermissionConfiguration : IEntityTypeConfiguration<MenuPermission>
    {
        public void Configure(EntityTypeBuilder<MenuPermission> builder)
        {
            builder.HasOne(mp => mp.Menu)
                   .WithMany(m => m.MenuPermissions)
                   .HasForeignKey(mp => mp.MenuId);

            builder.HasOne(mp => mp.Permission)
                   .WithMany(p => p.MenuPermissions)
                   .HasForeignKey(mp => mp.PermissionId);

            builder.HasMany(mp => mp.Roles)
                   .WithMany(r => r.MenuPermissions)
                   .UsingEntity(j => j.ToTable("MenuPermissionRoles"));
        }
    }
}
