using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Models
{
    public class UserFollow
    {
        public string FollowingId { get; set; }
        [InverseProperty("Followings")]
        public ApplicationUser Following { get; set; }

        public string FollowerId { get; set; }
        [InverseProperty("Followers")]
        public ApplicationUser Follower { get; set; }
    }
}
