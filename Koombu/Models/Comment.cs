using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
