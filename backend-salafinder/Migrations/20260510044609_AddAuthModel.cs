using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_salafinder.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioPerfil",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    identity_user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nombre_completo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    no_shows = table.Column<int>(type: "int", nullable: false),
                    bloqueado_hasta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPerfil", x => x.id);
                    table.ForeignKey(
                        name: "FK_UsuarioPerfil_AspNetUsers_identity_user_id",
                        column: x => x.identity_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPerfil_identity_user_id",
                table: "UsuarioPerfil",
                column: "identity_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioPerfil");
        }
    }
}
