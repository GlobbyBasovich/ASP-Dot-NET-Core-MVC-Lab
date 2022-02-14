using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab10.Data;
using Lab10.Models;

namespace Lab10.Controllers
{
    [Route("api/Fixes")]
    [ApiController]
    public class FixesApiController : ControllerBase
    {
        private readonly kindergaldbContext _context;

        public FixesApiController(kindergaldbContext context)
        {
            _context = context;
        }

        // GET: api/Fixes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fix>>> GetFixes()
        {
            return await _context.Fixes.ToListAsync();
        }

        // GET: api/Fixes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fix>> GetFix(int id)
        {
            var fix = await _context.Fixes.FindAsync(id);

            if (fix == null)
            {
                return NotFound();
            }

            return fix;
        }

        // PUT: api/Fixes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFix(int id, Fix fix)
        {
            if (id != fix.Id)
            {
                return BadRequest();
            }

            _context.Entry(fix).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FixExists(id))
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

        // POST: api/Fixes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Fix>> PostFix(Fix fix)
        {
            _context.Fixes.Add(fix);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFix", new { id = fix.Id }, fix);
        }

        // DELETE: api/Fixes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFix(int id)
        {
            var fix = await _context.Fixes.FindAsync(id);
            if (fix == null)
            {
                return NotFound();
            }

            _context.Fixes.Remove(fix);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FixExists(int id)
        {
            return _context.Fixes.Any(e => e.Id == id);
        }
    }
}
