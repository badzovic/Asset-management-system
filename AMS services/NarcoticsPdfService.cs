using AMS_data.Entities.Narcotics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AMS_services
{

    public class NarcoticsPdfService
    {
        public byte[] GenerateDepositPdf(NarcoticsDeposit deposit)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(18);
                    page.DefaultTextStyle(x => x.FontSize(7));

                    page.Content().Column(col =>
                    {
                        col.Spacing(8);

                        col.Item().Text("Narcotics Deposit")
                            .FontSize(16)
                            .SemiBold();

                        col.Item().LineHorizontal(1.5f);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                Field(c, "Deposit registration number: *", deposit.RegistrationNo, true);
                                Field(c, "Case number: *", deposit.CaseNo);
                                Field(c, "Case type:", deposit.CaseTypeLookup?.Name);
                                Field(c, "Date received: *", DateValue(deposit.ReceivedDate));
                                Field(c, "OU that performed seizure:", deposit.OUPerformedSeizureLookup?.Name);
                                Field(c, "Storage order number:", deposit.StorageOrderNo);
                                Field(c, "Storage order date:", DateValue(deposit.StorageOrderDate));
                                Field(c, "Deposit location:", deposit.DepositLocationLookup?.Name);
                                Field(c, "Evidence indicator:", deposit.EvidenceIndicatorLookup?.Name);
                            });

                            row.ConstantItem(12);

                            row.RelativeItem().Column(c =>
                            {
                                Field(c, "Submitting officer:", deposit.SubmittedByOfficer);
                                Field(c, "Handling officer:", deposit.HandlingOfficer);
                                Field(c, "Forensic report no:", deposit.ForensicReportNo);
                                Field(c, "Forensic report date:", DateValue(deposit.ForensicReportDate));
                                Field(c, "Verdict no:", deposit.VerdictNo);
                                Field(c, "Verdict date:", DateValue(deposit.VerdictDate));
                                Field(c, "Destruction order no:", deposit.DestructionOrderNo);
                                Field(c, "Destruction order date:", DateValue(deposit.DestructionOrderDate));
                                Field(c, "Destruction date:", DateValue(deposit.DestructionDate));
                            });

                            row.ConstantItem(12);

                            row.RelativeItem().Column(c =>
                            {
                                Field(c, "First name:", deposit.FirstName);
                                Field(c, "Surname:", deposit.Surname);
                                Field(c, "Address:", deposit.Address);
                                Field(c, "Personal ID number:", deposit.PersonalIdNo);
                                Field(c, "Case information folder:", deposit.CaseInfoFolderPath);
                                TextArea(c, "Additional remarks:", deposit.Remarks);
                            });
                        });

                        col.Item().LineHorizontal(1.5f);

                        col.Item().Text("Narcotics items")
                            .FontSize(12)
                            .SemiBold();

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            Header(table, "Type");
                            Header(table, "Composition");
                            Header(table, "Quantity");
                            Header(table, "Status");

                            var items = deposit.Items.Where(x => !x.IsDeleted).ToList();

                            if (items.Any())
                            {
                                foreach (var item in items)
                                {
                                    Cell(table, item.NarcoticsTypeLookup?.Name);
                                    Cell(table, item.CompositionLookup?.Name);
                                    Cell(table, $"{item.Quantity} {item.QuantityUnitLookup?.Name}");
                                    Cell(table, item.Status);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 4; i++)
                                    Cell(table, "");
                            }
                        });

                        var moves = deposit.MoveHistories
                            .Where(x => !x.IsDeleted)
                            .OrderByDescending(x => x.MoveDate)
                            .ToList();

                        if (moves.Any())
                        {
                            col.Item().PaddingTop(10);

                            col.Item().Text("Movement history")
                                .FontSize(12)
                                .SemiBold();

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                Header(table, "Date");
                                Header(table, "From");
                                Header(table, "To");
                                Header(table, "Purpose");
                                Header(table, "Moved by");

                                foreach (var move in moves)
                                {
                                    Cell(table, move.MoveDate.ToString("dd.MM.yyyy"));
                                    Cell(table, move.FromLocationLookup?.Name);
                                    Cell(table, move.ToLocationLookup?.Name);
                                    Cell(table, move.MovePurposeLookup?.Name);
                                    Cell(table, move.MovedBy);
                                }
                            });
                        }

                        col.Item().PaddingTop(18);

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(110).Text("Recorded by: *").SemiBold();

                            row.ConstantItem(190)
                                .Border(0.5f)
                                .Height(14)
                                .PaddingLeft(3)
                                .AlignMiddle()
                                .Text(
                                    deposit.CreatedByUser != null
                                        ? $"{deposit.CreatedByUser.Ime} {deposit.CreatedByUser.Prezime}"
                                        : ""
                                );
                        });

                        col.Item().LineHorizontal(1.5f);
                    });
                });
            }).GeneratePdf();
        }

        public byte[] GenerateQueryPdf(List<NarcoticsDeposit> data)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontSize(8));

                    page.Header()
                        .Text("Izvještaj depozita narkotika")
                        .FontSize(18)
                        .SemiBold();

                    page.Content().PaddingTop(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1.2f);
                            columns.RelativeColumn(1.2f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1);
                        });

                        Header(table, "Registracija");
                        Header(table, "Broj predmeta");
                        Header(table, "Vrsta predmeta");
                        Header(table, "Lokacija");
                        Header(table, "OU zapljene");
                        Header(table, "Status");

                        foreach (var x in data)
                        {
                            Cell(table, x.RegistrationNo);
                            Cell(table, x.CaseNo);
                            Cell(table, x.CaseTypeLookup?.Name);
                            Cell(table, x.DepositLocationLookup?.Name);
                            Cell(table, x.OUPerformedSeizureLookup?.Name);
                            Cell(table, x.Status);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generisano: {DateTime.Now:dd.MM.yyyy HH:mm}");
                });
            }).GeneratePdf();
        }

        private static void Field(ColumnDescriptor c, string label, string? value, bool boldValue = false)
        {
            c.Item().PaddingBottom(4).Row(row =>
            {
                row.RelativeItem(1.25f)
                    .Text(label)
                    .SemiBold();

                var box = row.RelativeItem(1f)
                    .Height(13)
                    .Border(0.5f)
                    .BorderColor(Colors.Grey.Lighten1)
                    .PaddingLeft(3)
                    .AlignMiddle();

                if (boldValue)
                    box.Text(value ?? "").Bold();
                else
                    box.Text(value ?? "");
            });
        }

        private static void TextArea(ColumnDescriptor c, string label, string? value)
        {
            c.Item().PaddingTop(6).Text(label).SemiBold();

            c.Item()
                .Height(55)
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(4)
                .Text(value ?? "");
        }

        private static void Header(TableDescriptor table, string text)
        {
            table.Cell()
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten1)
                .Background(Colors.Grey.Lighten4)
                .Padding(3)
                .Text(text)
                .SemiBold();
        }

        private static void Cell(TableDescriptor table, string? text)
        {
            table.Cell()
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(3)
                .Text(text ?? "");
        }

        private static string DateValue(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd.MM.yyyy") : "";
        }
    }
}