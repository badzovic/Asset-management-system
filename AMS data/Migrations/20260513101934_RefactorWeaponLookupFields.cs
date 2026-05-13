using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorWeaponLookupFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_OrganizacioneJedinice_OrganizacionaJedinicaId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Skladista_SkladisteId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "BookkeepingBy",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "DonorAgency",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "FunctionalStatus",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "GovernmentAgency",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "MarkLocation",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "OriginIndicator",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "OriginalLocation",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "OriginalState",
                table: "Weapons");

            migrationBuilder.RenameColumn(
                name: "SkladisteId",
                table: "Weapons",
                newName: "WeaponStateLookupId");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "Weapons",
                newName: "ManufactureDate");

            migrationBuilder.RenameColumn(
                name: "OrganizacionaJedinicaId",
                table: "Weapons",
                newName: "UnitLookupId");

            migrationBuilder.RenameIndex(
                name: "IX_Weapons_SkladisteId",
                table: "Weapons",
                newName: "IX_Weapons_WeaponStateLookupId");

            migrationBuilder.RenameIndex(
                name: "IX_Weapons_OrganizacionaJedinicaId",
                table: "Weapons",
                newName: "IX_Weapons_UnitLookupId");

            migrationBuilder.AddColumn<int>(
                name: "BookkeepingByLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonorAgencyLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernmentAgencyLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdTypeLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManufactureCountryLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MarkLocationLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginIndicatorLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginalLocationLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginalStateLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockLookupId",
                table: "Weapons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_BookkeepingByLookupId",
                table: "Weapons",
                column: "BookkeepingByLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CountryLookupId",
                table: "Weapons",
                column: "CountryLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_DonorAgencyLookupId",
                table: "Weapons",
                column: "DonorAgencyLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_GovernmentAgencyLookupId",
                table: "Weapons",
                column: "GovernmentAgencyLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_IdTypeLookupId",
                table: "Weapons",
                column: "IdTypeLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_ManufactureCountryLookupId",
                table: "Weapons",
                column: "ManufactureCountryLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_MarkLocationLookupId",
                table: "Weapons",
                column: "MarkLocationLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_OriginalLocationLookupId",
                table: "Weapons",
                column: "OriginalLocationLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_OriginalStateLookupId",
                table: "Weapons",
                column: "OriginalStateLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_OriginIndicatorLookupId",
                table: "Weapons",
                column: "OriginIndicatorLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_RegionLookupId",
                table: "Weapons",
                column: "RegionLookupId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_StockLookupId",
                table: "Weapons",
                column: "StockLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_BookkeepingByLookupId",
                table: "Weapons",
                column: "BookkeepingByLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_CountryLookupId",
                table: "Weapons",
                column: "CountryLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_DonorAgencyLookupId",
                table: "Weapons",
                column: "DonorAgencyLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_GovernmentAgencyLookupId",
                table: "Weapons",
                column: "GovernmentAgencyLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_IdTypeLookupId",
                table: "Weapons",
                column: "IdTypeLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_ManufactureCountryLookupId",
                table: "Weapons",
                column: "ManufactureCountryLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_MarkLocationLookupId",
                table: "Weapons",
                column: "MarkLocationLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_OriginIndicatorLookupId",
                table: "Weapons",
                column: "OriginIndicatorLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_OriginalLocationLookupId",
                table: "Weapons",
                column: "OriginalLocationLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_OriginalStateLookupId",
                table: "Weapons",
                column: "OriginalStateLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_RegionLookupId",
                table: "Weapons",
                column: "RegionLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_StockLookupId",
                table: "Weapons",
                column: "StockLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_UnitLookupId",
                table: "Weapons",
                column: "UnitLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_LookupItems_WeaponStateLookupId",
                table: "Weapons",
                column: "WeaponStateLookupId",
                principalTable: "LookupItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_BookkeepingByLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_CountryLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_DonorAgencyLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_GovernmentAgencyLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_IdTypeLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_ManufactureCountryLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_MarkLocationLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_OriginIndicatorLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_OriginalLocationLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_OriginalStateLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_RegionLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_StockLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_UnitLookupId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_LookupItems_WeaponStateLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_BookkeepingByLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_CountryLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_DonorAgencyLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_GovernmentAgencyLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_IdTypeLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_ManufactureCountryLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_MarkLocationLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_OriginalLocationLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_OriginalStateLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_OriginIndicatorLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_RegionLookupId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_StockLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "BookkeepingByLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "CountryLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "DonorAgencyLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "GovernmentAgencyLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "IdTypeLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "ManufactureCountryLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "MarkLocationLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "OriginIndicatorLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "OriginalLocationLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "OriginalStateLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "RegionLookupId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "StockLookupId",
                table: "Weapons");

            migrationBuilder.RenameColumn(
                name: "WeaponStateLookupId",
                table: "Weapons",
                newName: "SkladisteId");

            migrationBuilder.RenameColumn(
                name: "UnitLookupId",
                table: "Weapons",
                newName: "OrganizacionaJedinicaId");

            migrationBuilder.RenameColumn(
                name: "ManufactureDate",
                table: "Weapons",
                newName: "Region");

            migrationBuilder.RenameIndex(
                name: "IX_Weapons_WeaponStateLookupId",
                table: "Weapons",
                newName: "IX_Weapons_SkladisteId");

            migrationBuilder.RenameIndex(
                name: "IX_Weapons_UnitLookupId",
                table: "Weapons",
                newName: "IX_Weapons_OrganizacionaJedinicaId");

            migrationBuilder.AddColumn<string>(
                name: "BookkeepingBy",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DonorAgency",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FunctionalStatus",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GovernmentAgency",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarkLocation",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginIndicator",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalLocation",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalState",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_OrganizacioneJedinice_OrganizacionaJedinicaId",
                table: "Weapons",
                column: "OrganizacionaJedinicaId",
                principalTable: "OrganizacioneJedinice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Skladista_SkladisteId",
                table: "Weapons",
                column: "SkladisteId",
                principalTable: "Skladista",
                principalColumn: "Id");
        }
    }
}
