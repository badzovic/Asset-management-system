using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class AddWeaponsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calibers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calibers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeaponTypeId = table.Column<int>(type: "int", nullable: true),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true),
                    CaliberId = table.Column<int>(type: "int", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponModels_Calibers_CaliberId",
                        column: x => x.CaliberId,
                        principalTable: "Calibers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeaponModels_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeaponModels_WeaponTypes_WeaponTypeId",
                        column: x => x.WeaponTypeId,
                        principalTable: "WeaponTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FactorySerial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmSerial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeaponTypeId = table.Column<int>(type: "int", nullable: true),
                    WeaponModelId = table.Column<int>(type: "int", nullable: true),
                    CaliberId = table.Column<int>(type: "int", nullable: true),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true),
                    MarkLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovernmentAgency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginIndicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizacionaJedinicaId = table.Column<int>(type: "int", nullable: true),
                    SkladisteId = table.Column<int>(type: "int", nullable: true),
                    BookkeepingBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BarrelMark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlideMark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtstockMark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HolderInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfOwnership = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TempStock = table.Column<bool>(type: "bit", nullable: false),
                    DonationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DonorAgency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonorContractNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStatusId = table.Column<int>(type: "int", nullable: true),
                    FunctionalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMarked = table.Column<bool>(type: "bit", nullable: false),
                    IsProspective = table.Column<bool>(type: "bit", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_Calibers_CaliberId",
                        column: x => x.CaliberId,
                        principalTable: "Calibers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Weapons_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Weapons_OrganizacioneJedinice_OrganizacionaJedinicaId",
                        column: x => x.OrganizacionaJedinicaId,
                        principalTable: "OrganizacioneJedinice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Weapons_Skladista_SkladisteId",
                        column: x => x.SkladisteId,
                        principalTable: "Skladista",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Weapons_WeaponModels_WeaponModelId",
                        column: x => x.WeaponModelId,
                        principalTable: "WeaponModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Weapons_WeaponStatuses_CurrentStatusId",
                        column: x => x.CurrentStatusId,
                        principalTable: "WeaponStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Weapons_WeaponTypes_WeaponTypeId",
                        column: x => x.WeaponTypeId,
                        principalTable: "WeaponTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeaponModels_CaliberId",
                table: "WeaponModels",
                column: "CaliberId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponModels_ManufacturerId",
                table: "WeaponModels",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponModels_WeaponTypeId",
                table: "WeaponModels",
                column: "WeaponTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CaliberId",
                table: "Weapons",
                column: "CaliberId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CurrentStatusId",
                table: "Weapons",
                column: "CurrentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_ManufacturerId",
                table: "Weapons",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_OrganizacionaJedinicaId",
                table: "Weapons",
                column: "OrganizacionaJedinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_SkladisteId",
                table: "Weapons",
                column: "SkladisteId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_WeaponModelId",
                table: "Weapons",
                column: "WeaponModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_WeaponTypeId",
                table: "Weapons",
                column: "WeaponTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "WeaponModels");

            migrationBuilder.DropTable(
                name: "WeaponStatuses");

            migrationBuilder.DropTable(
                name: "Calibers");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "WeaponTypes");
        }
    }
}
