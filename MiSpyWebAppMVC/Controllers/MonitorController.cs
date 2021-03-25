using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiSpyWebAppMVC.Data;
using MiSpyWebAppMVC.Models;

namespace MiSpyWebAppMVC.Controllers
{
    [Authorize] 
    public class MonitorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonitorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MonitorManager
        public async Task<IActionResult> Index(int? pageNumber, int pageSize, string searchBy, string sortBy)    
        {
            var events = from e in _context.LoggedEvent select e; //Getting Events from the Database

            if (!String.IsNullOrEmpty(searchBy))
            {
                events = events.Where(x => x.UserId == User.Identity.Name && x.Event.Contains(searchBy)); //Getting all reports which match the search criteria
            }
            else
            {
                events = events.Where(x => x.UserId == User.Identity.Name).OrderBy(s => s.Id); //Get events in just ID order
            }

            if (pageSize < 5)
            {
                pageSize = 5; //Minimum Page Size is 5 reports per page.
            }

            switch (sortBy)
            {
                case "Time":
                    events = events.Where(x => x.UserId == User.Identity.Name).OrderBy(s => s.Time); //Ordering by Time of Event
                    break;
                case "HasRead":
                    events = events.Where(x => x.UserId == User.Identity.Name).OrderBy(s => s.HasRead); //Ordering by true and false of events being read in the reporting app.
                    break;
                default:
                    events = events.Where(x => x.UserId == User.Identity.Name); //Default Reporting. 
                    break;
            }

            await events.ToListAsync();

            return View(await PaginatedList<LoggedEvent>.CreateAsync(events.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        //GET: Monitor/EventDetails/1
        public async Task<IActionResult> EventDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailedEvent = await _context.LoggedEvent
                .FirstOrDefaultAsync(e => e.Id == id);

            if (detailedEvent == null)
            {
                return NotFound();
            }

            return View(detailedEvent);
        }
    }
}
