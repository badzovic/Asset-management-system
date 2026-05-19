using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class AddEvidenceDepositModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvidenceDeposits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseTypeLookupId = table.Column<int>(type: "int", nullable: true),
                    EvidenceIndicatorLookupId = table.Column<int>(type: "int", nullable: true),
                    DepositLocationLookupId = table.Column<int>(type: "int", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StorageOrderNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageOrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedByOfficer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HandlingOfficer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalIdNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SexLookupId = table.Column<int>(type: "int", nullable: true),
                    AgeBandLookupId = table.Column<int>(type: "int", nullable: true),
                    IsCoCriminalOffence = table.Column<bool>(type: "bit", nullable: false),
                    IsGenderBasedViolence = table.Column<bool>(type: "bit", nullable: false),
                    CaseInfoFolderPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuspended = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceDeposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvidenceDeposits_LookupItems_AgeBandLookupId",
                        column: x => x.AgeBandLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceDeposits_LookupItems_CaseTypeLookupId",
                        column: x => x.CaseTypeLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceDeposits_LookupItems_DepositLocationLookupId",
                        column: x => x.DepositLocationLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceDeposits_LookupItems_EvidenceIndicatorLookupId",
                        column: x => x.EvidenceIndicatorLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceDeposits_LookupItems_SexLookupId",
                        column: x => x.SexLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EvidenceDepositItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvidenceDepositId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeaponItemTypeLookupId = table.Column<int>(type: "int", nullable: true),
                    EvidenceWeaponTypeLookupId = table.Column<int>(type: "int", nullable: true),
                    EvidenceWeaponLookupId = table.Column<int>(type: "int", nullable: true),
                    WeaponLegalityLookupId = table.Column<int>(type: "int", nullable: true),
                    LinkedWeaponId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarkingText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceDepositItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvidenceDepositItems_EvidenceDeposits_EvidenceDepositId",
                        column: x => x.EvidenceDepositId,
                        principalTable: "EvidenceDeposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvidenceDepositItems_LookupItems_EvidenceWeaponLookupId",
                        column: x => x.EvidenceWeaponLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceDepositItems_LookupItems_EvidenceWeaponTypeLookupId",
                        column: x => x.EvidenceWeaponTypeLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceDepositItems_LookupItems_WeaponItemTypeLookupId",
                        column: x => x.WeaponItemTypeLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceDepositItems_LookupItems_WeaponLegalityLookupId",
                        column: x => x.WeaponLegalityLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceDepositItems_Weapons_LinkedWeaponId",
                        column: x => x.LinkedWeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDepositItems_EvidenceDepositId",
                table: "EvidenceDepositItems",
                column: "EvidenceDepositId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDepositItems_EvidenceWeaponLookupId",
                table: "EvidenceDepositItems",
                column: "EvidenceWeaponLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDepositItems_EvidenceWeaponTypeLookupId",
                table: "EvidenceDepositItems",
                column: "EvidenceWeaponTypeLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDepositItems_LinkedWeaponId",
                table: "EvidenceDepositItems",
                column: "LinkedWeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDepositItems_WeaponItemTypeLookupId",
                table: "EvidenceDepositItems",
                column: "WeaponItemTypeLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDepositItems_WeaponLegalityLookupId",
                table: "EvidenceDepositItems",
                column: "WeaponLegalityLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDeposits_AgeBandLookupId",
                table: "EvidenceDeposits",
                column: "AgeBandLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDeposits_CaseTypeLookupId",
                table: "EvidenceDeposits",
                column: "CaseTypeLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDeposits_DepositLocationLookupId",
                table: "EvidenceDeposits",
                column: "DepositLocationLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDeposits_EvidenceIndicatorLookupId",
                table: "EvidenceDeposits",
                column: "EvidenceIndicatorLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceDeposits_SexLookupId",
                table: "EvidenceDeposits",
                column: "SexLookupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvidenceDepositItems");

            migrationBuilder.DropTable(
                name: "EvidenceDeposits");
        }
    }
}
