using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasky.Migrations
{
    public partial class usertotask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_ApplicationUserId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Tasks",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ApplicationUserId",
                table: "Tasks",
                newName: "IX_Tasks_UserId1");

            migrationBuilder.AlterColumn<string>(
                name: "Descriere",
                table: "Tasks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tasks",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId1",
                table: "Tasks",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserProjects_Tasks_TaskId",
                table: "ApplicationUserProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId1",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserProjects_TaskId",
                table: "ApplicationUserProjects");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "ApplicationUserProjects");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Tasks",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserId1",
                table: "Tasks",
                newName: "IX_Tasks_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Descriere",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_ApplicationUserId",
                table: "Tasks",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
