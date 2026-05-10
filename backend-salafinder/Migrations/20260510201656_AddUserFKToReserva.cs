using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_salafinder.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFKToReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "id_usuario",
                table: "Reserva",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_id_usuario",
                table: "Reserva",
                column: "id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_UsuarioPerfil_id_usuario",
                table: "Reserva",
                column: "id_usuario",
                principalTable: "UsuarioPerfil",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_UsuarioPerfil_id_usuario",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_id_usuario",
                table: "Reserva");

            migrationBuilder.DropColumn(
                name: "id_usuario",
                table: "Reserva");
        }
    }
}
