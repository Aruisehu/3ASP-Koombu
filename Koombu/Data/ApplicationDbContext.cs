using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Koombu.Models;
using Microsoft.AspNetCore.Identity;

namespace Koombu.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims").HasKey(uc => uc.Id);
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins").HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles").HasKey(k => new {k.RoleId, k.UserId});
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasKey(ut => new { ut.LoginProvider, ut.UserId, ut.Name });
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims").HasKey(rc => rc.Id);

        }
    }
}
