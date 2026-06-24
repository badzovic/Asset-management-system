using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class AddWeaponTeamAndIdTypeOtherText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdTypeOtherText",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_TeamLookupId",
                table: "Weapons",
                column: "TeamLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_TeamLookupId",
                table: "Weapons",
                column: "TeamLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_TeamLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_TeamLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "IdTypeOtherText",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "TeamLookupId",
                table: "Weapons");
        }
    }
}
