using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class AddWeaponMoves : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeaponMoves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeaponId = table.Column<int>(type: "int", nullable: false),
                    MoveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovementActionLookupId = table.Column<int>(type: "int", nullable: true),
                    NewLocationLookupId = table.Column<int>(type: "int", nullable: true),
                    OrderNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthMoveNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoveOrdinalNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndUserCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserOrgName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreparedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreparedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorisedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorisedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorisedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponMoves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponMoves_LookupItems_MovementActionLookupId",
                        column: x => x.MovementActionLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeaponMoves_LookupItems_NewLocationLookupId",
                        column: x => x.NewLocationLookupId,
                        principalTable: "LookupItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeaponMoves_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeaponMoves_MovementActionLookupId",
                table: "WeaponMoves",
                column: "MovementActionLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponMoves_NewLocationLookupId",
                table: "WeaponMoves",
                column: "NewLocationLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponMoves_WeaponId",
                table: "WeaponMoves",
                column: "WeaponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeaponMoves");
        }
    }
}
