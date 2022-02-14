using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab10.Data;
using Lab10.Models;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;

namespace Lab10.Controllers
{
    public class FixesController : Controller
    {
        private readonly kindergaldbContext _context;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public FixesController(kindergaldbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _client = clientFactory.CreateClient("repairs");
        }

        // GET: Fixes
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Fixes.ToListAsync());
            var fixes = await _client.GetFromJsonAsync<List<Fix>>("Fixes", jsonSerializerOptions);
            return View(fixes);
        }

        // GET: Fixes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var fix = await _context.Fixes
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (fix == null)
            //{
            //    return NotFound();
            //}
            var response = await _client.GetAsync($"Fixes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var fix = await response.Content.ReadFromJsonAsync<Fix>(jsonSerializerOptions);

            return View(fix);
        }

        // GET: Fixes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fixes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Weight")] Fix fix)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(fix);
                //await _context.SaveChangesAsync();
                await _client.PostAsJsonAsync("Fixes", fix);
                return RedirectToAction(nameof(Index));
            }
            return View(fix);
        }

        // GET: Fixes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var fix = await _context.Fixes.FindAsync(id);
            //if (fix == null)
            //{
            //    return NotFound();
            //}
            var response = await _client.GetAsync($"Fixes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var fix = await response.Content.ReadFromJsonAsync<Fix>(jsonSerializerOptions);
            return View(fix);
        }

        // POST: Fixes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Weight")] Fix fix)
        {
            if (id != fix.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(fix);
                    //await _context.SaveChangesAsync();
                    await _client.PutAsJsonAsync($"Fixes/{id}", fix);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FixExistsAsync(fix.Id))
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
            return View(fix);
        }

        // GET: Fixes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var fix = await _context.Fixes
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (fix == null)
            //{
            //    return NotFound();
            //}
            var response = await _client.GetAsync($"Fixes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var fix = await response.Content.ReadFromJsonAsync<Fix>(jsonSerializerOptions);
            return View(fix);
        }

        // POST: Fixes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var fix = await _context.Fixes.FindAsync(id);
            //_context.Fixes.Remove(fix);
            //await _context.SaveChangesAsync();
            await _client.DeleteAsync($"Fixes/{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> FixExistsAsync(int id)
        {
            //return _context.Fixes.Any(e => e.Id == id);
            var fixes = await _client.GetFromJsonAsync<List<Fix>>("Fixes", jsonSerializerOptions);
            return fixes.Any(e => e.Id == id);
        }
    }
}
