using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab10.Data;
using Lab10.Models;

namespace Lab10.Controllers
{
    [Route("api/Appliances")]
    [ApiController]
    public class AppliancesApiController : ControllerBase
    {
        private readonly kindergaldbContext _context;

        public AppliancesApiController(kindergaldbContext context)
        {
            _context = context;
        }

        // GET: api/Appliances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appliance>>> GetAppliances()
        {
            return await _context.Appliances.ToListAsync();
        }

        // GET: api/Appliances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appliance>> GetAppliance(int id)
        {
            var appliance = await _context.Appliances.FindAsync(id);

            if (appliance == null)
            {
                return NotFound();
            }

            return appliance;
        }

        // PUT: api/Appliances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppliance(int id, Appliance appliance)
        {
            if (id != appliance.Id)
            {
                return BadRequest();
            }

            _context.Entry(appliance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplianceExists(id))
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

        // POST: api/Appliances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Appliance>> PostAppliance(Appliance appliance)
        {
            _context.Appliances.Add(appliance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppliance", new { id = appliance.Id }, appliance);
        }

        // DELETE: api/Appliances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppliance(int id)
        {
            var appliance = await _context.Appliances.FindAsync(id);
            if (appliance == null)
            {
                return NotFound();
            }

            _context.Appliances.Remove(appliance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplianceExists(int id)
        {
            return _context.Appliances.Any(e => e.Id == id);
        }
    }
}
