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
using Koombu.Models.PostViewModels;

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

        // GET: Posts/Search/string
        public async Task<IActionResult> Search(string query)
        {
            List<Post> posts;
            if (query != null)
            {
                posts = await _context.Posts.Where(p => p.Content.Contains(query)).Include(p => p.Group).Include(p => p.User).ToListAsync();

            }
            else
            {
                posts = await _context.Posts.Include(p => p.Group).Include(p => p.User).ToListAsync();
            }
            return View("Index", posts);
        }

        // POST: like
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Group)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Attachments)
                .Include(p => p.Images)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            ApplicationUser user = UserHelper.GetCurrentUser(User.Identity.Name);

            UserLike oldLike = await _context.UserLikes.Where(ul => ul.PostId == id && ul.UserId == user.Id).FirstOrDefaultAsync();

            if (oldLike != null)
            {
                List<string> errors = new List<string>();
                errors.Add("You have already liked this post");

                TempData.Add("errors", errors);

                return RedirectToAction("Details", new { id });
            }

            UserLike like = new UserLike
            {
                UserId = user.Id,
                PostId = post.Id
            };

            post.Likes++;

            if (ModelState.IsValid)
            {
                _context.Update(post);
                _context.Add(like);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = post.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: unlike
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlike(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Group)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Attachments)
                .Include(p => p.Images)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            ApplicationUser user = UserHelper.GetCurrentUser(User.Identity.Name);

            UserLike like = await _context.UserLikes.Where(ul => ul.PostId == id && ul.UserId == user.Id).FirstOrDefaultAsync();

            if (like == null)
            {
                List<string> errors = new List<string>();
                errors.Add("You are not liking this post");

                TempData.Add("errors", errors);

                return RedirectToAction("Details", new { id });
            }
            
            post.Likes--;

            if (ModelState.IsValid)
            {
                _context.Update(post);
                _context.UserLikes.Remove(like);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = post.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post post = await _context.Posts
                .Include(p => p.Group)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Attachments)
                .Include(p => p.Images)
                .SingleOrDefaultAsync(m => m.Id == id);
            DetailsViewModel model = new DetailsViewModel
            {
                Post = post,
                Comment = new Comment(),
                Attachment = new Attachment(),
                Image = new Image()
            };
            if (post == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ApplicationUser user = UserHelper.GetCurrentUser(User.Identity.Name);
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.UserGroups.Where(ug => ug.User.Equals(user)).Any()), "Id", "Name");
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
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.UserGroups.Where(ug => ug.User.Equals(user)).Any()), "Id", "Name");
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
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.UserGroups.Where(ug => ug.User.Equals(user)).Any()), "Id", "Name");
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
            ViewData["GroupId"] = new SelectList(_context.Groups.Where(g => g.UserGroups.Where(ug => ug.User.Equals(user)).Any()), "Id", "Name");
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
