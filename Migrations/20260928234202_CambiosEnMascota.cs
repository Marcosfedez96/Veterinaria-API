using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veterinaria_API.Migrations
{
    /// <inheritdoc />
    public partial class CambiosEnMascota : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Veterinarios_VeterianarioId",
                table: "Turnos");

            migrationBuilder.RenameColumn(
                name: "VeterianarioId",
                table: "Turnos",
                newName: "VeterinarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Turnos_VeterianarioId",
                table: "Turnos",
                newName: "IX_Turnos_VeterinarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Veterinarios_VeterinarioId",
                table: "Turnos",
                column: "VeterinarioId",
                principalTable: "Veterinarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Veterinarios_VeterinarioId",
                table: "Turnos");

            migrationBuilder.RenameColumn(
                name: "VeterinarioId",
                table: "Turnos",
                newName: "VeterianarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Turnos_VeterinarioId",
                table: "Turnos",
                newName: "IX_Turnos_VeterianarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Veterinarios_VeterianarioId",
                table: "Turnos",
                column: "VeterianarioId",
                principalTable: "Veterinarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
