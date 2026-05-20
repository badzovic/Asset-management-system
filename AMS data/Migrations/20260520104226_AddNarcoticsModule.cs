using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class AddNarcoticsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "EvidenceDeposits",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "NarcoticsDeposits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseTypeLookupId = table.Column<int>(type: "int", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OUPerformedSeizureLookupId = table.Column<int>(type: "int", nullable: true),
                    StorageOrderNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageOrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LinkToOrderNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmStorageOrderNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositLocationLookupId = table.Column<int>(type: "int", nullable: true),
                    EvidenceIndicatorLookupId = table.Column<int>(type: "int", nullable: true),
                    SubmittedByOfficer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HandlingOfficer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForensicReportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForensicReportDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerdictNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerdictDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DestructionOrderNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestructionOrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DestructionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalIdNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseInfoFolderPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuspended = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarcoticsDeposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NarcoticsDeposits_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsDeposits_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsDeposits_LookupItems_CaseTypeLookupId",
                        column: x => x.CaseTypeLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsDeposits_LookupItems_DepositLocationLookupId",
                        column: x => x.DepositLocationLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsDeposits_LookupItems_EvidenceIndicatorLookupId",
                        column: x => x.EvidenceIndicatorLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsDeposits_LookupItems_OUPerformedSeizureLookupId",
                        column: x => x.OUPerformedSeizureLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NarcoticsDepositItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NarcoticsDepositId = table.Column<int>(type: "int", nullable: false),
                    NarcoticsTypeLookupId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuantityUnitLookupId = table.Column<int>(type: "int", nullable: true),
                    CompositionLookupId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarcoticsDepositItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NarcoticsDepositItems_LookupItems_CompositionLookupId",
                        column: x => x.CompositionLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsDepositItems_LookupItems_NarcoticsTypeLookupId",
                        column: x => x.NarcoticsTypeLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsDepositItems_LookupItems_QuantityUnitLookupId",
                        column: x => x.QuantityUnitLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsDepositItems_NarcoticsDeposits_NarcoticsDepositId",
                        column: x => x.NarcoticsDepositId,
                        principalTable: "NarcoticsDeposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NarcoticsMoveHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NarcoticsDepositId = table.Column<int>(type: "int", nullable: false),
                    NarcoticsDepositItemId = table.Column<int>(type: "int", nullable: true),
                    FromLocationLookupId = table.Column<int>(type: "int", nullable: true),
                    ToLocationLookupId = table.Column<int>(type: "int", nullable: true),
                    MovePurposeLookupId = table.Column<int>(type: "int", nullable: true),
                    MoveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarcoticsMoveHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NarcoticsMoveHistories_LookupItems_FromLocationLookupId",
                        column: x => x.FromLocationLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsMoveHistories_LookupItems_MovePurposeLookupId",
                        column: x => x.MovePurposeLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsMoveHistories_LookupItems_ToLocationLookupId",
                        column: x => x.ToLocationLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsMoveHistories_NarcoticsDepositItems_NarcoticsDepositItemId",
                        column: x => x.NarcoticsDepositItemId,
                        principalTable: "NarcoticsDepositItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NarcoticsMoveHistories_NarcoticsDeposits_NarcoticsDepositId",
                        column: x => x.NarcoticsDepositId,
                        principalTable: "NarcoticsDeposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDeposits_CreatedBy",
                table: "EvidenceDeposits",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDepositItems_CompositionLookupId",
                table: "NarcoticsDepositItems",
                column: "CompositionLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDepositItems_NarcoticsDepositId",
                table: "NarcoticsDepositItems",
                column: "NarcoticsDepositId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDepositItems_NarcoticsTypeLookupId",
                table: "NarcoticsDepositItems",
                column: "NarcoticsTypeLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDepositItems_QuantityUnitLookupId",
                table: "NarcoticsDepositItems",
                column: "QuantityUnitLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDeposits_CaseTypeLookupId",
                table: "NarcoticsDeposits",
                column: "CaseTypeLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDeposits_CreatedBy",
                table: "NarcoticsDeposits",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDeposits_DepositLocationLookupId",
                table: "NarcoticsDeposits",
                column: "DepositLocationLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDeposits_EvidenceIndicatorLookupId",
                table: "NarcoticsDeposits",
                column: "EvidenceIndicatorLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDeposits_OUPerformedSeizureLookupId",
                table: "NarcoticsDeposits",
                column: "OUPerformedSeizureLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsDeposits_UpdatedBy",
                table: "NarcoticsDeposits",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsMoveHistories_FromLocationLookupId",
                table: "NarcoticsMoveHistories",
                column: "FromLocationLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsMoveHistories_MovePurposeLookupId",
                table: "NarcoticsMoveHistories",
                column: "MovePurposeLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsMoveHistories_NarcoticsDepositId",
                table: "NarcoticsMoveHistories",
                column: "NarcoticsDepositId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsMoveHistories_NarcoticsDepositItemId",
                table: "NarcoticsMoveHistories",
                column: "NarcoticsDepositItemId");

            migrationBuilder.CreateIndex(
                name: "IX_NarcoticsMoveHistories_ToLocationLookupId",
                table: "NarcoticsMoveHistories",
                column: "ToLocationLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvidenceDeposits_AspNetUsers_CreatedBy",
                table: "EvidenceDeposits",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvidenceDeposits_AspNetUsers_CreatedBy",
                table: "EvidenceDeposits");

            migrationBuilder.DropTable(
                name: "NarcoticsMoveHistories");

            migrationBuilder.DropTable(
                name: "NarcoticsDepositItems");

            migrationBuilder.DropTable(
                name: "NarcoticsDeposits");

            migrationBuilder.DropIndex(
                name: "IX_EvidenceDeposits_CreatedBy",
                table: "EvidenceDeposits");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "EvidenceDeposits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
