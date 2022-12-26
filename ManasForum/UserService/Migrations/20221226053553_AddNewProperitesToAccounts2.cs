using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    public partial class AddNewProperitesToAccounts2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Accounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Accounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Years",
                table: "Accounts",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Years",
                table: "Accounts");
        }
    }
}
