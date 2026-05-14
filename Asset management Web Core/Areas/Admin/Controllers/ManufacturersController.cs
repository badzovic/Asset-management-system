using AMS_data;
using AMS_data.Entities.Weapons;
using AMS_services.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asset_management_Web_Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManufacturersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly AuditLogService _auditLogService;

        public ManufacturersController(ApplicationDbContext db, AuditLogService auditLogService)
        {
            _db = db;
            _auditLogService = auditLogService;
        }

        public async Task<IActionResult> Index(string? search, string? status)
        {
            var query = _db.Manufacturers
            .Include(x => x.CountryLookup)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Code != null && x.Code.Contains(search)) ||
                    (x.CountryLookup != null && x.CountryLookup.Name.Contains(search)) ||
                    (x.Description != null && x.Description.Contains(search)));
            }

            if (status == "active")
                query = query.Where(x => x.IsActive);
            else if (status == "inactive")
                query = query.Where(x => !x.IsActive);

            ViewBag.Search = search;
            ViewBag.Status = status;

            var items = await query.OrderBy(x => x.Name).ToListAsync();

            return View(items);
        }
        private async Task LoadDropdowns()
        {
            ViewBag.Countries = await _db.LookupItems
                .Include(x => x.LookupCategory)
                .Where(x => x.LookupCategory.Key == "ManufacturerCountry" && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Manufacturer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Manufacturer model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            model.CreatedAt = DateTime.UtcNow;

            _db.Manufacturers.Add(model);
            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "CREATE_MANUFACTURER",
                "Manufacturer",
                model.Id.ToString(),
                newValues: new
                {
                    model.Name,
                    model.Code,
                    model.CountryLookupId,
                    model.Description,
                    model.IsActive
                });

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Manufacturers
                .Include(x => x.CountryLookup)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            await LoadDropdowns();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Manufacturer model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            var item = await _db.Manufacturers.FindAsync(id);

            if (item == null)
                return NotFound();

            var oldValues = new
            {
                item.Name,
                item.Code,
                item.Country,
                item.CountryLookupId,
                item.Description,
                item.IsActive
            };

            item.Name = model.Name;
            item.Code = model.Code;
            item.Country = model.Country;
            item.Description = model.Description;
            item.IsActive = model.IsActive;
            item.CountryLookupId = model.CountryLookupId;

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "EDIT_MANUFACTURER",
                "Manufacturer",
                item.Id.ToString(),
                oldValues,
                new
                {
                    item.Name,
                    item.Code,
                    item.Country,
                    item.Description,
                    item.IsActive
                });

            return RedirectToAction(nameof(Index));
        }
    }
}