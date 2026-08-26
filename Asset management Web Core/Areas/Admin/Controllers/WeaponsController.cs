using AMS_data;
using AMS_data.Entities.Weapons;
using AMS_services.Audit;
using Asset_management_Web_Core.Areas.Admin.ViewModels.Weapons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


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
            model.CountryLookupId = 1;
            model.RegionLookupId = 7;
            model.GovernmentAgencyLookupId = 13;
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
                TeamLookupId = model.TeamLookupId,
                StockLookupId = model.StockLookupId,
                BookkeepingByLookupId = model.BookkeepingByLookupId,

                BarrelMark = model.BarrelMark,
                SlideMark = model.SlideMark,
                ButtstockMark = model.ButtstockMark,

                IdTypeLookupId = model.IdTypeLookupId,
                IdTypeOtherText = model.IdTypeOtherText,
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

            TempData["SuccessMessage"] = "Weapon successfully registered.";
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
        public async Task<IActionResult> Check(int? weaponId, string? searchTerm)
        {
            var model = new WeaponCheckViewModel
            {
                SelectedWeaponId = weaponId,
                SearchTerm = searchTerm,
                CheckDate = DateTime.Today
            };

            model = await PopulateCheckModel(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Check(WeaponCheckViewModel model)
        {
            if (model.SelectedWeaponId == null)
            {
                ModelState.AddModelError(nameof(model.SelectedWeaponId), "Please select a weapon.");
            }

            if (!ModelState.IsValid)
            {
                model = await PopulateCheckModel(model);
                return View(model);
            }

            var weapon = await _db.Weapons
                .FirstOrDefaultAsync(x => x.Id == model.SelectedWeaponId && !x.IsDeleted);

            if (weapon == null)
                return NotFound();

            var check = new WeaponCheck
            {
                WeaponId = weapon.Id,
                CheckDate = model.CheckDate,
                CheckStateLookupId = model.CheckStateLookupId,
                IdNo = model.IdNo,
                Comments = model.Comments,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _db.WeaponChecks.Add(check);
            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "CREATE_WEAPON_CHECK",
                entityName: "WeaponCheck",
                entityId: check.Id.ToString(),
                newValues: new
                {
                    check.WeaponId,
                    weapon.RegistrationNo,
                    weapon.FactorySerial,
                    check.CheckDate,
                    check.CheckStateLookupId,
                    check.IdNo,
                    check.Comments
                });

            return RedirectToAction(nameof(Check), new { weaponId = weapon.Id });
        }

        public async Task<IActionResult> PrepareMove(int? weaponId, string? searchTerm)
        {
            var model = new WeaponMoveViewModel
            {
                SelectedWeaponId = weaponId,
                SearchTerm = searchTerm,
                MoveDate = DateTime.Today,
                MoveOrdinalNo = "001"
            };

            model = await PopulateMoveModel(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PrepareMove(WeaponMoveViewModel model)
        {
            if (model.SelectedWeaponId == null)
            {
                ModelState.AddModelError(nameof(model.SelectedWeaponId), "Please select a weapon.");
            }

            if (!ModelState.IsValid)
            {
                model = await PopulateMoveModel(model);
                return View(model);
            }

            var weapon = await _db.Weapons
                .FirstOrDefaultAsync(x => x.Id == model.SelectedWeaponId && !x.IsDeleted);

            if (weapon == null)
                return NotFound();

            var move = new WeaponMove
            {
                WeaponId = weapon.Id,
                MoveDate = model.MoveDate,
                MovementActionLookupId = model.MovementActionLookupId,
                NewLocationLookupId = model.NewLocationLookupId,
                OrderNo = model.OrderNo,
                AuthMoveNo = model.AuthMoveNo,
                MoveOrdinalNo = model.MoveOrdinalNo,
                EndUserCertificate = model.EndUserCertificate,
                UserOrgName = model.UserOrgName,
                Notes = model.Notes,
                AuthorisedByName = model.AuthorisedByName,
                Status = "Prepared",
                PreparedAt = DateTime.UtcNow,
                PreparedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _db.WeaponMoves.Add(move);
            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "PREPARE_WEAPON_MOVE",
                entityName: "WeaponMove",
                entityId: move.Id.ToString(),
                newValues: new
                {
                    move.WeaponId,
                    weapon.RegistrationNo,
                    weapon.FactorySerial,
                    move.MoveDate,
                    move.MovementActionLookupId,
                    move.NewLocationLookupId,
                    move.OrderNo,
                    move.AuthMoveNo,
                    move.MoveOrdinalNo,
                    move.Status
                });

            TempData["SuccessMessage"] = "Prijenos oružja je uspješno pripremljen.";

            return RedirectToAction(nameof(PrepareMove), new { weaponId = weapon.Id });
        }

        public async Task<IActionResult> AuthoriseMove(int? moveId)
        {
            var model = new WeaponMoveAuthoriseViewModel
            {
                SelectedMoveId = moveId
            };

            model = await PopulateAuthoriseMoveModel(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AuthoriseMove(WeaponMoveAuthoriseViewModel model)
        {
            if (model.SelectedMoveId == null)
            {
                ModelState.AddModelError(nameof(model.SelectedMoveId), "Please select a weapon move.");
            }

            if (!ModelState.IsValid)
            {
                model = await PopulateAuthoriseMoveModel(model);
                return View(model);
            }

            var move = await _db.WeaponMoves
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x =>
                    x.Id == model.SelectedMoveId &&
                    !x.IsDeleted &&
                    x.Status == "Prepared");

            if (move == null)
                return NotFound();

            move.AuthorisedByName = model.AuthorisedByName;
            move.AuthorisedAt = DateTime.UtcNow;
            move.AuthorisedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            move.Status = "Authorised";

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "AUTHORISE_WEAPON_MOVE",
                entityName: "WeaponMove",
                entityId: move.Id.ToString(),
                newValues: new
                {
                    move.WeaponId,
                    move.Weapon?.RegistrationNo,
                    move.Weapon?.FactorySerial,
                    move.AuthorisedByName,
                    move.AuthorisedAt,
                    move.Status
                });

            TempData["SuccessMessage"] = "Prijenos oružja je uspješno autorizovan.";

            return RedirectToAction(nameof(AuthoriseMove));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReopenMove(int id)
        {
            var move = await _db.WeaponMoves
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                    x.Status == "Authorised");

            if (move == null)
                return NotFound();

            move.Status = "Prepared";
            move.AuthorisedByName = null;
            move.AuthorisedAt = null;
            move.AuthorisedByUserId = null;

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "REOPEN_WEAPON_MOVE",
                entityName: "WeaponMove",
                entityId: move.Id.ToString(),
                newValues: new
                {
                    move.Id,
                    move.WeaponId,
                    move.Status
                });

            TempData["SuccessMessage"] = "Prijenos oružja je vraćen na pripremu.";

            return RedirectToAction(nameof(AuthoriseMove), new { moveId = move.Id });
        }
        public async Task<IActionResult> ExportQueryCsv(WeaponQueryViewModel model)
        {
            var results = await GetWeaponQueryResults(model);

            var sb = new StringBuilder();

            sb.AppendLine("Registration No,Factory Serial,Model,Type,Manufacturer,Caliber,Registration Date,Marked,Original Location");

            foreach (var item in results)
            {
                sb.AppendLine(string.Join(",",
                    Csv(item.RegistrationNo),
                    Csv(item.FactorySerial),
                    Csv(item.WeaponModel?.Name),
                    Csv(item.WeaponType?.Name),
                    Csv(item.Manufacturer?.Name),
                    Csv(item.Caliber?.Name),
                    Csv(item.RegistrationDate.ToString("dd.MM.yyyy")),
                    Csv(item.IsMarked ? "Yes" : "No"),
                    Csv(item.OriginalLocationLookup?.Name)
                ));
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            return File(bytes, "text/csv", $"weapon-query-{DateTime.Now:yyyyMMdd-HHmm}.csv");
        }
        public async Task<IActionResult> ExportQueryPdf(WeaponQueryViewModel model)
        {
            model = await PopulateQueryModel(model);
            model.Results = await GetWeaponQueryResults(model);
            model.HasSearched = true;

            return View("QueryPdf", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveWeaponQuery(WeaponQueryViewModel model, string queryName, bool isPublic = false)
        {
            if (string.IsNullOrWhiteSpace(queryName))
            {
                TempData["SuccessMessage"] = "Naziv upita je obavezan.";
                return RedirectToAction(nameof(QueryGenerator));
            }

            var queryJson = JsonSerializer.Serialize(model);

            var saved = new SavedWeaponQuery
            {
                Name = queryName,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                IsPublic = isPublic,
                QueryJson = queryJson,
                CreatedAt = DateTime.UtcNow
            };

            _db.SavedWeaponQueries.Add(saved);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Query je uspješno sačuvan.";

            return RedirectToAction(nameof(QueryGenerator));
        }
        private static string Csv(string? value)
        {
            value ??= "";
            value = value.Replace("\"", "\"\"");
            return $"\"{value}\"";
        }
        public async Task<IActionResult> QueryGenerator(WeaponQueryViewModel model)
        {
            model = await PopulateQueryModel(model);

            var hasAnyFilter =
                !string.IsNullOrWhiteSpace(model.RegistrationNo) ||
                !string.IsNullOrWhiteSpace(model.FactorySerial) ||
                model.WeaponTypeId != null ||
                model.WeaponModelId != null ||
                model.ManufacturerId != null ||
                model.CaliberId != null ||
                model.CountryLookupId != null ||
                model.RegionLookupId != null ||
                model.OriginalLocationLookupId != null ||
                model.OriginIndicatorLookupId != null ||
                model.StockLookupId != null ||
                model.UnitLookupId != null ||
                model.IsMarked != null ||
                model.IsProspective != null ||
                model.RegistrationDateFrom != null ||
                model.RegistrationDateTo != null;

            if (!hasAnyFilter)
                return View(model);

            var query = _db.Weapons
                .Include(x => x.WeaponType)
                .Include(x => x.WeaponModel)
                .Include(x => x.Manufacturer)
                .Include(x => x.Caliber)
                .Include(x => x.CountryLookup)
                .Include(x => x.RegionLookup)
                .Include(x => x.OriginalLocationLookup)
                .Include(x => x.OriginIndicatorLookup)
                .Include(x => x.StockLookup)
                .Include(x => x.UnitLookup)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.RegistrationNo))
                query = query.Where(x => x.RegistrationNo.Contains(model.RegistrationNo));

            if (!string.IsNullOrWhiteSpace(model.FactorySerial))
                query = query.Where(x => x.FactorySerial != null && x.FactorySerial.Contains(model.FactorySerial));

            if (model.WeaponTypeId != null)
                query = query.Where(x => x.WeaponTypeId == model.WeaponTypeId);

            if (model.WeaponModelId != null)
                query = query.Where(x => x.WeaponModelId == model.WeaponModelId);

            if (model.ManufacturerId != null)
                query = query.Where(x => x.ManufacturerId == model.ManufacturerId);

            if (model.CaliberId != null)
                query = query.Where(x => x.CaliberId == model.CaliberId);

            if (model.CountryLookupId != null)
                query = query.Where(x => x.CountryLookupId == model.CountryLookupId);

            if (model.RegionLookupId != null)
                query = query.Where(x => x.RegionLookupId == model.RegionLookupId);

            if (model.OriginalLocationLookupId != null)
                query = query.Where(x => x.OriginalLocationLookupId == model.OriginalLocationLookupId);

            if (model.OriginIndicatorLookupId != null)
                query = query.Where(x => x.OriginIndicatorLookupId == model.OriginIndicatorLookupId);

            if (model.StockLookupId != null)
                query = query.Where(x => x.StockLookupId == model.StockLookupId);

            if (model.UnitLookupId != null)
                query = query.Where(x => x.UnitLookupId == model.UnitLookupId);

            if (model.IsMarked != null)
                query = query.Where(x => x.IsMarked == model.IsMarked);

            if (model.IsProspective != null)
                query = query.Where(x => x.IsProspective == model.IsProspective);

            if (model.RegistrationDateFrom != null)
                query = query.Where(x => x.RegistrationDate.Date >= model.RegistrationDateFrom.Value.Date);

            if (model.RegistrationDateTo != null)
                query = query.Where(x => x.RegistrationDate.Date <= model.RegistrationDateTo.Value.Date);

            model.Results = await query
                .OrderByDescending(x => x.RegistrationDate)
                .Take(500)
                .ToListAsync();

            model.HasSearched = true;

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            model.SavedQueries = await _db.SavedWeaponQueries
                .Where(x =>
                    x.UserId == currentUserId ||
                    x.IsPublic)
                .OrderByDescending(x => x.CreatedAt)
                .Take(20)
                .ToListAsync();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSavedQuery(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = await _db.SavedWeaponQueries
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    (x.UserId == currentUserId || User.IsInRole("Admin")));

            if (query == null)
                return NotFound();

            _db.SavedWeaponQueries.Remove(query);

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Saved query deleted.";

            return RedirectToAction(nameof(QueryGenerator));
        }
        public async Task<IActionResult> OpenSavedQuery(int id)
        {
            var saved = await _db.SavedWeaponQueries
                .FirstOrDefaultAsync(x => x.Id == id);

            if (saved == null)
                return NotFound();

            var model = JsonSerializer.Deserialize<WeaponQueryViewModel>(saved.QueryJson);

            if (model == null)
                return RedirectToAction(nameof(QueryGenerator));

            model = await PopulateQueryModel(model);

            model.Results = await GetWeaponQueryResults(model);
            model.HasSearched = true;

            return View("QueryGenerator", model);
        }

        public async Task<IActionResult> Mark(int? weaponId)
        {
            var model = new WeaponMarkingViewModel
            {
                SelectedWeaponId = weaponId
            };

            model = await PopulateMarkingModel(model);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetMarkingJobStatus(int jobId)
        {
            var job = await _db.LaserJobs
                .FirstOrDefaultAsync(x => x.Id == jobId);

            if (job == null)
            {
                return NotFound();
            }

            return Json(new
            {
                status = job.Status
            });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Mark(WeaponMarkingViewModel model)
        //{
        //    if (model.SelectedWeaponId == null)
        //        ModelState.AddModelError(nameof(model.SelectedWeaponId), "Please select a weapon.");

        //    if (!ModelState.IsValid)
        //    {
        //        model = await PopulateMarkingModel(model);
        //        return View(model);
        //    }

        //    var weapon = await _db.Weapons
        //        .Include(x => x.WeaponModel)
        //        .Include(x => x.WeaponType)
        //        .Include(x => x.Manufacturer)
        //        .Include(x => x.Caliber)
        //        .FirstOrDefaultAsync(x => x.Id == model.SelectedWeaponId && !x.IsDeleted);

        //    if (weapon == null)
        //        return NotFound();

        //    var job = new WeaponMarkingJob
        //    {
        //        WeaponId = weapon.Id,
        //        MarkingLayoutId = model.MarkingLayoutId,
        //        JobDate = DateTime.UtcNow,
        //        Status = "Prepared",

        //        RegistrationNo = weapon.RegistrationNo,
        //        FactorySerial = weapon.FactorySerial,
        //        WeaponModel = weapon.WeaponModel?.Name,
        //        WeaponType = weapon.WeaponType?.Name,
        //        Manufacturer = weapon.Manufacturer?.Name,
        //        Caliber = weapon.Caliber?.Name,

        //        MarkingText1 = model.MarkingText1,
        //        MarkingText2 = model.MarkingText2,
        //        MarkingText3 = model.MarkingText3,
        //        DataMatrixValue = model.DataMatrixValue,
        //        QrValue = model.QrValue,

        //        CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    _db.WeaponMarkingJobs.Add(job);
        //    await _db.SaveChangesAsync();

        //    TempData["SuccessMessage"] = "Marking job je uspješno pripremljen.";

        //    return RedirectToAction(nameof(Mark), new { weaponId = weapon.Id });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Mark(WeaponMarkingViewModel model)
        {
            if (!model.SelectedWeaponId.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Oružje nije odabrano.");
                model = await PopulateMarkingModel(model);
                return View(model);
            }

            var weapon = await  _db.Weapons
                .FirstOrDefaultAsync(x => x.Id == model.SelectedWeaponId.Value && !x.IsDeleted);

            if (weapon == null)
                return NotFound();

            var activeJobExists = await _db.LaserJobs.AnyAsync(x =>
             x.LayoutCode == "REGISTER_WEAPON" &&
             (x.Status == "READY" || x.Status == "PROCESSING"));

            if (activeJobExists)
            {
                ModelState.AddModelError(string.Empty, "Već postoji aktivan posao markiranja. Završite postojeći posao prije slanja novog.");
                model = await PopulateMarkingModel(model);
                return View(model);
            }

            var laserJob = new LaserJob
            {
                WeaponId = weapon.Id,
                LayoutCode = "REGISTER_WEAPON",
                RegistrationNo = weapon.RegistrationNo,
                FactorySerial = weapon.FactorySerial,
                Status = "READY",
                CreatedOn = DateTime.Now
            };

             _db.LaserJobs.Add(laserJob);
            await  _db.SaveChangesAsync();
            TempData["DeviceAgentJobId"] = laserJob.Id;
            TempData["SuccessMessage"] = weapon.IsMarked
            ? "Oružje je već ranije bilo markirano. Novi posao markiranja je poslan u MarkMaster."
            : "Posao markiranja je poslan u MarkMaster.";

            return RedirectToAction(nameof(Mark), new { weaponId = weapon.Id });
        }

        public async Task<IActionResult> MarkingLayouts()
        {
            var layouts = await _db.MarkingLayouts
                .OrderBy(x => x.Name)
                .ToListAsync();

            return View(layouts);
        }

        public IActionResult CreateMarkingLayout()
        {
            return View(new MarkingLayoutFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMarkingLayout(MarkingLayoutFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var templatePath = await SaveMarkingLayoutFileAsync(model.TemplateFile, "templates");
            var backgroundPath = await SaveMarkingLayoutFileAsync(model.BackgroundFile, "backgrounds");
            var previewPath = await SaveMarkingLayoutFileAsync(model.PreviewImage, "previews");

            var layout = new MarkingLayout
            {
                Name = model.Name,
                LayoutType = model.LayoutType,
                Description = model.Description,
                WidthMm = model.WidthMm,
                HeightMm = model.HeightMm,
                Unit = model.Unit,
                TemplateFilePath = templatePath,
                BackgroundFilePath = backgroundPath,
                PreviewImagePath = previewPath,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _db.MarkingLayouts.Add(layout);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Layout uspješno kreiran.";

            return RedirectToAction(nameof(MarkingLayouts));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMarkingJob(int id)
        {
            var job = await _db.WeaponMarkingJobs
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (job == null)
                return NotFound();

            job.Status = "SentToMarker";
            job.SentAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Marking job je poslan na marker.";

            return RedirectToAction(nameof(Mark), new { weaponId = job.WeaponId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteMarkingJob(int id)
        {
            var job = await _db.WeaponMarkingJobs
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (job == null)
                return NotFound();

            job.Status = "Completed";
            job.CompletedAt = DateTime.UtcNow;

            job.Weapon.IsMarked = true;
            job.Weapon.UpdatedAt = DateTime.UtcNow;
            job.Weapon.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Markiranje je označeno kao završeno.";

            return RedirectToAction(nameof(Mark), new { weaponId = job.WeaponId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FailMarkingJob(int id)
        {
            var job = await _db.WeaponMarkingJobs
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (job == null)
                return NotFound();

            job.Status = "Failed";
            job.ErrorMessage = "Marked as failed by operator.";

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Marking job je označen kao neuspješan.";

            return RedirectToAction(nameof(Mark), new { weaponId = job.WeaponId });
        }

        public async Task<IActionResult> MarkingQueue(string? status = null, string? search = null)
        {
            var query = _db.WeaponMarkingJobs
                .Include(x => x.Weapon)
                    .ThenInclude(x => x.WeaponModel)
                .Include(x => x.MarkingLayout)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    (x.RegistrationNo != null && x.RegistrationNo.Contains(search)) ||
                    (x.FactorySerial != null && x.FactorySerial.Contains(search)) ||
                    (x.WeaponModel != null && x.WeaponModel.Contains(search)) ||
                    (x.MarkingLayout != null && x.MarkingLayout.Name.Contains(search)));
            }

            ViewBag.Status = status;
            ViewBag.Search = search;

            var jobs = await query
                .OrderByDescending(x => x.CreatedAt)
                .Take(200)
                .ToListAsync();

            return View(jobs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RetryMarkingJob(int id)
        {
            var job = await _db.WeaponMarkingJobs
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (job == null)
                return NotFound();

            job.Status = "Prepared";
            job.ErrorMessage = null;
            job.SentAt = null;
            job.CompletedAt = null;

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Marking job je vraćen na pripremu.";

            return RedirectToAction(nameof(MarkingQueue));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelMarkingJob(int id)
        {
            var job = await _db.WeaponMarkingJobs
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (job == null)
                return NotFound();

            job.Status = "Cancelled";

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Marking job je otkazan.";

            return RedirectToAction(nameof(MarkingQueue));
        }
        public async Task<IActionResult> LayoutEditor(int id)
        {
            var layout = await _db.MarkingLayouts
                .Include(x => x.Objects)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (layout == null)
                return NotFound();

            return View(layout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveLayoutEditor(int id, string layoutJson)
        {
            var layout = await _db.MarkingLayouts
                .Include(x => x.Objects)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (layout == null)
                return NotFound();

            layout.LayoutJson = layoutJson;

            _db.MarkingLayoutObjects.RemoveRange(layout.Objects);

            if (!string.IsNullOrWhiteSpace(layoutJson))
            {
                var objects = System.Text.Json.JsonSerializer.Deserialize<List<MarkingLayoutObject>>(layoutJson);

                if (objects != null)
                {
                    var order = 1;

                    foreach (var obj in objects)
                    {
                        obj.Id = 0;
                        obj.MarkingLayoutId = layout.Id;
                        obj.DisplayOrder = order++;
                        obj.IsActive = true;

                        _db.MarkingLayoutObjects.Add(obj);
                    }
                }
            }

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Layout editor uspješno sačuvan.";

            return RedirectToAction(nameof(LayoutEditor), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetLayoutPreview(int id)
        {
            var layout = await _db.MarkingLayouts
                .Include(x => x.Objects)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (layout == null)
                return NotFound();

            var objects = new List<MarkingLayoutObject>();

            if (!string.IsNullOrWhiteSpace(layout.LayoutJson))
            {
                objects = System.Text.Json.JsonSerializer.Deserialize<List<MarkingLayoutObject>>(
                    layout.LayoutJson,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                ) ?? new List<MarkingLayoutObject>();
            }
            else
            {
                objects = layout.Objects
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.DisplayOrder)
                    .ToList();
            }

            return Json(new
            {
                id = layout.Id,
                name = layout.Name,
                widthMm = layout.WidthMm > 0 ? layout.WidthMm : 100,
                heightMm = layout.HeightMm > 0 ? layout.HeightMm : 80,
                backgroundFilePath = layout.BackgroundFilePath,
                objects = objects
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new
                    {
                        objectType = x.ObjectType,
                        name = x.Name,
                        x = x.X,
                        y = x.Y,
                        width = x.Width,
                        height = x.Height,
                        textValue = x.TextValue,
                        variableName = x.VariableName,
                        fontSize = x.FontSize,
                        isBold = x.IsBold,
                        strokeWidth = x.StrokeWidth
                    })
            });
        }
        private async Task<string?> SaveMarkingLayoutFileAsync(IFormFile? file, string subFolder)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[]
            {
        ".dxf", ".xml", ".layout", ".lbl", ".json", ".png", ".jpg", ".jpeg", ".svg"
    };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid layout file type.");

            var folder = Path.Combine(_env.WebRootPath, "uploads", "marking-layouts", subFolder);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/marking-layouts/{subFolder}/{fileName}";
        }

        private async Task<WeaponMarkingViewModel> PopulateMarkingModel(WeaponMarkingViewModel model)
        {
            model.Weapons = await _db.Weapons
                .Include(x => x.WeaponModel)
                .Include(x => x.WeaponType)
                .Include(x => x.Manufacturer)
                .Include(x => x.Caliber)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Take(100)
                .ToListAsync();

            if (!model.SelectedWeaponId.HasValue)
            {
                model.LaserJobs = new List<LaserJob>();
                return model;
            }

            model.SelectedWeapon = await _db.Weapons
                .Include(x => x.WeaponModel)
                .Include(x => x.WeaponType)
                .Include(x => x.Manufacturer)
                .Include(x => x.Caliber)
                .FirstOrDefaultAsync(x =>
                    x.Id == model.SelectedWeaponId.Value &&
                    !x.IsDeleted);

            model.LaserJobs = await _db.LaserJobs
                .Where(x => x.WeaponId == model.SelectedWeaponId.Value)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();

            return model;
        }
        private async Task<WeaponQueryViewModel> PopulateQueryModel(WeaponQueryViewModel model)
        {
            model.WeaponTypes = await SelectWeaponTypes();
            model.WeaponModels = await SelectWeaponModels();
            model.Manufacturers = await SelectManufacturers();
            model.Calibers = await SelectCalibers();

            model.Countries = await SelectLookup("Country");
            model.Regions = await SelectLookup("Region");
            model.OriginalLocations = await SelectLookup("OriginalLocation");
            model.OriginIndicators = await SelectLookup("OriginIndicator");
            model.Stocks = await SelectLookup("Stock");
            model.Units = await SelectLookup("Unit");

            return model;
        }

        private async Task<WeaponMoveAuthoriseViewModel> PopulateAuthoriseMoveModel(WeaponMoveAuthoriseViewModel model)
        {
            model.PreparedMoves = await _db.WeaponMoves
                .Include(x => x.Weapon)
                    .ThenInclude(x => x.WeaponModel)
                .Include(x => x.Weapon)
                    .ThenInclude(x => x.WeaponType)
                .Include(x => x.Weapon)
                    .ThenInclude(x => x.Manufacturer)
                .Include(x => x.Weapon)
                    .ThenInclude(x => x.Caliber)
                .Include(x => x.Weapon)
                    .ThenInclude(x => x.OriginalStateLookup)
                .Include(x => x.MovementActionLookup)
                .Include(x => x.NewLocationLookup)
                .Where(x => !x.IsDeleted && x.Status == "Prepared")
                .OrderByDescending(x => x.PreparedAt)
                .ToListAsync();

            model.AuthorisedMoves = await _db.WeaponMoves
                .Include(x => x.Weapon)
                    .ThenInclude(x => x.WeaponModel)
                .Include(x => x.MovementActionLookup)
                .Include(x => x.NewLocationLookup)
                .Where(x => !x.IsDeleted && x.Status == "Authorised")
                .OrderByDescending(x => x.AuthorisedAt)
                .Take(50)
                .ToListAsync();

            if (model.SelectedMoveId != null)
            {
                model.SelectedMove = await _db.WeaponMoves
                    .Include(x => x.Weapon)
                        .ThenInclude(x => x.WeaponModel)
                    .Include(x => x.Weapon)
                        .ThenInclude(x => x.WeaponType)
                    .Include(x => x.Weapon)
                        .ThenInclude(x => x.Manufacturer)
                    .Include(x => x.Weapon)
                        .ThenInclude(x => x.Caliber)
                    .Include(x => x.Weapon)
                        .ThenInclude(x => x.OriginalStateLookup)
                    .Include(x => x.MovementActionLookup)
                    .Include(x => x.NewLocationLookup)
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.SelectedMoveId &&
                        !x.IsDeleted);
            }

            return model;
        }
        private async Task<WeaponMoveViewModel> PopulateMoveModel(WeaponMoveViewModel model)
        {
            model.MovementActions = await SelectLookup("MovementAction");
            model.NewLocations = await SelectLookup("MovedWeaponLocation");

            var query = _db.Weapons
                .Include(x => x.WeaponType)
                .Include(x => x.WeaponModel)
                .Include(x => x.Manufacturer)
                .Include(x => x.Caliber)
                .Include(x => x.OriginalLocationLookup)
                .Include(x => x.ManufactureCountryLookup)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                query = query.Where(x =>
                    x.RegistrationNo.Contains(model.SearchTerm) ||
                    (x.FactorySerial != null && x.FactorySerial.Contains(model.SearchTerm)) ||
                    (x.InventoryNo != null && x.InventoryNo.Contains(model.SearchTerm)) ||
                    (x.WeaponModel != null && x.WeaponModel.Name.Contains(model.SearchTerm)));
            }

            model.ActiveWeapons = await query
                .OrderByDescending(x => x.CreatedAt)
                .Take(100)
                .ToListAsync();

            if (model.SelectedWeaponId != null)
            {
                model.SelectedWeapon = await _db.Weapons
                    .Include(x => x.WeaponType)
                    .Include(x => x.WeaponModel)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Caliber)
                    .Include(x => x.OriginalLocationLookup)
                    .Include(x => x.ManufactureCountryLookup)
                    .FirstOrDefaultAsync(x => x.Id == model.SelectedWeaponId && !x.IsDeleted);

                model.MoveHistory = await _db.WeaponMoves
                    .Include(x => x.MovementActionLookup)
                    .Include(x => x.NewLocationLookup)
                    .Where(x => x.WeaponId == model.SelectedWeaponId && !x.IsDeleted)
                    .OrderByDescending(x => x.PreparedAt)
                    .ToListAsync();
            }

            return model;
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

        private async Task<WeaponCheckViewModel> PopulateCheckModel(WeaponCheckViewModel model)
        {
            model.CheckStates = await SelectLookup("CheckState");

            var query = _db.Weapons
                .Include(x => x.WeaponType)
                .Include(x => x.WeaponModel)
                .Include(x => x.Manufacturer)
                .Include(x => x.Caliber)
                .Include(x => x.OriginalStateLookup)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                query = query.Where(x =>
                    x.RegistrationNo.Contains(model.SearchTerm) ||
                    (x.FactorySerial != null && x.FactorySerial.Contains(model.SearchTerm)) ||
                    (x.InventoryNo != null && x.InventoryNo.Contains(model.SearchTerm)) ||
                    (x.WeaponModel != null && x.WeaponModel.Name.Contains(model.SearchTerm)));
            }

            model.ActiveWeapons = await query
                .OrderByDescending(x => x.CreatedAt)
                .Take(100)
                .ToListAsync();

            if (model.SelectedWeaponId != null)
            {
                model.SelectedWeapon = await _db.Weapons
                    .Include(x => x.WeaponType)
                    .Include(x => x.WeaponModel)
                    .Include(x => x.Manufacturer)
                    .Include(x => x.Caliber)
                    .Include(x => x.OriginalStateLookup)
                    .FirstOrDefaultAsync(x => x.Id == model.SelectedWeaponId && !x.IsDeleted);

                model.CheckHistory = await _db.WeaponChecks
                    .Include(x => x.CheckStateLookup)
                    .Where(x => x.WeaponId == model.SelectedWeaponId && !x.IsDeleted)
                    .OrderByDescending(x => x.CheckDate)
                    .ThenByDescending(x => x.CreatedAt)
                    .ToListAsync();
            }

            return model;
        }

        private async Task<List<Weapon>> GetWeaponQueryResults(WeaponQueryViewModel model)
        {
            var query = _db.Weapons
                .Include(x => x.WeaponType)
                .Include(x => x.WeaponModel)
                .Include(x => x.Manufacturer)
                .Include(x => x.Caliber)
                .Include(x => x.CountryLookup)
                .Include(x => x.RegionLookup)
                .Include(x => x.OriginalLocationLookup)
                .Include(x => x.OriginIndicatorLookup)
                .Include(x => x.StockLookup)
                .Include(x => x.UnitLookup)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.RegistrationNo))
                query = query.Where(x => x.RegistrationNo.Contains(model.RegistrationNo));

            if (!string.IsNullOrWhiteSpace(model.FactorySerial))
                query = query.Where(x => x.FactorySerial != null && x.FactorySerial.Contains(model.FactorySerial));

            if (model.WeaponTypeId != null)
                query = query.Where(x => x.WeaponTypeId == model.WeaponTypeId);

            if (model.WeaponModelId != null)
                query = query.Where(x => x.WeaponModelId == model.WeaponModelId);

            if (model.ManufacturerId != null)
                query = query.Where(x => x.ManufacturerId == model.ManufacturerId);

            if (model.CaliberId != null)
                query = query.Where(x => x.CaliberId == model.CaliberId);

            if (model.CountryLookupId != null)
                query = query.Where(x => x.CountryLookupId == model.CountryLookupId);

            if (model.RegionLookupId != null)
                query = query.Where(x => x.RegionLookupId == model.RegionLookupId);

            if (model.OriginalLocationLookupId != null)
                query = query.Where(x => x.OriginalLocationLookupId == model.OriginalLocationLookupId);

            if (model.OriginIndicatorLookupId != null)
                query = query.Where(x => x.OriginIndicatorLookupId == model.OriginIndicatorLookupId);

            if (model.StockLookupId != null)
                query = query.Where(x => x.StockLookupId == model.StockLookupId);

            if (model.UnitLookupId != null)
                query = query.Where(x => x.UnitLookupId == model.UnitLookupId);

            if (model.IsMarked != null)
                query = query.Where(x => x.IsMarked == model.IsMarked);

            if (model.IsProspective != null)
                query = query.Where(x => x.IsProspective == model.IsProspective);

            if (model.RegistrationDateFrom != null)
                query = query.Where(x => x.RegistrationDate.Date >= model.RegistrationDateFrom.Value.Date);

            if (model.RegistrationDateTo != null)
                query = query.Where(x => x.RegistrationDate.Date <= model.RegistrationDateTo.Value.Date);

            return await query
                .OrderByDescending(x => x.RegistrationDate)
                .Take(500)
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
            model.Teams = await SelectLookup("Team");
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