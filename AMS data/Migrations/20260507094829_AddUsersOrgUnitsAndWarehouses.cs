using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersOrgUnitsAndWarehouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktivan",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumKreiranja",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Ime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizacionaJedinicaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prezime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkladisteId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrganizacioneJedinice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sifra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktivna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacioneJedinice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skladista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sifra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizacionaJedinicaId = table.Column<int>(type: "int", nullable: true),
                    Aktivno = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skladista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skladista_OrganizacioneJedinice_OrganizacionaJedinicaId",
                        column: x => x.OrganizacionaJedinicaId,
                        principalTable: "OrganizacioneJedinice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skladista_OrganizacionaJedinicaId",
                table: "Skladista",
                column: "OrganizacionaJedinicaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skladista");

            migrationBuilder.DropTable(
                name: "OrganizacioneJedinice");

            migrationBuilder.DropColumn(
                name: "Aktivan",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DatumKreiranja",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganizacionaJedinicaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Prezime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SkladisteId",
                table: "AspNetUsers");
        }
    }
}
