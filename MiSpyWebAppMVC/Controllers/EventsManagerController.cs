using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiSpyWebAppMVC.Data;
using MiSpyWebAppMVC.Models;

namespace MiSpyWebAppMVC.Controllers
{
    public class EventsManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventsManager
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoggedEvent.ToListAsync());
        }

        // GET: EventsManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loggedEvent = await _context.LoggedEvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loggedEvent == null)
            {
                return NotFound();
            }

            return View(loggedEvent);
        }

        // GET: EventsManager/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventsManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Event,Time,UserId,HasRead")] LoggedEvent loggedEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loggedEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loggedEvent);
        }

        // GET: EventsManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loggedEvent = await _context.LoggedEvent.FindAsync(id);
            if (loggedEvent == null)
            {
                return NotFound();
            }
            return View(loggedEvent);
        }

        // POST: EventsManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Event,Time,UserId,HasRead")] LoggedEvent loggedEvent)
        {
            if (id != loggedEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loggedEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoggedEventExists(loggedEvent.Id))
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
            return View(loggedEvent);
        }

        // GET: EventsManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loggedEvent = await _context.LoggedEvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loggedEvent == null)
            {
                return NotFound();
            }

            return View(loggedEvent);
        }

        // POST: EventsManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loggedEvent = await _context.LoggedEvent.FindAsync(id);
            _context.LoggedEvent.Remove(loggedEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoggedEventExists(int id)
        {
            return _context.LoggedEvent.Any(e => e.Id == id);
        }
    }
}
