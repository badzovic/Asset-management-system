using AMS_data;
using AMS_data.Entities.Weapons;
using AMS_services.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var query = _db.Manufacturers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Code != null && x.Code.Contains(search)) ||
                    (x.Country != null && x.Country.Contains(search)) ||
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

        public IActionResult Create()
        {
            return View(new Manufacturer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Manufacturer model)
        {
            if (!ModelState.IsValid)
                return View(model);

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
                    model.Country,
                    model.Description,
                    model.IsActive
                });

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Manufacturers.FindAsync(id);

            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Manufacturer model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var item = await _db.Manufacturers.FindAsync(id);

            if (item == null)
                return NotFound();

            var oldValues = new
            {
                item.Name,
                item.Code,
                item.Country,
                item.Description,
                item.IsActive
            };

            item.Name = model.Name;
            item.Code = model.Code;
            item.Country = model.Country;
            item.Description = model.Description;
            item.IsActive = model.IsActive;

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