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
        public async Task<IActionResult> Index(int? pageNumber)    
        {
            var events = from e in _context.LoggedEvent
                         where e.UserId == User.Identity.Name
                         select e;

            int pageSize = 10;
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
