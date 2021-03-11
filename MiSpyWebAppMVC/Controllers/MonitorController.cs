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
    public class MonitorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonitorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MonitorManager
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)    
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var events = from e in _context.LoggedEvent
                         where e.UserId == User.Identity.Name
                         select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    events = events.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    events = events.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    events = events.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    events = events.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}
