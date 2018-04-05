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
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Group> Groups{ get; set; }
        public DbSet<Post> Posts{ get; set; }
        public DbSet<UserGroup> UserGroups{ get; set; }

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
            builder.Entity<Group>().ToTable("Groups").HasKey(g => g.Id);
            builder.Entity<Group>().HasOne(g => g.Owner).WithMany(u => u.OwnerGroups);
            builder.Entity<UserGroup>().ToTable("UserGroups").HasKey(sc => new { sc.UserId, sc.GroupId });
            builder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(s => s.UserGroups)
                .HasForeignKey(ug => ug.UserId);
            builder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            builder.Entity<Post>().ToTable("Posts").HasKey(p => p.Id);
            builder.Entity<Post>().HasOne(p => p.User).WithMany(u => u.Posts);

            builder.Entity<Comment>().ToTable("Comments").HasKey(c => c.Id);
            builder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments);
            builder.Entity<Comment>().HasOne(c => c.User).WithMany(u => u.Comments);

            builder.Entity<Attachment>().ToTable("Attachments").HasKey(a => a.Id);
            builder.Entity<Attachment>().HasOne(a => a.Post).WithMany(p => p.Attachments);

            builder.Entity<UserFollow>().ToTable("UserFollows").HasKey(uf => new { uf.FollowingId, uf.FollowerId });
            builder.Entity<UserFollow>().HasOne(uf => uf.Following).WithMany(u => u.Followings);
            builder.Entity<UserFollow>().HasOne(uf => uf.Follower).WithMany(u => u.Followers);
            



        }
    }
}
