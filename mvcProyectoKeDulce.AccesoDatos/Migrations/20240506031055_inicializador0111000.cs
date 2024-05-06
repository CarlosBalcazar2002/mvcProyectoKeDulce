using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvcProyectoKeDulce.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class inicializador0111000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Pedido",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pedido");
        }
    }
}
