using AMS_data.Entities.Evidence;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Evidence
{
    public class EvidenceDepositViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string RegistrationNo { get; set; } = string.Empty;

        public string? CaseNo { get; set; }

        public int? CaseTypeLookupId { get; set; }
        public int? EvidenceIndicatorLookupId { get; set; }
        public int? DepositLocationLookupId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.Today;

        public DateTime? ReceivedDate { get; set; }

        public string? StorageOrderNo { get; set; }
        public DateTime? StorageOrderDate { get; set; }

        public string? SubmittedByOfficer { get; set; }
        public string? HandlingOfficer { get; set; }

        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? PersonalIdNo { get; set; }

        public int? SexLookupId { get; set; }
        public int? AgeBandLookupId { get; set; }

        public bool IsCoCriminalOffence { get; set; }
        public bool IsGenderBasedViolence { get; set; }

        public string? CaseInfoFolderPath { get; set; }
        public string? Remarks { get; set; }

        public string? ItemDescription { get; set; }
        public int? WeaponItemTypeLookupId { get; set; }
        public int? EvidenceWeaponTypeLookupId { get; set; }
        public int? EvidenceWeaponLookupId { get; set; }
        public int? WeaponLegalityLookupId { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public string? SerialNo { get; set; }
        public string? MarkingText { get; set; }

        public List<SelectListItem> CaseTypes { get; set; } = new();
        public List<SelectListItem> EvidenceIndicators { get; set; } = new();
        public List<SelectListItem> DepositLocations { get; set; } = new();
        public List<SelectListItem> Sexes { get; set; } = new();
        public List<SelectListItem> AgeBands { get; set; } = new();

        public List<SelectListItem> WeaponItemTypes { get; set; } = new();
        public List<SelectListItem> EvidenceWeaponTypes { get; set; } = new();
        public List<SelectListItem> EvidenceWeapons { get; set; } = new();
        public List<SelectListItem> WeaponLegalities { get; set; } = new();

        public List<EvidenceDeposit> RecentDeposits { get; set; } = new();
        public List<EvidenceDepositItem> Items { get; set; } = new();
    }
}