using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class DeletePrivileges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACCOUNTPRIVILEGE",
                table: "ACCOUNT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ACCOUNTPRIVILEGE",
                table: "ACCOUNT",
                type: "VARCHAR2(1)",
                unicode: false,
                maxLength: 1,
                nullable: false,
                defaultValueSql: "'n' ");
        }
    }
}
