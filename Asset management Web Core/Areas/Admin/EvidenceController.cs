using AMS_data;
using AMS_data.Entities.Evidence;
using AMS_services;
using AMS_services;
using AMS_services.Audit;
using Asset_management_Web_Core.Areas.Admin.ViewModels.Evidence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace Asset_management_Web_Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EvidenceController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly AuditLogService _auditLogService;
        private readonly EvidencePdfService _evidencePdfService;

        public EvidenceController(
            ApplicationDbContext db,
            AuditLogService auditLogService,
            EvidencePdfService evidencePdfService)
        {
            _db = db;
            _auditLogService = auditLogService;
            _evidencePdfService = evidencePdfService;
        }


        public async Task<IActionResult> Deposit(int? id)
        {
            var model = new EvidenceDepositViewModel
            {
                RegistrationDate = DateTime.Today
            };

            if (id != null)
            {
                var deposit = await _db.EvidenceDeposits
                    .Include(x => x.Items.Where(i => !i.IsDeleted))
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (deposit == null)
                    return NotFound();

                model.Id = deposit.Id;
                model.RegistrationNo = deposit.RegistrationNo;
                model.CaseNo = deposit.CaseNo;
                model.CaseTypeLookupId = deposit.CaseTypeLookupId;
                model.EvidenceIndicatorLookupId = deposit.EvidenceIndicatorLookupId;
                model.DepositLocationLookupId = deposit.DepositLocationLookupId;
                model.RegistrationDate = deposit.RegistrationDate;
                model.ReceivedDate = deposit.ReceivedDate;
                model.StorageOrderNo = deposit.StorageOrderNo;
                model.StorageOrderDate = deposit.StorageOrderDate;
                model.SubmittedByOfficer = deposit.SubmittedByOfficer;
                model.HandlingOfficer = deposit.HandlingOfficer;
                model.FirstName = deposit.FirstName;
                model.Surname = deposit.Surname;
                model.Address = deposit.Address;
                model.PersonalIdNo = deposit.PersonalIdNo;
                model.SexLookupId = deposit.SexLookupId;
                model.AgeBandLookupId = deposit.AgeBandLookupId;
                model.IsCoCriminalOffence = deposit.IsCoCriminalOffence;
                model.IsGenderBasedViolence = deposit.IsGenderBasedViolence;
                model.CaseInfoFolderPath = deposit.CaseInfoFolderPath;
                model.Remarks = deposit.Remarks;
                model.Items = deposit.Items.Where(x => !x.IsDeleted).ToList();
            }
            else
            {
                model.RegistrationNo = await GenerateEvidenceRegistrationNo();
            }

            model = await PopulateEvidenceModel(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(EvidenceDepositViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await PopulateEvidenceModel(model);
                return View(model);
            }

            EvidenceDeposit deposit;

            if (model.Id != null)
            {
                deposit = await _db.EvidenceDeposits
                    .FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted);

                if (deposit == null)
                    return NotFound();

                deposit.UpdatedAt = DateTime.UtcNow;
                deposit.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                deposit = new EvidenceDeposit
                {
                    RegistrationNo = model.RegistrationNo,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _db.EvidenceDeposits.Add(deposit);
            }

            deposit.CaseNo = model.CaseNo;
            deposit.CaseTypeLookupId = model.CaseTypeLookupId;
            deposit.EvidenceIndicatorLookupId = model.EvidenceIndicatorLookupId;
            deposit.DepositLocationLookupId = model.DepositLocationLookupId;
            deposit.RegistrationDate = model.RegistrationDate;
            deposit.ReceivedDate = model.ReceivedDate;
            deposit.StorageOrderNo = model.StorageOrderNo;
            deposit.StorageOrderDate = model.StorageOrderDate;
            deposit.SubmittedByOfficer = model.SubmittedByOfficer;
            deposit.HandlingOfficer = model.HandlingOfficer;
            deposit.FirstName = model.FirstName;
            deposit.Surname = model.Surname;
            deposit.Address = model.Address;
            deposit.PersonalIdNo = model.PersonalIdNo;
            deposit.SexLookupId = model.SexLookupId;
            deposit.AgeBandLookupId = model.AgeBandLookupId;
            deposit.IsCoCriminalOffence = model.IsCoCriminalOffence;
            deposit.IsGenderBasedViolence = model.IsGenderBasedViolence;
            deposit.CaseInfoFolderPath = model.CaseInfoFolderPath;
            deposit.Remarks = model.Remarks;

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: model.Id == null ? "CREATE_EVIDENCE_DEPOSIT" : "UPDATE_EVIDENCE_DEPOSIT",
                entityName: "EvidenceDeposit",
                entityId: deposit.Id.ToString(),
                newValues: new
                {
                    deposit.RegistrationNo,
                    deposit.CaseNo,
                    deposit.CaseTypeLookupId,
                    deposit.EvidenceIndicatorLookupId,
                    deposit.DepositLocationLookupId
                });

            TempData["SuccessMessage"] = "Evidence deposit je uspješno sačuvan.";

            return RedirectToAction(nameof(Deposit), new { id = deposit.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(EvidenceDepositViewModel model)
        {
            if (model.Id == null)
                return RedirectToAction(nameof(Deposit));

            var deposit = await _db.EvidenceDeposits
                .FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            var item = new EvidenceDepositItem
            {
                EvidenceDepositId = deposit.Id,
                Description = model.ItemDescription,
                WeaponItemTypeLookupId = model.WeaponItemTypeLookupId,
                EvidenceWeaponTypeLookupId = model.EvidenceWeaponTypeLookupId,
                EvidenceWeaponLookupId = model.EvidenceWeaponLookupId,
                WeaponLegalityLookupId = model.WeaponLegalityLookupId,
                Quantity = model.Quantity,
                Unit = model.Unit,
                SerialNo = model.SerialNo,
                MarkingText = model.MarkingText,
                CreatedAt = DateTime.UtcNow
            };

            _db.EvidenceDepositItems.Add(item);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Evidence item je dodan.";

            return RedirectToAction(nameof(Deposit), new { id = deposit.Id });
        }
        public async Task<IActionResult> PrintDeposit(int id)
        {
            var deposit = await _db.EvidenceDeposits
                .Include(x => x.CreatedByUser)
                .Include(x => x.Items.Where(i => !i.IsDeleted))
                .Include(x => x.MoveHistories.Where(m => !m.IsDeleted))
                .ThenInclude(x => x.FromLocationLookup)
                .Include(x => x.MoveHistories.Where(m => !m.IsDeleted))
                .ThenInclude(x => x.ToLocationLookup)
                .Include(x => x.MoveHistories.Where(m => !m.IsDeleted))
                .ThenInclude(x => x.MovePurposeLookup).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            var pdfBytes = _evidencePdfService.GenerateDepositPdf(deposit);

            return File(
                pdfBytes,
                "application/pdf",
                $"EvidenceDeposit_{deposit.RegistrationNo}.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> ExportEvidenceQueryPdf(EvidenceQueryGeneratorViewModel model)
        {
            var data = await BuildEvidenceQuery(model)
                .OrderByDescending(x => x.RegistrationDate)
                .Take(5000)
                .ToListAsync();

            var pdfBytes = _evidencePdfService.GenerateEvidenceQueryPdf(data);

            return File(
                pdfBytes,
                "application/pdf",
                $"EvidenceQuery_{DateTime.Now:yyyyMMdd_HHmm}.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> QueryGenerator(EvidenceQueryGeneratorViewModel model)
        {
            model.CaseTypes = await SelectLookup("CaseType");
            model.DepositLocations = await SelectLookup("EvidenceDepositLocation");

            var hasAnyFilter =
                !string.IsNullOrWhiteSpace(model.RegistrationNo) ||
                !string.IsNullOrWhiteSpace(model.CaseNo) ||
                model.CaseTypeLookupId.HasValue ||
                model.DepositLocationLookupId.HasValue ||
                model.RegistrationDateFrom.HasValue ||
                model.RegistrationDateTo.HasValue;

            model.HasSearched = hasAnyFilter;

            if (!hasAnyFilter)
                return View(model);

            var query = _db.EvidenceDeposits
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Include(x => x.EvidenceIndicatorLookup)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.RegistrationNo))
                query = query.Where(x => x.RegistrationNo.Contains(model.RegistrationNo));

            if (!string.IsNullOrWhiteSpace(model.CaseNo))
                query = query.Where(x => x.CaseNo != null && x.CaseNo.Contains(model.CaseNo));

            if (model.CaseTypeLookupId.HasValue)
                query = query.Where(x => x.CaseTypeLookupId == model.CaseTypeLookupId.Value);

            if (model.DepositLocationLookupId.HasValue)
                query = query.Where(x => x.DepositLocationLookupId == model.DepositLocationLookupId.Value);

            if (model.RegistrationDateFrom.HasValue)
                query = query.Where(x => x.RegistrationDate >= model.RegistrationDateFrom.Value);

            if (model.RegistrationDateTo.HasValue)
                query = query.Where(x => x.RegistrationDate <= model.RegistrationDateTo.Value);

            model.Results = await BuildEvidenceQuery(model)
             .OrderByDescending(x => x.RegistrationDate)
             .Take(1000)
             .ToListAsync();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ExportEvidenceQueryCsv(EvidenceQueryGeneratorViewModel model)
        {
            var data = await BuildEvidenceQuery(model)
                .OrderByDescending(x => x.RegistrationDate)
                .Take(5000)
                .ToListAsync();

            var sb = new StringBuilder();

            sb.AppendLine("Registration No;Case No;Case Type;Registration Date;Received Date;Deposit Location;Evidence Indicator;First Name;Surname;Personal ID No;Status");

            foreach (var x in data)
            {
                sb.AppendLine(string.Join(";",
                    Csv(x.RegistrationNo),
                    Csv(x.CaseNo),
                    Csv(x.CaseTypeLookup?.Name),
                    Csv(x.RegistrationDate.ToString("dd.MM.yyyy")),
                    Csv(x.ReceivedDate?.ToString("dd.MM.yyyy")),
                    Csv(x.DepositLocationLookup?.Name),
                    Csv(x.EvidenceIndicatorLookup?.Name),
                    Csv(x.FirstName),
                    Csv(x.Surname),
                    Csv(x.PersonalIdNo),
                    Csv(x.Status)
                ));
            }

            var bytes = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();

            return File(bytes, "text/csv", $"EvidenceQuery_{DateTime.Now:yyyyMMdd_HHmm}.csv");
        }

        private static string Csv(string? value)
        {
            value ??= "";
            value = value.Replace("\"", "\"\"");
            return $"\"{value}\"";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMove(EvidenceMoveViewModel model)
        {
            var deposit = await _db.EvidenceDeposits
                .FirstOrDefaultAsync(x => x.Id == model.EvidenceDepositId && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            var move = new EvidenceMoveHistory
            {
                EvidenceDepositId = model.EvidenceDepositId,
                EvidenceDepositItemId = model.EvidenceDepositItemId,
                FromLocationLookupId = model.FromLocationLookupId,
                ToLocationLookupId = model.ToLocationLookupId,
                MovePurposeLookupId = model.MovePurposeLookupId,
                MoveDate = model.MoveDate,
                ApprovedBy = model.ApprovedBy,
                MovedBy = model.MovedBy,
                Remarks = model.Remarks,
                CreatedAt = DateTime.UtcNow
            };

            _db.EvidenceMoveHistories.Add(move);

            if (model.EvidenceDepositItemId == null)
            {
                deposit.DepositLocationLookupId = model.ToLocationLookupId;
                deposit.UpdatedAt = DateTime.UtcNow;
                deposit.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "EVIDENCE_MOVE",
                entityName: "EvidenceDeposit",
                entityId: deposit.Id.ToString(),
                newValues: new
                {
                    model.FromLocationLookupId,
                    model.ToLocationLookupId,
                    model.MovePurposeLookupId,
                    model.MoveDate,
                    model.ApprovedBy,
                    model.MovedBy
                });

            TempData["SuccessMessage"] = "Move history zapis je uspješno dodan.";

            return RedirectToAction(nameof(MoveHistory), new { id = model.EvidenceDepositId });
        }
        public async Task<IActionResult> MoveHistory(int id)
        {
            var deposit = await _db.EvidenceDeposits
                .Include(x => x.DepositLocationLookup)
                .Include(x => x.Items.Where(i => !i.IsDeleted))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            var model = new EvidenceMoveViewModel
            {
                EvidenceDepositId = deposit.Id,
                FromLocationLookupId = deposit.DepositLocationLookupId,
                MoveDate = DateTime.Now,
                Locations = await SelectLookup("EvidenceDepositLocation"),
                MovePurposes = await SelectLookup("MoveEvidencePurpose")
            };

            model.History = await _db.EvidenceMoveHistories
                .Include(x => x.EvidenceDepositItem)
                .Include(x => x.FromLocationLookup)
                .Include(x => x.ToLocationLookup)
                .Include(x => x.MovePurposeLookup)
                .Where(x => x.EvidenceDepositId == id && !x.IsDeleted)
                .OrderByDescending(x => x.MoveDate)
                .Select(x => new EvidenceMoveHistoryRowVM
                {
                    Id = x.Id,
                    Item = x.EvidenceDepositItem != null
                        ? x.EvidenceDepositItem.Description
                        : "Whole deposit",
                    FromLocation = x.FromLocationLookup != null ? x.FromLocationLookup.Name : "",
                    ToLocation = x.ToLocationLookup != null ? x.ToLocationLookup.Name : "",
                    Purpose = x.MovePurposeLookup != null ? x.MovePurposeLookup.Name : "",
                    MoveDate = x.MoveDate,
                    ApprovedBy = x.ApprovedBy,
                    MovedBy = x.MovedBy
                })
                .ToListAsync();

            ViewBag.DepositRegistrationNo = deposit.RegistrationNo;
            ViewBag.CaseNo = deposit.CaseNo;

            return View(model);
        }
        public async Task<IActionResult> PrintLabel(int id)
        {
            var deposit = await _db.EvidenceDeposits
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            TempData["SuccessMessage"] = "Label print job ćemo povezati u sljedećem koraku.";

            return RedirectToAction(nameof(Deposit), new { id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDeposit(int id)
        {
            var deposit = await _db.EvidenceDeposits
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            deposit.IsDeleted = true;
            deposit.UpdatedAt = DateTime.UtcNow;
            deposit.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "DELETE_EVIDENCE_DEPOSIT",
                entityName: "EvidenceDeposit",
                entityId: deposit.Id.ToString(),
                newValues: new
                {
                    deposit.RegistrationNo,
                    deposit.CaseNo
                });

            TempData["SuccessMessage"] = "Evidence deposit je obrisan.";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.EvidenceDepositItems
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (item == null)
                return NotFound();

            item.IsDeleted = true;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Evidence item je obrisan.";

            return RedirectToAction(nameof(Deposit), new { id = item.EvidenceDepositId });
        }
        public async Task<IActionResult> Index()
        {
            var deposits = await _db.EvidenceDeposits
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(deposits);
        }
        private IQueryable<EvidenceDeposit> BuildEvidenceQuery(EvidenceQueryGeneratorViewModel model)
        {
            var query = _db.EvidenceDeposits
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Include(x => x.EvidenceIndicatorLookup)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.RegistrationNo))
                query = query.Where(x => x.RegistrationNo.Contains(model.RegistrationNo));

            if (!string.IsNullOrWhiteSpace(model.CaseNo))
                query = query.Where(x => x.CaseNo != null && x.CaseNo.Contains(model.CaseNo));

            if (model.CaseTypeLookupId.HasValue)
                query = query.Where(x => x.CaseTypeLookupId == model.CaseTypeLookupId.Value);

            if (model.DepositLocationLookupId.HasValue)
                query = query.Where(x => x.DepositLocationLookupId == model.DepositLocationLookupId.Value);

            if (model.RegistrationDateFrom.HasValue)
                query = query.Where(x => x.RegistrationDate >= model.RegistrationDateFrom.Value);

            if (model.RegistrationDateTo.HasValue)
                query = query.Where(x => x.RegistrationDate <= model.RegistrationDateTo.Value);

            return query;
        }

        private async Task<EvidenceDepositViewModel> PopulateEvidenceModel(EvidenceDepositViewModel model)
        {
            model.CaseTypes = await SelectLookup("CaseType");
            model.EvidenceIndicators = await SelectLookup("EvidenceIndicator");
            model.DepositLocations = await SelectLookup("EvidenceDepositLocation");
            model.Sexes = await SelectLookup("Sex");
            model.AgeBands = await SelectLookup("AgeBand");

            model.WeaponItemTypes = await SelectLookup("WeaponItemType");
            model.EvidenceWeaponTypes = await SelectLookup("EvidenceWeaponType");
            model.EvidenceWeapons = await SelectLookup("EvidenceWeapon");
            model.WeaponLegalities = await SelectLookup("WeaponLegality");

            model.RecentDeposits = await _db.EvidenceDeposits
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Take(50)
                .ToListAsync();

            return model;
        }

        private async Task<List<SelectListItem>> SelectLookup(string key)
        {
            return await _db.LookupItems
                .Include(x => x.LookupCategory)
                .Where(x => x.LookupCategory.Key == key && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        private async Task<string> GenerateEvidenceRegistrationNo()
        {
            var year = DateTime.Now.Year;

            var count = await _db.EvidenceDeposits
                .CountAsync(x => x.CreatedAt.Year == year);

            return $"EVD-{year}-{(count + 1):00000}";
        }
    }
}