using AMS_data;
using AMS_data.Entities.Weapons;
using AMS_services.Audit;
using Asset_management_Web_Core.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Asset_management_Web_Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WeaponModelsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly AuditLogService _auditLogService;
        private readonly IWebHostEnvironment _env;

        public WeaponModelsController(
            ApplicationDbContext db,
            AuditLogService auditLogService,
            IWebHostEnvironment env)
        {
            _db = db;
            _auditLogService = auditLogService;
            _env = env;
        }

        public async Task<IActionResult> Index(string? search, string? status)
        {
            var query = _db.WeaponModels
                .Include(x => x.WeaponType)
                .Include(x => x.Manufacturer)
                .Include(x => x.Caliber)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Code != null && x.Code.Contains(search)) ||
                    (x.Description != null && x.Description.Contains(search)) ||
                    (x.WeaponType != null && x.WeaponType.Name.Contains(search)) ||
                    (x.Manufacturer != null && x.Manufacturer.Name.Contains(search)) ||
                    (x.Caliber != null && x.Caliber.Name.Contains(search)));
            }

            if (status == "active")
                query = query.Where(x => x.IsActive);
            else if (status == "inactive")
                query = query.Where(x => !x.IsActive);

            ViewBag.Search = search;
            ViewBag.Status = status;

            var items = await query
                .OrderBy(x => x.Name)
                .ToListAsync();

            return View(items);
        }

        public async Task<IActionResult> Create()
        {
            var model = await BuildModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WeaponModelFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await BuildModel(model);
                return View(model);
            }

            var imagePath = await SaveImageAsync(model.ImageFile);

            var entity = new WeaponModel
            {
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
                WeaponTypeId = model.WeaponTypeId,
                ManufacturerId = model.ManufacturerId,
                CaliberId = model.CaliberId,
                ImagePath = imagePath,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _db.WeaponModels.Add(entity);
            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "CREATE_WEAPON_MODEL",
                "WeaponModel",
                entity.Id.ToString(),
                newValues: new
                {
                    entity.Name,
                    entity.Code,
                    entity.Description,
                    entity.WeaponTypeId,
                    entity.ManufacturerId,
                    entity.CaliberId,
                    entity.ImagePath,
                    entity.IsActive
                });

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.WeaponModels.FindAsync(id);

            if (entity == null)
                return NotFound();

            var model = await BuildModel(new WeaponModelFormViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                WeaponTypeId = entity.WeaponTypeId,
                ManufacturerId = entity.ManufacturerId,
                CaliberId = entity.CaliberId,
                ExistingImagePath = entity.ImagePath,
                IsActive = entity.IsActive
            });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WeaponModelFormViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                model = await BuildModel(model);
                return View(model);
            }

            var entity = await _db.WeaponModels.FindAsync(id);

            if (entity == null)
                return NotFound();

            var oldValues = new
            {
                entity.Name,
                entity.Code,
                entity.Description,
                entity.WeaponTypeId,
                entity.ManufacturerId,
                entity.CaliberId,
                entity.ImagePath,
                entity.IsActive
            };

            var newImagePath = await SaveImageAsync(model.ImageFile);

            entity.Name = model.Name;
            entity.Code = model.Code;
            entity.Description = model.Description;
            entity.WeaponTypeId = model.WeaponTypeId;
            entity.ManufacturerId = model.ManufacturerId;
            entity.CaliberId = model.CaliberId;
            entity.IsActive = model.IsActive;

            if (!string.IsNullOrWhiteSpace(newImagePath))
            {
                entity.ImagePath = newImagePath;
            }

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "EDIT_WEAPON_MODEL",
                "WeaponModel",
                entity.Id.ToString(),
                oldValues,
                new
                {
                    entity.Name,
                    entity.Code,
                    entity.Description,
                    entity.WeaponTypeId,
                    entity.ManufacturerId,
                    entity.CaliberId,
                    entity.ImagePath,
                    entity.IsActive
                });

            return RedirectToAction(nameof(Index));
        }

        private async Task<WeaponModelFormViewModel> BuildModel(WeaponModelFormViewModel? model = null)
        {
            model ??= new WeaponModelFormViewModel();

            model.WeaponTypes = await _db.WeaponTypes
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            model.Manufacturers = await _db.Manufacturers
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            model.Calibers = await _db.Calibers
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            return model;
        }

        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".svg" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid image file type.");

            var folder = Path.Combine(_env.WebRootPath, "uploads", "weapon-models");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/weapon-models/{fileName}";
        }
    }
}