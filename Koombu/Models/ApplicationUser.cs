using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Koombu.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; }}
        public int Department { get; set; }
        public string Title { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
        public ICollection<Group> OwnerGroups { get; } = new List<Group>();
        public ICollection<Post> Posts { get; } = new List<Post>();
        public ICollection<Comment> Comments { get; } = new List<Comment>();
        [InverseProperty("Following")]
        public ICollection<UserFollow> Followings { get; } = new List<UserFollow>();
        [InverseProperty("Follower")]
        public ICollection<UserFollow> Followers { get; } = new List<UserFollow>();
        public ICollection<UserLike> UserLikes { get; } = new List<UserLike>();

    }
}
