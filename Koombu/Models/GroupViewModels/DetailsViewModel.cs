using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Models.GroupViewModels
{
    public class DetailsViewModel
    {
        public Group Group { get; set; }

        public ApplicationUser User { get; set; }

        public List<UserGroup> UserGroups { get; set; }
    }
}
