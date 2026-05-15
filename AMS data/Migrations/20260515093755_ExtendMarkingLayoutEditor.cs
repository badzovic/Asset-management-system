using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS_data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendMarkingLayoutEditor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundFilePath",
                table: "MarkingLayouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HeightMm",
                table: "MarkingLayouts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "LayoutJson",
                table: "MarkingLayouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "MarkingLayouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "WidthMm",
                table: "MarkingLayouts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "MarkingLayoutObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkingLayoutId = table.Column<int>(type: "int", nullable: false),
                    ObjectType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    X = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Y = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rotation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TextValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VariableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FontSize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsBold = table.Column<bool>(type: "bit", nullable: false),
                    StrokeWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PropertiesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkingLayoutObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkingLayoutObjects_MarkingLayouts_MarkingLayoutId",
                        column: x => x.MarkingLayoutId,
                        principalTable: "MarkingLayouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarkingLayoutObjects_MarkingLayoutId",
                table: "MarkingLayoutObjects",
                column: "MarkingLayoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarkingLayoutObjects");

            migrationBuilder.DropColumn(
                name: "BackgroundFilePath",
                table: "MarkingLayouts");

            migrationBuilder.DropColumn(
                name: "HeightMm",
                table: "MarkingLayouts");

            migrationBuilder.DropColumn(
                name: "LayoutJson",
                table: "MarkingLayouts");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "MarkingLayouts");

            migrationBuilder.DropColumn(
                name: "WidthMm",
                table: "MarkingLayouts");
        }
    }
}
