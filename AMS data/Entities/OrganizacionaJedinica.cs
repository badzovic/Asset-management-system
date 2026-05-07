namespace AMS_data.Entities
{
    public class OrganizacionaJedinica
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string? Sifra { get; set; }
        public bool Aktivna { get; set; } = true;
    }
}