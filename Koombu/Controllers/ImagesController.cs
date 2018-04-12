using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Koombu.Data;
using Koombu.Models;
using Koombu.Models.PostViewModels;

namespace Koombu.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: Attachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Image.PostId = model.Post.Id;
                _context.Add(model.Image);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Posts", new { id = model.Post.Id });
        }

        // POST: Attachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string postId)
        {
            var attachment = await _context.Attachments.SingleOrDefaultAsync(m => m.Id == id);
            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new { id = postId });
        }
    }
}
