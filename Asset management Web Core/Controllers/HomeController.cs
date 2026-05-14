using System.Diagnostics;
using AMS_data;
using Asset_management_Web_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Asset_management_Web_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            Log.Information("AMS Home page opened");

            var model = new DashboardViewModel
            {
                TotalWeapons = await _db.Weapons.CountAsync(x => !x.IsDeleted),
                MarkedWeapons = await _db.Weapons.CountAsync(x => !x.IsDeleted && x.IsMarked),
                UnmarkedWeapons = await _db.Weapons.CountAsync(x => !x.IsDeleted && !x.IsMarked),
                TotalChecks = await _db.WeaponChecks.CountAsync(x => !x.IsDeleted),

                RecentWeapons = await _db.Weapons
                    .Include(x => x.WeaponModel)
                    .Include(x => x.WeaponType)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Caliber)
                    .Where(x => !x.IsDeleted)
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(8)
                    .ToListAsync(),

                RecentChecks = await _db.WeaponChecks
                    .Include(x => x.Weapon)
                        .ThenInclude(x => x.WeaponModel)
                    .Include(x => x.CheckStateLookup)
                    .Where(x => !x.IsDeleted)
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(8)
                    .ToListAsync()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}