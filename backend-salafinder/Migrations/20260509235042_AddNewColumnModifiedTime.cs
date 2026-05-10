using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_salafinder.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnModifiedTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ultima_vez_modificado",
                table: "Espacio",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ultima_vez_modificado",
                table: "Espacio");
        }
    }
}
