using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Koombu.Data;
using Koombu.Models;
using Microsoft.AspNetCore.Authorization;
using Koombu.Models.GroupViewModels;
using Koombu.Helpers;

namespace Koombu.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
            UserHelper.Context = _context;
        }

        // GET: Groups
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Groups.Include(g => g.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Groups/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Owner)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (@group == null)
            {
                return NotFound();
            }

            var @userGroups = await _context.UserGroups
                .Include(u => u.User)
                .Where(m => m.GroupId == id)
                .ToListAsync();

            var user = UserHelper.GetCurrentUser(User.Identity.Name);

            DetailsViewModel model = new DetailsViewModel();
            model.Group = @group;
            model.UserGroups = @userGroups;
            model.User = user;

            return View(model);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Group @group)
        {
            var user = UserHelper.GetCurrentUser(User.Identity.Name);

            @group.OwnerId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(@group);
                UserGroup join = new UserGroup
                {
                    Group = @group,
                    UserId = user.Id
                };
                _context.Add(join);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(@group);
        }

        // GET: Groups/NotOwner
        public async Task<IActionResult> NotOwner()
        {
            return View();
        }

        // GET: Groups/AddUser
        public async Task<IActionResult> AddUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Owner)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (@group == null)
            {
                return NotFound();
            }

            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();

            if (@group.OwnerId != user.Id)
            {
                return RedirectToAction("NotOwner");
            }

            return View();
        }

        // POST: Groups/AddUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(string id, string userEmail)
        {
            if (id == null || userEmail == null)
            {
                return NotFound();
            }

            var @addedUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);

            var @group = await _context.Groups
                .Include(g => g.Owner)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (@group == null || @addedUser == null)
            {
                return NotFound();
            }

            var user = UserHelper.GetCurrentUser(User.Identity.Name);

            if (@group.OwnerId != user.Id)
            {
                return RedirectToAction("NotOwner");
            }

            UserGroup join = new UserGroup
            {
                GroupId = @group.Id,
                UserId = @addedUser.Id
            };

            if (ModelState.IsValid)
            {
                _context.Add(join);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = @group.Id });
            }

            return View(@group);
        }

        public async Task<IActionResult> RemoveUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Owner)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (@group == null)
            {
                return NotFound();
            }

            var user = UserHelper.GetCurrentUser(User.Identity.Name);

            if (@group.OwnerId != user.Id)
            {
                return RedirectToAction("NotOwner");
            }

            return View(@group);
        }

        // POST: Groups/RemoveUser
        [HttpPost, ActionName("RemoveUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUser(string id, string userEmail)
        {
            if (id == null || userEmail == null)
            {
                return NotFound();
            }

            var @addedUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);

            var @group = await _context.Groups
                .Include(g => g.Owner)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (@group == null || @addedUser == null)
            {
                return NotFound();
            }

            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();

            if (@group.OwnerId != user.Id)
            {
                return RedirectToAction("NotOwner");
            }

            UserGroup join = await _context.UserGroups.Where(ug => ug.UserId == @addedUser.Id && ug.GroupId == @group.Id).FirstOrDefaultAsync();

            if (join == null)
            {
                return NotFound();
            }

            _context.UserGroups.Remove(join);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = @group.Id });
        }

        public async Task<IActionResult> RemoveSelf(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Owner)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (@group == null)
            {
                return NotFound();
            }

            var user = UserHelper.GetCurrentUser(User.Identity.Name);

            UserGroup join = await _context.UserGroups.Where(ug => ug.UserId == user.Id && ug.GroupId == @group.Id).FirstOrDefaultAsync();

            if (join == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/RemoveSelf
        [HttpPost, ActionName("RemoveSelf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSelfPost(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Owner)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (@group == null)
            {
                return NotFound();
            }

            var user = UserHelper.GetCurrentUser(User.Identity.Name);

            UserGroup join = await _context.UserGroups.Where(ug => ug.UserId == user.Id && ug.GroupId == @group.Id).FirstOrDefaultAsync();

            if (join == null)
            {
                return NotFound();
            }

            _context.UserGroups.Remove(join);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = @group.Id });
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();

            if (@group.OwnerId != user.Id)
            {
                return RedirectToAction("NotOwner");
            }

            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            var user = UserHelper.GetCurrentUser(User.Identity.Name);

            if (@group.OwnerId != user.Id)
            {
                return RedirectToAction("NotOwner");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
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

            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Owner)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            var user = UserHelper.GetCurrentUser(User.Identity.Name);

            if (@group.OwnerId != user.Id)
            {
                return RedirectToAction("NotOwner");
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);

            var user = UserHelper.GetCurrentUser(User.Identity.Name);

            if (@group.OwnerId != user.Id)
            {
                return RedirectToAction("NotOwner");
            }

            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(string id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
