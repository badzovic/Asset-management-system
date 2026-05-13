using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendLookupItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DoNotDelete",
                table: "LookupItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "LookupItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentLookupItemId",
                table: "LookupItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserDefinedSort",
                table: "LookupItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_LookupItems_ParentLookupItemId",
                table: "LookupItems",
                column: "ParentLookupItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LookupItems_LookupItems_ParentLookupItemId",
                table: "LookupItems",
                column: "ParentLookupItemId",
                principalTable: "LookupItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LookupItems_LookupItems_ParentLookupItemId",
                table: "LookupItems");

            migrationBuilder.DropIndex(
                name: "IX_LookupItems_ParentLookupItemId",
                table: "LookupItems");

            migrationBuilder.DropColumn(
                name: "DoNotDelete",
                table: "LookupItems");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "LookupItems");

            migrationBuilder.DropColumn(
                name: "ParentLookupItemId",
                table: "LookupItems");

            migrationBuilder.DropColumn(
                name: "UserDefinedSort",
                table: "LookupItems");
        }
    }
}
