using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantManagement.Data.Migrations
{
    public partial class unitOfMeasurementAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "UnitOfMeasurementId",
                table: "Product",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UnitOfMeasurement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(nullable: true),
                    UnitDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasurement", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_UnitOfMeasurementId",
                table: "Product",
                column: "UnitOfMeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_UnitOfMeasurement_UnitOfMeasurementId",
                table: "Product",
                column: "UnitOfMeasurementId",
                principalTable: "UnitOfMeasurement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_UnitOfMeasurement_UnitOfMeasurementId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "UnitOfMeasurement");

            migrationBuilder.DropIndex(
                name: "IX_Product_UnitOfMeasurementId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasurementId",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "UnitOfMeasure",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
