using Microsoft.EntityFrameworkCore.Migrations;

namespace YetAnotherTodoApp.Infrastructure.DAL.Migrations
{
    public partial class AddedValueObjectes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Steps");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "Users",
                newName: "Password_Salt");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "Name_LastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "Name_FirstName");

            migrationBuilder.AddColumn<string>(
                name: "Email_Value",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password_Hash",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username_Value",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title_Value",
                table: "Todos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title_Value",
                table: "TodoLists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title_Value",
                table: "Steps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email_Value",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password_Hash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username_Value",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Title_Value",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Title_Value",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "Title_Value",
                table: "Steps");

            migrationBuilder.RenameColumn(
                name: "Password_Salt",
                table: "Users",
                newName: "Salt");

            migrationBuilder.RenameColumn(
                name: "Name_LastName",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name_FirstName",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Todos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TodoLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Steps",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
