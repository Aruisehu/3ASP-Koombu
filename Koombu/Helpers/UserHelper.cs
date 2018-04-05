using Koombu.Data;
using Koombu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Koombu.Helpers
{
    public static class UserHelper
    {
        public static ApplicationDbContext Context { get; set; }

        public static ApplicationUser GetCurrentUser(string name)
        {
            return Context.ApplicationUsers.Where(u => u.Email == name).FirstOrDefault();
        }
    }
}
