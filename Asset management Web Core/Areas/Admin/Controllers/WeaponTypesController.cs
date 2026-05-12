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
    public class WeaponTypesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly AuditLogService _auditLogService;

        public WeaponTypesController(
            ApplicationDbContext db,
            AuditLogService auditLogService)
        {
            _db = db;
            _auditLogService = auditLogService;
        }

        public async Task<IActionResult> Index(string? search, string? status)
        {
            var query = _db.WeaponTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Code != null && x.Code.Contains(search)) ||
                    (x.Description != null && x.Description.Contains(search)));
            }

            if (status == "active")
            {
                query = query.Where(x => x.IsActive);
            }
            else if (status == "inactive")
            {
                query = query.Where(x => !x.IsActive);
            }

            ViewBag.Search = search;
            ViewBag.Status = status;

            var weaponTypes = await query
                .OrderBy(x => x.Name)
                .ToListAsync();

            return View(weaponTypes);
        }

        public IActionResult Create()
        {
            return View(new WeaponType());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WeaponType model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedAt = DateTime.UtcNow;

            _db.WeaponTypes.Add(model);
            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "CREATE_WEAPON_TYPE",
                entityName: "WeaponType",
                entityId: model.Id.ToString(),
                newValues: new
                {
                    model.Id,
                    model.Name,
                    model.Code,
                    model.Description,
                    model.IsActive
                });

            TempData["SuccessMessage"] = "Weapon type successfully created.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.WeaponTypes.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WeaponType model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var item = await _db.WeaponTypes.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            var oldValues = new
            {
                item.Name,
                item.Code,
                item.Description,
                item.IsActive
            };

            item.Name = model.Name;
            item.Code = model.Code;
            item.Description = model.Description;
            item.IsActive = model.IsActive;

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "EDIT_WEAPON_TYPE",
                entityName: "WeaponType",
                entityId: item.Id.ToString(),
                oldValues: oldValues,
                newValues: new
                {
                    item.Name,
                    item.Code,
                    item.Description,
                    item.IsActive
                });

            TempData["SuccessMessage"] = "Weapon type successfully updated.";

            return RedirectToAction(nameof(Index));
        }
    }
}