﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Models
{
    public class UserLike
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}
