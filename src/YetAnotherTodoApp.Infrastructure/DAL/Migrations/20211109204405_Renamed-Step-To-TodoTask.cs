using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherTodoApp.Infrastructure.DAL.Migrations
{
    public partial class RenamedStepToTodoTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Todos_TodoId",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Steps",
                table: "Steps");

            migrationBuilder.RenameTable(
                name: "Steps",
                newName: "TodoTasks");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_TodoId",
                table: "TodoTasks",
                newName: "IX_TodoTasks_TodoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTasks",
                table: "TodoTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Todos_TodoId",
                table: "TodoTasks",
                column: "TodoId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Todos_TodoId",
                table: "TodoTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTasks",
                table: "TodoTasks");

            migrationBuilder.RenameTable(
                name: "TodoTasks",
                newName: "Steps");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTasks_TodoId",
                table: "Steps",
                newName: "IX_Steps_TodoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Steps",
                table: "Steps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Todos_TodoId",
                table: "Steps",
                column: "TodoId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
