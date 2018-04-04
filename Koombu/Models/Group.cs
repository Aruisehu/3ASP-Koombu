using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Models
{
    public class Group
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }

        public ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
        public ICollection<Post> Posts { get; } = new List<Post>();

    }
}
