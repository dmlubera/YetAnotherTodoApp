using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherTodoApp.Infrastructure.DAL.Migrations
{
    public partial class EntityRefactoringMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Username_Value",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Password_Salt",
                table: "Users",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "Password_Hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Name_LastName",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name_FirstName",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Email_Value",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "todoListId",
                table: "Todos",
                newName: "TodoListId");

            migrationBuilder.RenameColumn(
                name: "Title_Value",
                table: "Todos",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_todoListId",
                table: "Todos",
                newName: "IX_Todos_TodoListId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "TodoLists",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Title_Value",
                table: "TodoLists",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLists_userId",
                table: "TodoLists",
                newName: "IX_TodoLists_UserId");

            migrationBuilder.RenameColumn(
                name: "todoId",
                table: "Steps",
                newName: "TodoId");

            migrationBuilder.RenameColumn(
                name: "Title_Value",
                table: "Steps",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_todoId",
                table: "Steps",
                newName: "IX_Steps_TodoId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishDate",
                table: "Todos",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Todos_TodoId",
                table: "Steps",
                column: "TodoId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Users_UserId",
                table: "TodoLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoLists_TodoListId",
                table: "Todos",
                column: "TodoListId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Username",
                table: "Users",
                newName: "Username_Value");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "Password_Salt");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password_Hash");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "Name_LastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "Name_FirstName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Email_Value");

            migrationBuilder.RenameColumn(
                name: "TodoListId",
                table: "Todos",
                newName: "todoListId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Todos",
                newName: "Title_Value");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_TodoListId",
                table: "Todos",
                newName: "IX_Todos_todoListId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TodoLists",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TodoLists",
                newName: "Title_Value");

            migrationBuilder.RenameIndex(
                name: "IX_TodoLists_UserId",
                table: "TodoLists",
                newName: "IX_TodoLists_userId");

            migrationBuilder.RenameColumn(
                name: "TodoId",
                table: "Steps",
                newName: "todoId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Steps",
                newName: "Title_Value");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_TodoId",
                table: "Steps",
                newName: "IX_Steps_todoId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishDate",
                table: "Todos",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

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
    }
}
