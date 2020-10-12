using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantManagement.Data.Migrations
{
    public partial class roleAndDecimalAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserRole",
                table: "Staff",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "UserRoleNavigationId",
                table: "Staff",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "StockAmount",
                table: "Product",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Portion",
                table: "DishProduct",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "PersonRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRole", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserRoleNavigationId",
                table: "Staff",
                column: "UserRoleNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_PersonRole_UserRoleNavigationId",
                table: "Staff",
                column: "UserRoleNavigationId",
                principalTable: "PersonRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_PersonRole_UserRoleNavigationId",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "PersonRole");

            migrationBuilder.DropIndex(
                name: "IX_Staff_UserRoleNavigationId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "UserRoleNavigationId",
                table: "Staff");

            migrationBuilder.AlterColumn<string>(
                name: "UserRole",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "StockAmount",
                table: "Product",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Portion",
                table: "DishProduct",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");
        }
    }
}
