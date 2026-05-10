using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_salafinder.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReservaFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Espacio_id",
                table: "Reserva");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_id_espacio",
                table: "Reserva",
                column: "id_espacio");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Espacio_id_espacio",
                table: "Reserva",
                column: "id_espacio",
                principalTable: "Espacio",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Espacio_id_espacio",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_id_espacio",
                table: "Reserva");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Espacio_id",
                table: "Reserva",
                column: "id",
                principalTable: "Espacio",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
