using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class AddEvidenceMoveHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvidenceMoveHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvidenceDepositId = table.Column<int>(type: "int", nullable: false),
                    EvidenceDepositItemId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_EvidenceMoveHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvidenceMoveHistories_EvidenceDepositItems_EvidenceDepositItemId",
                        column: x => x.EvidenceDepositItemId,
                        principalTable: "EvidenceDepositItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceMoveHistories_EvidenceDeposits_EvidenceDepositId",
                        column: x => x.EvidenceDepositId,
                        principalTable: "EvidenceDeposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvidenceMoveHistories_LookupItems_FromLocationLookupId",
                        column: x => x.FromLocationLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceMoveHistories_LookupItems_MovePurposeLookupId",
                        column: x => x.MovePurposeLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvidenceMoveHistories_LookupItems_ToLocationLookupId",
                        column: x => x.ToLocationLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceMoveHistories_EvidenceDepositId",
                table: "EvidenceMoveHistories",
                column: "EvidenceDepositId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceMoveHistories_EvidenceDepositItemId",
                table: "EvidenceMoveHistories",
                column: "EvidenceDepositItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceMoveHistories_FromLocationLookupId",
                table: "EvidenceMoveHistories",
                column: "FromLocationLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceMoveHistories_MovePurposeLookupId",
                table: "EvidenceMoveHistories",
                column: "MovePurposeLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceMoveHistories_ToLocationLookupId",
                table: "EvidenceMoveHistories",
                column: "ToLocationLookupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvidenceMoveHistories");
        }
    }
}
