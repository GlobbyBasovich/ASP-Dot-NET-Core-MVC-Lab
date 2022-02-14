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
    public class WorksController : Controller
    {
        private readonly kindergaldbContext _context;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public WorksController(kindergaldbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _client = clientFactory.CreateClient("repairs");
        }

        // GET: Works
        public async Task<IActionResult> Index()
        {
            //var kindergaldbContext = _context.Works.Include(w => w.ApplianceNavigation).Include(w => w.FixNavigation);
            //return View(await kindergaldbContext.ToListAsync());
            var works = await _client.GetFromJsonAsync<List<Work>>("Works", jsonSerializerOptions);
            foreach (var work in works)
            {
                var appliance = await _client.GetFromJsonAsync<Appliance>($"Appliances/{work.Appliance}", jsonSerializerOptions);
                work.ApplianceNavigation = appliance;
                var fix = await _client.GetFromJsonAsync<Fix>($"Fixes/{work.Fix}", jsonSerializerOptions);
                work.FixNavigation = fix;
            }
            return View(works);
        }

        // GET: Works/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var work = await _context.Works
            //    .Include(w => w.ApplianceNavigation)
            //    .Include(w => w.FixNavigation)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (work == null)
            //{
            //    return NotFound();
            //}
            var response = await _client.GetAsync($"Works/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var work = await response.Content.ReadFromJsonAsync<Work>(jsonSerializerOptions);
            var appliance = await _client.GetFromJsonAsync<Appliance>($"Appliances/{work.Appliance}", jsonSerializerOptions);
            work.ApplianceNavigation = appliance;
            var fix = await _client.GetFromJsonAsync<Fix>($"Fixes/{work.Fix}", jsonSerializerOptions);
            work.FixNavigation = fix;

            return View(work);
        }

        // GET: Works/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            //ViewData["Appliance"] = new SelectList(_context.Appliances, "Id", "Name");
            //ViewData["Fix"] = new SelectList(_context.Fixes, "Id", "Name");
            var appliances = await _client.GetFromJsonAsync<List<Appliance>>("Appliances", jsonSerializerOptions);
            ViewData["Appliance"] = new SelectList(appliances, "Id", "Name");
            var fixes = await _client.GetFromJsonAsync<List<Fix>>("Fixes", jsonSerializerOptions);
            ViewData["Fix"] = new SelectList(fixes, "Id", "Name");
            return View();
        }

        // POST: Works/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Appliance,Count,Fix,Date")] Work work)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(work);
                //await _context.SaveChangesAsync();
                await _client.PostAsJsonAsync("Works", work);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["Appliance"] = new SelectList(_context.Appliances, "Id", "Name", work.Appliance);
            //ViewData["Fix"] = new SelectList(_context.Fixes, "Id", "Name", work.Fix);
            var appliances = await _client.GetFromJsonAsync<List<Appliance>>("Appliances", jsonSerializerOptions);
            ViewData["Appliance"] = new SelectList(appliances, "Id", "Name", work.Appliance);
            var fixes = await _client.GetFromJsonAsync<List<Fix>>("Fixes", jsonSerializerOptions);
            ViewData["Fix"] = new SelectList(fixes, "Id", "Name", work.Fix);
            return View(work);
        }

        // GET: Works/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var work = await _context.Works.FindAsync(id);
            //if (work == null)
            //{
            //    return NotFound();
            //}
            //ViewData["Appliance"] = new SelectList(_context.Appliances, "Id", "Name", work.Appliance);
            //ViewData["Fix"] = new SelectList(_context.Fixes, "Id", "Name", work.Fix);
            var response = await _client.GetAsync($"Works/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var work = await response.Content.ReadFromJsonAsync<Work>(jsonSerializerOptions);
            var appliances = await _client.GetFromJsonAsync<List<Appliance>>("Appliances", jsonSerializerOptions);
            ViewData["Appliance"] = new SelectList(appliances, "Id", "Name", work.Appliance);
            var fixes = await _client.GetFromJsonAsync<List<Fix>>("Fixes", jsonSerializerOptions);
            ViewData["Fix"] = new SelectList(fixes, "Id", "Name", work.Fix);
            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Appliance,Count,Fix,Date")] Work work)
        {
            if (id != work.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(work);
                    //await _context.SaveChangesAsync();
                    await _client.PutAsJsonAsync($"Works/{id}", work);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await WorkExistsAsync(work.Id))
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
            //ViewData["Appliance"] = new SelectList(_context.Appliances, "Id", "Name", work.Appliance);
            //ViewData["Fix"] = new SelectList(_context.Fixes, "Id", "Name", work.Fix);
            var appliances = await _client.GetFromJsonAsync<List<Appliance>>("Appliances", jsonSerializerOptions);
            ViewData["Appliance"] = new SelectList(appliances, "Id", "Name", work.Appliance);
            var fixes = await _client.GetFromJsonAsync<List<Fix>>("Fixes", jsonSerializerOptions);
            ViewData["Fix"] = new SelectList(fixes, "Id", "Name", work.Fix);
            return View(work);
        }

        // GET: Works/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var work = await _context.Works
            //    .Include(w => w.ApplianceNavigation)
            //    .Include(w => w.FixNavigation)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (work == null)
            //{
            //    return NotFound();
            //}
            var response = await _client.GetAsync($"Works/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var work = await response.Content.ReadFromJsonAsync<Work>(jsonSerializerOptions);
            var appliance = await _client.GetFromJsonAsync<Appliance>($"Appliances/{work.Appliance}", jsonSerializerOptions);
            work.ApplianceNavigation = appliance;
            var fix = await _client.GetFromJsonAsync<Fix>($"Fixes/{work.Fix}", jsonSerializerOptions);
            work.FixNavigation = fix;

            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var work = await _context.Works.FindAsync(id);
            //_context.Works.Remove(work);
            //await _context.SaveChangesAsync();
            await _client.DeleteAsync($"Works/{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> WorkExistsAsync(int id)
        {
            //return _context.Works.Any(e => e.Id == id);
            var works = await _client.GetFromJsonAsync<List<Work>>("Works", jsonSerializerOptions);
            return works.Any(e => e.Id == id);
        }
    }
}
