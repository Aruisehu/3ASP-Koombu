﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Koombu.Data;
using Koombu.Models;

namespace Koombu.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Attachments")]
    public class AttachmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttachmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Attachments
        [HttpGet]
        public IEnumerable<Attachment> GetAttachments()
        {
            return _context.Attachments;
        }

        // GET: api/Attachments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttachment([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var attachment = await _context.Attachments.SingleOrDefaultAsync(m => m.Id == id);

            if (attachment == null)
            {
                return NotFound();
            }

            return Ok(attachment);
        }

        // PUT: api/Attachments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttachment([FromRoute] string id, [FromBody] Attachment attachment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attachment.Id)
            {
                return BadRequest();
            }

            _context.Entry(attachment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttachmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Attachments
        [HttpPost]
        public async Task<IActionResult> PostAttachment([FromBody] Attachment attachment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttachment", new { id = attachment.Id }, attachment);
        }

        // DELETE: api/Attachments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var attachment = await _context.Attachments.SingleOrDefaultAsync(m => m.Id == id);
            if (attachment == null)
            {
                return NotFound();
            }

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();

            return Ok(attachment);
        }

        private bool AttachmentExists(string id)
        {
            return _context.Attachments.Any(e => e.Id == id);
        }
    }
}