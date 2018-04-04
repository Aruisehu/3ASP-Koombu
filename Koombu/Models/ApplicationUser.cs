using System;
using System.Collections.Generic;
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
        public int Department { get; set; }
        public string Title { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
        public ICollection<Group> OwnerGroups { get; } = new List<Group>();
        public ICollection<Post> Posts { get; } = new List<Post>();
        public ICollection<Comment> Comments { get; } = new List<Comment>();

    }
}
