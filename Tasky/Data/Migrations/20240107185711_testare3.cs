using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasky.Migrations
{
    public partial class testare3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProjects_Tasks_TaskId",
                table: "ApplicationUserProjects");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserProjects_TaskId",
                table: "ApplicationUserProjects");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "ApplicationUserProjects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "ApplicationUserProjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProjects_TaskId",
                table: "ApplicationUserProjects",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserProjects_Tasks_TaskId",
                table: "ApplicationUserProjects",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
