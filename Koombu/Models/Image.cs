using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Models
{
    public class Image
    {
        [Key]
        public string Id { get; set; }
        public string Url { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}
