using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Models
{
    public class UserGroup
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string GroupId { get; set; }
        public Group Group { get; set; }
    }
}
