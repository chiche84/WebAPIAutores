using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPIAutores1.Migrations
{
    public partial class usuarioscomentarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Comentarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "Comentarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UsuarioId1",
                table: "Comentarios",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_AspNetUsers_UsuarioId1",
                table: "Comentarios",
                column: "UsuarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_AspNetUsers_UsuarioId1",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_UsuarioId1",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Comentarios");
        }
    }
}
