using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherTodoApp.Infrastructure.DAL.Migrations
{
    public partial class ConfiguredEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Todos_TodoId",
                table: "Steps");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_Users_UserId",
                table: "TodoLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoLists_TodoListId",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "TodoListId",
                table: "Todos",
                newName: "todoListId");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_TodoListId",
                table: "Todos",
                newName: "IX_Todos_todoListId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TodoLists",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLists_UserId",
                table: "TodoLists",
                newName: "IX_TodoLists_userId");

            migrationBuilder.RenameColumn(
                name: "TodoId",
                table: "Steps",
                newName: "todoId");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_TodoId",
                table: "Steps",
                newName: "IX_Steps_todoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Todos_todoId",
                table: "Steps",
                column: "todoId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Users_userId",
                table: "TodoLists",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoLists_todoListId",
                table: "Todos",
                column: "todoListId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Todos_todoId",
                table: "Steps");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_Users_userId",
                table: "TodoLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoLists_todoListId",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "todoListId",
                table: "Todos",
                newName: "TodoListId");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_todoListId",
                table: "Todos",
                newName: "IX_Todos_TodoListId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "TodoLists",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLists_userId",
                table: "TodoLists",
                newName: "IX_TodoLists_UserId");

            migrationBuilder.RenameColumn(
                name: "todoId",
                table: "Steps",
                newName: "TodoId");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_todoId",
                table: "Steps",
                newName: "IX_Steps_TodoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Todos_TodoId",
                table: "Steps",
                column: "TodoId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Users_UserId",
                table: "TodoLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoLists_TodoListId",
                table: "Todos",
                column: "TodoListId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
