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
    public class AppliancesController : Controller
    {
        private readonly kindergaldbContext _context;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public AppliancesController(kindergaldbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _client = clientFactory.CreateClient("repairs");
        }

        // GET: Appliances
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Appliances.ToListAsync());
            var appliances = await _client.GetFromJsonAsync<List<Appliance>>("Appliances", jsonSerializerOptions);
            return View(appliances);
        }

        // GET: Appliances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var appliance = await _context.Appliances
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (appliance == null)
            //{
            //    return NotFound();
            //}
            var response = await _client.GetAsync($"Appliances/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var appliance = await response.Content.ReadFromJsonAsync<Appliance>(jsonSerializerOptions);

            return View(appliance);
        }

        // GET: Appliances/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appliances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Weight")] Appliance appliance)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(appliance);
                //await _context.SaveChangesAsync();
                await _client.PostAsJsonAsync("Appliances", appliance);
                return RedirectToAction(nameof(Index));
            }
            return View(appliance);
        }

        // GET: Appliances/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var appliance = await _context.Appliances.FindAsync(id);
            //if (appliance == null)
            //{
            //    return NotFound();
            //}
            var response = await _client.GetAsync($"Appliances/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var appliance = await response.Content.ReadFromJsonAsync<Appliance>(jsonSerializerOptions);
            return View(appliance);
        }

        // POST: Appliances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Weight")] Appliance appliance)
        {
            if (id != appliance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(appliance);
                    //await _context.SaveChangesAsync();
                    await _client.PutAsJsonAsync($"Appliances/{id}", appliance);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ApplianceExistsAsync(appliance.Id))
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
            return View(appliance);
        }

        // GET: Appliances/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var appliance = await _context.Appliances
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (appliance == null)
            //{
            //    return NotFound();
            //}
            var response = await _client.GetAsync($"Appliances/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var appliance = await response.Content.ReadFromJsonAsync<Appliance>(jsonSerializerOptions);
            return View(appliance);
        }

        // POST: Appliances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var appliance = await _context.Appliances.FindAsync(id);
            //_context.Appliances.Remove(appliance);
            //await _context.SaveChangesAsync();
            await _client.DeleteAsync($"Appliances/{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ApplianceExistsAsync(int id)
        {
            //return _context.Appliances.Any(e => e.Id == id);
            var appliances = await _client.GetFromJsonAsync<List<Appliance>>("Appliances", jsonSerializerOptions);
            return appliances.Any(e => e.Id == id);
        }
    }
}
