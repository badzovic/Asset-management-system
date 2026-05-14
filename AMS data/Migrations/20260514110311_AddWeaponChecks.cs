using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class AddWeaponChecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeaponChecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeaponId = table.Column<int>(type: "int", nullable: false),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckStateLookupId = table.Column<int>(type: "int", nullable: true),
                    IdNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponChecks_LookupItems_CheckStateLookupId",
                        column: x => x.CheckStateLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeaponChecks_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeaponChecks_CheckStateLookupId",
                table: "WeaponChecks",
                column: "CheckStateLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponChecks_WeaponId",
                table: "WeaponChecks",
                column: "WeaponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeaponChecks");
        }
    }
}
