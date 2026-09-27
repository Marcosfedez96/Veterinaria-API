using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veterinaria_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDueño : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TutorId",
                table: "Mascotas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tutores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mascotas_TutorId",
                table: "Mascotas",
                column: "TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mascotas_Tutores_TutorId",
                table: "Mascotas",
                column: "TutorId",
                principalTable: "Tutores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mascotas_Tutores_TutorId",
                table: "Mascotas");

            migrationBuilder.DropTable(
                name: "Tutores");

            migrationBuilder.DropIndex(
                name: "IX_Mascotas_TutorId",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "TutorId",
                table: "Mascotas");
        }
    }
}
