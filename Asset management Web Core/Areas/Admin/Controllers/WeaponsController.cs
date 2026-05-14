using AMS_data;
using AMS_data.Entities.Weapons;
using AMS_services.Audit;
using Asset_management_Web_Core.Areas.Admin.ViewModels.Weapons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Asset_management_Web_Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Operator")]
    public class WeaponsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly AuditLogService _auditLogService;
        private readonly IWebHostEnvironment _env;

        public WeaponsController(
            ApplicationDbContext db,
            AuditLogService auditLogService,
            IWebHostEnvironment env)
        {
            _db = db;
            _auditLogService = auditLogService;
            _env = env;
        }

        public async Task<IActionResult> Register()
        {
            var model = new WeaponRegisterViewModel
            {
                RegistrationNo = await GenerateRegistrationNo(),
                RegistrationDate = DateTime.Today,
                IsMarked = false
            };

            model = await PopulateDropdowns(model);

            ViewBag.ActiveWeapons = await GetActiveWeapons();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(WeaponRegisterViewModel model)
        {
            if (model.FactorySerial != model.ConfirmSerial)
            {
                ModelState.AddModelError(nameof(model.ConfirmSerial), "Serial numbers do not match.");
            }

            if (!ModelState.IsValid)
            {
                model = await PopulateDropdowns(model);
                ViewBag.ActiveWeapons = await GetActiveWeapons();
                return View(model);
            }

            var imagePath = await SaveImageAsync(model.ImageFile);

            var weapon = new Weapon
            {
                RegistrationNo = model.RegistrationNo,
                RegistrationDate = model.RegistrationDate,

                IsMarked = model.IsMarked,
                IsProspective = model.IsProspective,

                FactorySerial = model.FactorySerial,
                ConfirmSerial = model.ConfirmSerial,

                WeaponTypeId = model.WeaponTypeId,
                WeaponModelId = model.WeaponModelId,
                CaliberId = model.CaliberId,
                ManufacturerId = model.ManufacturerId,

                MarkLocationLookupId = model.MarkLocationLookupId,
                CountryLookupId = model.CountryLookupId,
                RegionLookupId = model.RegionLookupId,
                GovernmentAgencyLookupId = model.GovernmentAgencyLookupId,
                ManufactureCountryLookupId = model.ManufactureCountryLookupId,

                ManufactureDate = model.ManufactureDate,

                OriginalLocationLookupId = model.OriginalLocationLookupId,
                OriginIndicatorLookupId = model.OriginIndicatorLookupId,
                OriginalStateLookupId = model.OriginalStateLookupId,

                UnitLookupId = model.UnitLookupId,
                StockLookupId = model.StockLookupId,
                BookkeepingByLookupId = model.BookkeepingByLookupId,

                BarrelMark = model.BarrelMark,
                SlideMark = model.SlideMark,
                ButtstockMark = model.ButtstockMark,

                IdTypeLookupId = model.IdTypeLookupId,
                IdNo = model.IdNo,
                HolderInfo = model.HolderInfo,
                DateOfOwnership = model.DateOfOwnership,

                Notes = model.Notes,
                InventoryNo = model.InventoryNo,

                TempStock = model.TempStock,
                DonationDate = model.DonationDate,
                DonorAgencyLookupId = model.DonorAgencyLookupId,
                DonorContractNo = model.DonorContractNo,

                CurrentStatusId = model.CurrentStatusId,
                WeaponStateLookupId = model.WeaponStateLookupId,

                ImagePath = imagePath,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _db.Weapons.Add(weapon);
            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "REGISTER_WEAPON",
                entityName: "Weapon",
                entityId: weapon.Id.ToString(),
                newValues: new
                {
                    weapon.RegistrationNo,
                    weapon.FactorySerial,
                    weapon.WeaponTypeId,
                    weapon.WeaponModelId,
                    weapon.ManufacturerId,
                    weapon.CaliberId,
                    weapon.CurrentStatusId
                });

            return RedirectToAction(nameof(Register));
        }


        [HttpGet]
        public async Task<IActionResult> GetModelDetails(int id)
        {
            var model = await _db.WeaponModels
              .Include(x => x.WeaponType)
              .Include(x => x.Manufacturer)
                  .ThenInclude(x => x.CountryLookup)
              .Include(x => x.Caliber)
              .FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
                return NotFound();

            int? manufacturerCountryLookupId = null;

            if (!string.IsNullOrWhiteSpace(model.Manufacturer?.Country))
            {
                manufacturerCountryLookupId = await _db.LookupItems
                    .Include(x => x.LookupCategory)
                    .Where(x =>
                        x.IsActive &&
                        x.LookupCategory.Key == "ManufacturerCountry" &&
                        x.Name == model.Manufacturer.Country)
                    .Select(x => (int?)x.Id)
                    .FirstOrDefaultAsync();
            }

            return Json(new
            {
                weaponTypeId = model.WeaponTypeId,
                weaponTypeName = model.WeaponType?.Name,

                manufacturerId = model.ManufacturerId,
                manufacturerName = model.Manufacturer?.Name,

                manufacturerCountryLookupId = model.Manufacturer?.CountryLookupId,
                manufacturerCountry = model.Manufacturer?.CountryLookup?.Name,

                caliberId = model.CaliberId,
                caliberName = model.Caliber?.Name,

                imagePath = model.ImagePath
            });
        }

        private async Task<List<Weapon>> GetActiveWeapons()
        {
            return await _db.Weapons
                .Include(x => x.WeaponType)
                .Include(x => x.WeaponModel)
                .Include(x => x.Caliber)
                .Include(x => x.Manufacturer)
                .Include(x => x.CurrentStatus)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Take(100)
                .ToListAsync();
        }

        private async Task<WeaponRegisterViewModel> PopulateDropdowns(WeaponRegisterViewModel model)
        {
            model.WeaponTypes = await SelectWeaponTypes();
            model.WeaponModels = await SelectWeaponModels();
            model.Manufacturers = await SelectManufacturers();
            model.Calibers = await SelectCalibers();
            model.WeaponStatuses = await SelectWeaponStatuses();

            model.MarkLocations = await SelectLookup("MarkLocation");
            model.Countries = await SelectLookup("Country");
            model.Regions = await SelectLookup("Region");
            model.GovernmentAgencies = await SelectLookup("GovernmentAgency");
            model.ManufactureCountries = await SelectLookup("ManufacturerCountry");
            model.OriginalLocations = await SelectLookup("OriginalLocation");
            model.OriginIndicators = await SelectLookup("OriginIndicator");
            model.OriginalStates = await SelectLookup("OriginalState");
            model.Units = await SelectLookup("Unit");
            model.Stocks = await SelectLookup("Stock");
            model.BookkeepingByList = await SelectLookup("BookkeepingBy");
            model.IdTypes = await SelectLookup("IdType");
            model.DonorAgencies = await SelectLookup("GovernmentAgency");
            model.WeaponStates = await SelectLookup("WeaponState");

            return model;
        }

        private async Task<List<SelectListItem>> SelectLookup(string categoryKey)
        {
            return await _db.LookupItems
                .Include(x => x.LookupCategory)
                .Where(x => x.LookupCategory.Key == categoryKey && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> SelectWeaponTypes()
        {
            return await _db.WeaponTypes
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> SelectWeaponModels()
        {
            return await _db.WeaponModels
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> SelectManufacturers()
        {
            return await _db.Manufacturers
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> SelectCalibers()
        {
            return await _db.Calibers
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> SelectWeaponStatuses()
        {
            return await _db.WeaponStatuses
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        private async Task<string> GenerateRegistrationNo()
        {
            var count = await _db.Weapons.CountAsync() + 1;
            return $"FUP5X{count:000000}";
        }

        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid image file type.");

            var folder = Path.Combine(_env.WebRootPath, "uploads", "weapons");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/weapons/{fileName}";
        }
    }
}