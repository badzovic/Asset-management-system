using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorManufacturerCountryLookup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryLookupId",
                table: "Manufacturers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_CountryLookupId",
                table: "Manufacturers",
                column: "CountryLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_LookupItems_CountryLookupId",
                table: "Manufacturers",
                column: "CountryLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_LookupItems_CountryLookupId",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_CountryLookupId",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "CountryLookupId",
                table: "Manufacturers");
        }
    }
}
