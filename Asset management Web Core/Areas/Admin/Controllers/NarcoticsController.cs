using AMS_data;
using AMS_data.Entities.Narcotics;
using AMS_services;
using AMS_services.Audit;
using Asset_management_Web_Core.Areas.Admin.ViewModels.Narcotics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Asset_management_Web_Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NarcoticsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly AuditLogService _auditLogService;
        private readonly NarcoticsPdfService _narcoticsPdfService;

        public NarcoticsController(
            ApplicationDbContext db,
            AuditLogService auditLogService,
            NarcoticsPdfService narcoticsPdfService)
        {
            _db = db;
            _auditLogService = auditLogService;
            _narcoticsPdfService = narcoticsPdfService;
        }

        public async Task<IActionResult> Index()
        {
            var deposits = await _db.NarcoticsDeposits
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(deposits);
        }

        public async Task<IActionResult> Register(int? id)
        {
            var model = new NarcoticsDepositViewModel();

            if (id != null)
            {
                var deposit = await _db.NarcoticsDeposits
                    .Include(x => x.Items.Where(i => !i.IsDeleted))
                        .ThenInclude(x => x.NarcoticsTypeLookup)
                    .Include(x => x.Items.Where(i => !i.IsDeleted))
                        .ThenInclude(x => x.QuantityUnitLookup)
                    .Include(x => x.Items.Where(i => !i.IsDeleted))
                        .ThenInclude(x => x.CompositionLookup)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (deposit == null)
                    return NotFound();

                model.Id = deposit.Id;
                model.RegistrationNo = deposit.RegistrationNo;
                model.CaseNo = deposit.CaseNo;
                model.CaseTypeLookupId = deposit.CaseTypeLookupId;
                model.ReceivedDate = deposit.ReceivedDate;
                model.OUPerformedSeizureLookupId = deposit.OUPerformedSeizureLookupId;
                model.StorageOrderNo = deposit.StorageOrderNo;
                model.StorageOrderDate = deposit.StorageOrderDate;
                model.LinkToOrderNo = deposit.LinkToOrderNo;
                model.ConfirmStorageOrderNo = deposit.ConfirmStorageOrderNo;
                model.DepositLocationLookupId = deposit.DepositLocationLookupId;
                model.EvidenceIndicatorLookupId = deposit.EvidenceIndicatorLookupId;
                model.SubmittedByOfficer = deposit.SubmittedByOfficer;
                model.HandlingOfficer = deposit.HandlingOfficer;
                model.ForensicReportNo = deposit.ForensicReportNo;
                model.ForensicReportDate = deposit.ForensicReportDate;
                model.VerdictNo = deposit.VerdictNo;
                model.VerdictDate = deposit.VerdictDate;
                model.DestructionOrderNo = deposit.DestructionOrderNo;
                model.DestructionOrderDate = deposit.DestructionOrderDate;
                model.DestructionDate = deposit.DestructionDate;
                model.FirstName = deposit.FirstName;
                model.Surname = deposit.Surname;
                model.Address = deposit.Address;
                model.PersonalIdNo = deposit.PersonalIdNo;
                model.CaseInfoFolderPath = deposit.CaseInfoFolderPath;
                model.Remarks = deposit.Remarks;
                model.Items = deposit.Items.Where(x => !x.IsDeleted).ToList();
            }
            else
            {
                model.RegistrationNo = await GenerateNarcoticsRegistrationNo();
            }

            model = await PopulateNarcoticsModel(model);

            return View(model);
        }
        public async Task<IActionResult> PrintDeposit(int id)
        {
            var deposit = await _db.NarcoticsDeposits
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Include(x => x.EvidenceIndicatorLookup)
                .Include(x => x.OUPerformedSeizureLookup)
                .Include(x => x.CreatedByUser)

                .Include(x => x.Items.Where(i => !i.IsDeleted))
                    .ThenInclude(x => x.NarcoticsTypeLookup)

                .Include(x => x.Items.Where(i => !i.IsDeleted))
                    .ThenInclude(x => x.QuantityUnitLookup)

                .Include(x => x.Items.Where(i => !i.IsDeleted))
                    .ThenInclude(x => x.CompositionLookup)

                .Include(x => x.MoveHistories.Where(m => !m.IsDeleted))
                    .ThenInclude(x => x.FromLocationLookup)

                .Include(x => x.MoveHistories.Where(m => !m.IsDeleted))
                    .ThenInclude(x => x.ToLocationLookup)

                .Include(x => x.MoveHistories.Where(m => !m.IsDeleted))
                    .ThenInclude(x => x.MovePurposeLookup)

                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            var pdfBytes = _narcoticsPdfService.GenerateDepositPdf(deposit);

            return File(
                pdfBytes,
                "application/pdf",
                $"NarcoticsDeposit_{deposit.RegistrationNo}.pdf");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(NarcoticsDepositViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await PopulateNarcoticsModel(model);
                return View(model);
            }

            NarcoticsDeposit deposit;

            if (model.Id != null)
            {
                deposit = await _db.NarcoticsDeposits
                    .FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted);

                if (deposit == null)
                    return NotFound();

                deposit.UpdatedAt = DateTime.UtcNow;
                deposit.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                deposit = new NarcoticsDeposit
                {
                    RegistrationNo = model.RegistrationNo,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _db.NarcoticsDeposits.Add(deposit);
            }

            deposit.CaseNo = model.CaseNo;
            deposit.CaseTypeLookupId = model.CaseTypeLookupId;
            deposit.ReceivedDate = model.ReceivedDate;
            deposit.OUPerformedSeizureLookupId = model.OUPerformedSeizureLookupId;
            deposit.StorageOrderNo = model.StorageOrderNo;
            deposit.StorageOrderDate = model.StorageOrderDate;
            deposit.LinkToOrderNo = model.LinkToOrderNo;
            deposit.ConfirmStorageOrderNo = model.ConfirmStorageOrderNo;
            deposit.DepositLocationLookupId = model.DepositLocationLookupId;
            deposit.EvidenceIndicatorLookupId = model.EvidenceIndicatorLookupId;
            deposit.SubmittedByOfficer = model.SubmittedByOfficer;
            deposit.HandlingOfficer = model.HandlingOfficer;
            deposit.ForensicReportNo = model.ForensicReportNo;
            deposit.ForensicReportDate = model.ForensicReportDate;
            deposit.VerdictNo = model.VerdictNo;
            deposit.VerdictDate = model.VerdictDate;
            deposit.DestructionOrderNo = model.DestructionOrderNo;
            deposit.DestructionOrderDate = model.DestructionOrderDate;
            deposit.DestructionDate = model.DestructionDate;
            deposit.FirstName = model.FirstName;
            deposit.Surname = model.Surname;
            deposit.Address = model.Address;
            deposit.PersonalIdNo = model.PersonalIdNo;
            deposit.CaseInfoFolderPath = model.CaseInfoFolderPath;
            deposit.Remarks = model.Remarks;

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: model.Id == null ? "CREATE_NARCOTICS_DEPOSIT" : "UPDATE_NARCOTICS_DEPOSIT",
                entityName: "NarcoticsDeposit",
                entityId: deposit.Id.ToString(),
                newValues: new
                {
                    deposit.RegistrationNo,
                    deposit.CaseNo,
                    deposit.CaseTypeLookupId,
                    deposit.DepositLocationLookupId
                });

            TempData["SuccessMessage"] = "Narcotics deposit je uspješno sačuvan.";

            return RedirectToAction(nameof(Register), new { id = deposit.Id });
        }

        [HttpGet]
        public async Task<IActionResult> QueryGenerator(NarcoticsQueryGeneratorViewModel model)
        {
            model.CaseTypes = await SelectLookup("CaseType");
            model.DepositLocations = await SelectLookup("EvidenceDepositLocation");
            model.OUList = await SelectLookup("OUName");

            var hasAnyFilter =
                !string.IsNullOrWhiteSpace(model.RegistrationNo) ||
                !string.IsNullOrWhiteSpace(model.CaseNo) ||
                model.CaseTypeLookupId.HasValue ||
                model.DepositLocationLookupId.HasValue ||
                model.OUPerformedSeizureLookupId.HasValue ||
                model.DateFrom.HasValue ||
                model.DateTo.HasValue;

            model.HasSearched = hasAnyFilter;

            if (!hasAnyFilter)
                return View(model);

            model.Results = await BuildNarcoticsQuery(model)
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Include(x => x.OUPerformedSeizureLookup)
                .OrderByDescending(x => x.CreatedAt)
                .Take(1000)
                .ToListAsync();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(NarcoticsDepositViewModel model)
        {
            if (model.Id == null)
                return RedirectToAction(nameof(Register));

            var deposit = await _db.NarcoticsDeposits
                .FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            var item = new NarcoticsDepositItem
            {
                NarcoticsDepositId = deposit.Id,
                NarcoticsTypeLookupId = model.NarcoticsTypeLookupId,
                Quantity = model.Quantity,
                QuantityUnitLookupId = model.QuantityUnitLookupId,
                CompositionLookupId = model.CompositionLookupId,
                CreatedAt = DateTime.UtcNow
            };

            _db.NarcoticsDepositItems.Add(item);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Narcotics item je dodan.";

            return RedirectToAction(nameof(Register), new { id = deposit.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.NarcoticsDepositItems
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (item == null)
                return NotFound();

            item.IsDeleted = true;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Narcotics item je obrisan.";

            return RedirectToAction(nameof(Register), new { id = item.NarcoticsDepositId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDeposit(int id)
        {
            var deposit = await _db.NarcoticsDeposits
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            deposit.IsDeleted = true;
            deposit.UpdatedAt = DateTime.UtcNow;
            deposit.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Narcotics deposit je obrisan.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MoveHistory(int id)
        {
            var deposit = await _db.NarcoticsDeposits
                .Include(x => x.DepositLocationLookup)
                .Include(x => x.Items.Where(i => !i.IsDeleted))
                    .ThenInclude(x => x.NarcoticsTypeLookup)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            var model = new NarcoticsMoveViewModel
            {
                NarcoticsDepositId = deposit.Id,
                FromLocationLookupId = deposit.DepositLocationLookupId,
                MoveDate = DateTime.Now,
                Locations = await SelectLookup("EvidenceDepositLocation"),
                MovePurposes = await SelectLookup("MoveEvidencePurpose")
            };

            model.History = await _db.NarcoticsMoveHistories
                .Include(x => x.NarcoticsDepositItem)
                    .ThenInclude(x => x.NarcoticsTypeLookup)
                .Include(x => x.FromLocationLookup)
                .Include(x => x.ToLocationLookup)
                .Include(x => x.MovePurposeLookup)
                .Where(x => x.NarcoticsDepositId == id && !x.IsDeleted)
                .OrderByDescending(x => x.MoveDate)
                .Select(x => new NarcoticsMoveHistoryRowVM
                {
                    Id = x.Id,
                    Item = x.NarcoticsDepositItem != null && x.NarcoticsDepositItem.NarcoticsTypeLookup != null
                        ? x.NarcoticsDepositItem.NarcoticsTypeLookup.Name
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

        [HttpGet]
        public async Task<IActionResult> ExportQueryCsv(NarcoticsQueryGeneratorViewModel model)
        {
            var results = await BuildNarcoticsQuery(model)
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Include(x => x.OUPerformedSeizureLookup)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var sb = new System.Text.StringBuilder();

            sb.AppendLine("RegistrationNo,CaseNo,CaseType,Location,OU,Status");

            foreach (var x in results)
            {
                sb.AppendLine(
                    $"\"{x.RegistrationNo}\"," +
                    $"\"{x.CaseNo}\"," +
                    $"\"{x.CaseTypeLookup?.Name}\"," +
                    $"\"{x.DepositLocationLookup?.Name}\"," +
                    $"\"{x.OUPerformedSeizureLookup?.Name}\"," +
                    $"\"{x.Status}\"");
            }

            return File(
                System.Text.Encoding.UTF8.GetBytes(sb.ToString()),
                "text/csv",
                $"NarcoticsQuery_{DateTime.Now:yyyyMMddHHmmss}.csv");
        }

        [HttpGet]
        public async Task<IActionResult> ExportQueryPdf(NarcoticsQueryGeneratorViewModel model)
        {
            var results = await BuildNarcoticsQuery(model)
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Include(x => x.OUPerformedSeizureLookup)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var pdfBytes = _narcoticsPdfService.GenerateQueryPdf(results);

            return File(
                pdfBytes,
                "application/pdf",
                $"NarcoticsQuery_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMove(NarcoticsMoveViewModel model)
        {
            var deposit = await _db.NarcoticsDeposits
                .FirstOrDefaultAsync(x => x.Id == model.NarcoticsDepositId && !x.IsDeleted);

            if (deposit == null)
                return NotFound();

            var move = new NarcoticsMoveHistory
            {
                NarcoticsDepositId = model.NarcoticsDepositId,
                NarcoticsDepositItemId = model.NarcoticsDepositItemId,
                FromLocationLookupId = model.FromLocationLookupId,
                ToLocationLookupId = model.ToLocationLookupId,
                MovePurposeLookupId = model.MovePurposeLookupId,
                MoveDate = model.MoveDate,
                ApprovedBy = model.ApprovedBy,
                MovedBy = model.MovedBy,
                Remarks = model.Remarks,
                CreatedAt = DateTime.UtcNow
            };

            _db.NarcoticsMoveHistories.Add(move);

            if (model.NarcoticsDepositItemId == null)
            {
                deposit.DepositLocationLookupId = model.ToLocationLookupId;
                deposit.UpdatedAt = DateTime.UtcNow;
                deposit.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            await _db.SaveChangesAsync();

            await _auditLogService.LogAsync(
                action: "NARCOTICS_MOVE",
                entityName: "NarcoticsDeposit",
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

            TempData["SuccessMessage"] = "Pomjeranje narkotika je uspješno evidentirano.";

            return RedirectToAction(nameof(MoveHistory), new { id = model.NarcoticsDepositId });
        }
        private async Task<NarcoticsDepositViewModel> PopulateNarcoticsModel(NarcoticsDepositViewModel model)
        {
            model.CaseTypes = await SelectLookup("CaseType");
            model.OUPerformedSeizures = await SelectLookup("OUName");
            model.DepositLocations = await SelectLookup("EvidenceDepositLocation");
            model.EvidenceIndicators = await SelectLookup("EvidenceIndicator");

            model.NarcoticsTypes = await SelectLookup("NarcoticsType");
            model.QuantityUnits = await SelectLookup("NarcoticQuantityUnit");
            model.Compositions = await SelectLookup("NarcoticComposition");

            model.RecentDeposits = await _db.NarcoticsDeposits
                .Include(x => x.CaseTypeLookup)
                .Include(x => x.DepositLocationLookup)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Take(50)
                .ToListAsync();

            return model;
        }

        private IQueryable<NarcoticsDeposit> BuildNarcoticsQuery(
    NarcoticsQueryGeneratorViewModel model)
        {
            var query = _db.NarcoticsDeposits
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.RegistrationNo))
            {
                query = query.Where(x =>
                    x.RegistrationNo.Contains(model.RegistrationNo));
            }

            if (!string.IsNullOrWhiteSpace(model.CaseNo))
            {
                query = query.Where(x =>
                    x.CaseNo != null &&
                    x.CaseNo.Contains(model.CaseNo));
            }

            if (model.CaseTypeLookupId.HasValue)
            {
                query = query.Where(x =>
                    x.CaseTypeLookupId == model.CaseTypeLookupId);
            }

            if (model.DepositLocationLookupId.HasValue)
            {
                query = query.Where(x =>
                    x.DepositLocationLookupId == model.DepositLocationLookupId);
            }

            if (model.OUPerformedSeizureLookupId.HasValue)
            {
                query = query.Where(x =>
                    x.OUPerformedSeizureLookupId == model.OUPerformedSeizureLookupId);
            }

            if (model.DateFrom.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt >= model.DateFrom.Value);
            }

            if (model.DateTo.HasValue)
            {
                var to = model.DateTo.Value.Date.AddDays(1);

                query = query.Where(x =>
                    x.CreatedAt < to);
            }

            return query;
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

        private async Task<string> GenerateNarcoticsRegistrationNo()
        {
            var year = DateTime.Now.Year;

            var count = await _db.NarcoticsDeposits
                .CountAsync(x => x.CreatedAt.Year == year);

            return $"NRC-{year}-{(count + 1):00000}";
        }
    }
}