namespace AMS_data.Entities
{
    public class Skladiste
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string? Sifra { get; set; }

        public int? OrganizacionaJedinicaId { get; set; }
        public OrganizacionaJedinica? OrganizacionaJedinica { get; set; }

        public bool Aktivno { get; set; } = true;
    }
}