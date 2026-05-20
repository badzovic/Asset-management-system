using AMS_data.Entities.Narcotics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Narcotics
{
    public class NarcoticsDepositViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string RegistrationNo { get; set; } = string.Empty;

        public string? CaseNo { get; set; }
        public int? CaseTypeLookupId { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public int? OUPerformedSeizureLookupId { get; set; }

        public string? StorageOrderNo { get; set; }
        public DateTime? StorageOrderDate { get; set; }

        public string? LinkToOrderNo { get; set; }
        public string? ConfirmStorageOrderNo { get; set; }

        public int? DepositLocationLookupId { get; set; }
        public int? EvidenceIndicatorLookupId { get; set; }

        public string? SubmittedByOfficer { get; set; }
        public string? HandlingOfficer { get; set; }

        public string? ForensicReportNo { get; set; }
        public DateTime? ForensicReportDate { get; set; }

        public string? VerdictNo { get; set; }
        public DateTime? VerdictDate { get; set; }

        public string? DestructionOrderNo { get; set; }
        public DateTime? DestructionOrderDate { get; set; }

        public DateTime? DestructionDate { get; set; }

        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? PersonalIdNo { get; set; }

        public string? CaseInfoFolderPath { get; set; }
        public string? Remarks { get; set; }

        public int? NarcoticsTypeLookupId { get; set; }
        public decimal? Quantity { get; set; }
        public int? QuantityUnitLookupId { get; set; }
        public int? CompositionLookupId { get; set; }

        public List<SelectListItem> CaseTypes { get; set; } = new();
        public List<SelectListItem> OUPerformedSeizures { get; set; } = new();
        public List<SelectListItem> DepositLocations { get; set; } = new();
        public List<SelectListItem> EvidenceIndicators { get; set; } = new();

        public List<SelectListItem> NarcoticsTypes { get; set; } = new();
        public List<SelectListItem> QuantityUnits { get; set; } = new();
        public List<SelectListItem> Compositions { get; set; } = new();

        public List<NarcoticsDeposit> RecentDeposits { get; set; } = new();
        public List<NarcoticsDepositItem> Items { get; set; } = new();
    }
}