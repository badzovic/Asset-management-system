using AMS_data;
using AMS_data.Entities.Lookups;
using AMS_services.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asset_management_Web_Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LookupsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly AuditLogService _auditLogService;

        public LookupsController(ApplicationDbContext db, AuditLogService auditLogService)
        {
            _db = db;
            _auditLogService = auditLogService;
        }

        public async Task<IActionResult> Items(string categoryKey, string? search, string? status)
        {
            if (string.IsNullOrWhiteSpace(categoryKey))
                return BadRequest();

            var category = await _db.LookupCategories
                .FirstOrDefaultAsync(x => x.Key == categoryKey);

            if (category == null)
                return NotFound();

            var query = _db.LookupItems
                .Where(x => x.LookupCategoryId == category.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Code != null && x.Code.Contains(search)) ||
                    (x.Description != null && x.Description.Contains(search)));
            }

            if (status == "active")
                query = query.Where(x => x.IsActive);
            else if (status == "inactive")
                query = query.Where(x => !x.IsActive);

            ViewBag.Category = category;
            ViewBag.Search = search;
            ViewBag.Status = status;

            var items = await query
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return View(items);
        }

        public async Task<IActionResult> Create(string categoryKey)
        {
            var category = await _db.LookupCategories
                .FirstOrDefaultAsync(x => x.Key == categoryKey);

            if (category == null)
                return NotFound();

            ViewBag.Category = category;

            return View(new LookupItem
            {
                LookupCategoryId = category.Id,
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LookupItem model)
        {
            var category = await _db.LookupCategories.FindAsync(model.LookupCategoryId);

            if (category == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Category = category;
                return View(model);
            }

            model.CreatedAt = DateTime.UtcNow;

            _db.LookupItems.Add(model);
            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "CREATE_LOOKUP_ITEM",
                entityName: "LookupItem",
                entityId: model.Id.ToString(),
                newValues: new
                {
                    Category = category.Key,
                    model.Name,
                    model.Code,
                    model.Description,
                    model.DisplayOrder,
                    model.IsActive
                });

            return RedirectToAction(nameof(Items), new { categoryKey = category.Key });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.LookupItems
                .Include(x => x.LookupCategory)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            ViewBag.Category = item.LookupCategory;

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LookupItem model)
        {
            if (id != model.Id)
                return BadRequest();

            var item = await _db.LookupItems
                .Include(x => x.LookupCategory)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Category = item.LookupCategory;
                return View(model);
            }

            var oldValues = new
            {
                item.Name,
                item.Code,
                item.Description,
                item.DisplayOrder,
                item.IsActive
            };

            item.Name = model.Name;
            item.Code = model.Code;
            item.Description = model.Description;
            item.DisplayOrder = model.DisplayOrder;
            item.IsActive = model.IsActive;

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "EDIT_LOOKUP_ITEM",
                entityName: "LookupItem",
                entityId: item.Id.ToString(),
                oldValues: oldValues,
                newValues: new
                {
                    Category = item.LookupCategory.Key,
                    item.Name,
                    item.Code,
                    item.Description,
                    item.DisplayOrder,
                    item.IsActive
                });

            return RedirectToAction(nameof(Items), new { categoryKey = item.LookupCategory.Key });
        }
    }
}