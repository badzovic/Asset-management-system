namespace Asset_management_Web_Core.Areas.Admin.ViewModels
{
    public class UserListItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? RoleName { get; set; }
        public string? OrganizacionaJedinica { get; set; }
        public string? Skladiste { get; set; }
        public bool Aktivan { get; set; }
    }
}