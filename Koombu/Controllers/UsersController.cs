using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Koombu.Data;
using Koombu.Models;

namespace Koombu.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users/Search
        public async Task<IActionResult> Search(string query)
        {
            List<ApplicationUser> users = await _context.ApplicationUsers.Where(u => u.FullName.Contains(query)).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = await _context.ApplicationUsers.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            ApplicationUser user = await _context.ApplicationUsers.FindAsync(id);
            return View(user);
        }
    }
}
