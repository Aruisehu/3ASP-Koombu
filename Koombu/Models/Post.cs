using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Models
{
    public class Post
    {
        public string Id { get; set; } 
        public string Content  { get; set; } 
        public int Likes { get; set; } 
        public string UserId { get; set; } 
        public ApplicationUser User { get; set; } 
        public string GroupId { get; set; } 
        public Group Group { get; set; } 

        public ICollection<Comment> Comments { get; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; } = new List<Attachment>();
        public ICollection<UserLike> UserLikes { get; } = new List<UserLike>();
    }
}
