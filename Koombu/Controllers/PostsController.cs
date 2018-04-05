using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Koombu.Data;
using Koombu.Models;
using Koombu.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Koombu.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
            UserHelper.Context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = UserHelper.GetCurrentUser(User.Identity.Name);
            var applicationDbContext = _context.Posts.Include(p => p.Group).Include(p => p.User);//.Where(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Group)
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ApplicationUser user = UserHelper.GetCurrentUser(User.Identity.Name);
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.UserGroups.Any(value => user.UserGroups.Contains(value))), "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,GroupId")] Post post)
        {
            ApplicationUser user = UserHelper.GetCurrentUser(User.Identity.Name);
            if (ModelState.IsValid)
            {
                post.User = user;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.UserGroups.Any(value => user.UserGroups.Contains(value))), "Id", "Name");
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            ApplicationUser user = UserHelper.GetCurrentUser(User.Identity.Name);
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.UserGroups.Any(value => user.UserGroups.Contains(value))), "Id", "Name");
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Content,Likes,UserId,GroupId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ApplicationUser user = UserHelper.GetCurrentUser(User.Identity.Name);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", post.GroupId);
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.UserGroups.Any(value => user.UserGroups.Contains(value))), "Id", "Name");
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Group)
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(string id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
