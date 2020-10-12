using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantManagement.Data.Migrations
{
    public partial class editOfEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_PersonRole_UserRoleNavigationId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLog_Staff_ModifiedByNavigationId",
                table: "UserLog");

            migrationBuilder.DropIndex(
                name: "IX_UserLog_ModifiedByNavigationId",
                table: "UserLog");

            migrationBuilder.DropIndex(
                name: "IX_Staff_UserRoleNavigationId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "ModifiedByNavigationId",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "UserRoleNavigationId",
                table: "Staff");

            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "UserLog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PersonRoleId",
                table: "Staff",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserLog_StaffId",
                table: "UserLog",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_PersonRoleId",
                table: "Staff",
                column: "PersonRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_PersonRole_PersonRoleId",
                table: "Staff",
                column: "PersonRoleId",
                principalTable: "PersonRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLog_Staff_StaffId",
                table: "UserLog",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_PersonRole_PersonRoleId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLog_Staff_StaffId",
                table: "UserLog");

            migrationBuilder.DropIndex(
                name: "IX_UserLog_StaffId",
                table: "UserLog");

            migrationBuilder.DropIndex(
                name: "IX_Staff_PersonRoleId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "PersonRoleId",
                table: "Staff");

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "UserLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByNavigationId",
                table: "UserLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserRole",
                table: "Staff",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserRoleNavigationId",
                table: "Staff",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLog_ModifiedByNavigationId",
                table: "UserLog",
                column: "ModifiedByNavigationId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserLog_Staff_ModifiedByNavigationId",
                table: "UserLog",
                column: "ModifiedByNavigationId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
