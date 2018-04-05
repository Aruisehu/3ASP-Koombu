using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Models.GroupViewModels
{
    public class DetailsViewModel
    {
        public Group group { get; set; }

        public List<UserGroup> userGroups { get; set; }
    }
}
