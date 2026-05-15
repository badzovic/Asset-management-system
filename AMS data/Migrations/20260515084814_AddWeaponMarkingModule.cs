using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class AddWeaponMarkingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarkingLayouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LayoutType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviewImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkingLayouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponMarkingJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeaponId = table.Column<int>(type: "int", nullable: false),
                    MarkingLayoutId = table.Column<int>(type: "int", nullable: true),
                    JobDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FactorySerial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeaponModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeaponType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Caliber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarkingText1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarkingText2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarkingText3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataMatrixValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutputJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponMarkingJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponMarkingJobs_MarkingLayouts_MarkingLayoutId",
                        column: x => x.MarkingLayoutId,
                        principalTable: "MarkingLayouts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeaponMarkingJobs_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeaponMarkingJobs_MarkingLayoutId",
                table: "WeaponMarkingJobs",
                column: "MarkingLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponMarkingJobs_WeaponId",
                table: "WeaponMarkingJobs",
                column: "WeaponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeaponMarkingJobs");

            migrationBuilder.DropTable(
                name: "MarkingLayouts");
        }
    }
}
