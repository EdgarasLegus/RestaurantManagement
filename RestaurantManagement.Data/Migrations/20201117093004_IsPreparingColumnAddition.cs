using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantManagement.Data.Migrations
{
    public partial class IsPreparingColumnAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPreparing",
                table: "Order",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPreparing",
                table: "Order");
        }
    }
}
