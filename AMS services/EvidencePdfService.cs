using AMS_data.Entities.Evidence;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AMS_services
{
    public class EvidencePdfService
    {
        public byte[] GenerateDepositPdf(EvidenceDeposit deposit)
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

                        col.Item().Text("Evidence Deposit")
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
                                Field(c, "Storage order number: *", deposit.StorageOrderNo);
                                Field(c, "Storage order date: *", DateValue(deposit.StorageOrderDate));
                                Field(c, "Original evidence deposit location: *", deposit.DepositLocationLookup?.Name);
                                Field(c, "Authorized submitting officer: *", deposit.SubmittedByOfficer);
                                Field(c, "Evidence indicator/aquisition:", deposit.EvidenceIndicatorLookup?.Name);
                                Field(c, "First name:", deposit.FirstName);
                                Field(c, "Surname:", deposit.Surname);
                                Field(c, "Address:", deposit.Address);
                                Field(c, "Personal ID number:", deposit.PersonalIdNo);
                                Field(c, "Case information folder:", deposit.CaseInfoFolderPath);
                            });

                            row.ConstantItem(12);
                            row.RelativeItem().Column(c =>
                            {
                                Field(c, "Date of registration:", DateValue(deposit.RegistrationDate));
                                Field(c, "Date received: *", DateValue(deposit.ReceivedDate));
                                Field(c, "Auth evidence handling officer: *", deposit.HandlingOfficer);
                                TextArea(c, "Additional remarks:", deposit.Remarks);
                            });

                            row.ConstantItem(12);
                            row.RelativeItem().Column(c =>
                            {
                                Field(c, "Weapons/other items", "");
                                Field(c, "Weapons legal/illegal", "");
                                Check(c, "CO-Criminal offence:", deposit.IsCoCriminalOffence);
                                Check(c, "Gender-based violence:", deposit.IsGenderBasedViolence);
                                Field(c, "Confiscated weapon person gender:", deposit.SexLookup?.Name);
                                Field(c, "Confiscated weapon person age band:", deposit.AgeBandLookup?.Name);
                            });
                        });

                        col.Item().LineHorizontal(1.5f);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem(2).Text("Evidence description: *").SemiBold();
                            row.RelativeItem().Text("Type:");
                            row.RelativeItem().Text("Model:");
                        });

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            Header(table, "Evidence description");
                            Header(table, "Type");
                            Header(table, "Model");
                            Header(table, "Move ord no");
                            Header(table, "Move ord date");
                            Header(table, "New location");
                            Header(table, "Purpose");
                            Header(table, "Move date");

                            var items = deposit.Items.Where(x => !x.IsDeleted).ToList();

                            if (items.Any())
                            {
                                foreach (var item in items)
                                {
                                    Cell(table, item.Description);
                                    Cell(table, item.EvidenceWeaponTypeLookup?.Name);
                                    Cell(table, item.EvidenceWeaponLookup?.Name);
                                    Cell(table, "");
                                    Cell(table, "");
                                    Cell(table, "");
                                    Cell(table, "");
                                    Cell(table, "");
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 8; i++)
                                    Cell(table, "");
                            }
                            var moves = deposit.MoveHistories
                            .Where(x => !x.IsDeleted)
                            .OrderByDescending(x => x.MoveDate)
                            .ToList();

                            if (moves.Any())
                            {
                                col.Item().PaddingTop(12);

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

                        });

                        col.Item().MinHeight(55).Border(0.5f).BorderColor(Colors.Grey.Lighten1);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                Field(c, "Move order number: *", "");
                                Field(c, "New evidence deposit location: *", deposit.DepositLocationLookup?.Name);
                            });

                            row.ConstantItem(35);

                            row.RelativeItem().Column(c =>
                            {
                                Field(c, "Move order date: *", "");
                                Field(c, "Move evidence purpose: *", "");
                                Field(c, "Move date:", "");
                            });

                            row.RelativeItem();
                        });

                        col.Item().LineHorizontal(1.5f);

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(110).Text("Recorded by: *").SemiBold();
                            row.ConstantItem(170).Border(0.5f).Height(14).Text(
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

        private static void Field(ColumnDescriptor c, string label, string? value, bool boldValue = false)
        {
            c.Item().PaddingBottom(4).Row(row =>
            {
                row.RelativeItem(1.2f)
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

        private static void Check(ColumnDescriptor c, string label, bool value)
        {
            c.Item().PaddingBottom(5).Row(row =>
            {
                row.ConstantItem(170).Text(label).SemiBold();
                row.ConstantItem(13)
                    .Height(13)
                    .Border(0.5f)
                    .BorderColor(Colors.Grey.Lighten1)
                    .AlignCenter()
                    .AlignMiddle()
                    .Text(value ? "X" : "");
            });
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

        private static string DateValue(DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }
    }
}