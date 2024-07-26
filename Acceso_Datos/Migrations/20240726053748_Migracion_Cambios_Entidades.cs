using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acceso_Datos.Migrations
{
    public partial class Migracion_Cambios_Entidades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreProducto",
                table: "DetalleFacturas");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioProducto",
                table: "DetalleFacturas",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "IdProductoEnDetalle",
                table: "DetalleFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_IdProductoEnDetalle",
                table: "DetalleFacturas",
                column: "IdProductoEnDetalle");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFacturas_Productos_IdProductoEnDetalle",
                table: "DetalleFacturas",
                column: "IdProductoEnDetalle",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFacturas_Productos_IdProductoEnDetalle",
                table: "DetalleFacturas");

            migrationBuilder.DropIndex(
                name: "IX_DetalleFacturas_IdProductoEnDetalle",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "IdProductoEnDetalle",
                table: "DetalleFacturas");

            migrationBuilder.AlterColumn<double>(
                name: "PrecioProducto",
                table: "DetalleFacturas",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<string>(
                name: "NombreProducto",
                table: "DetalleFacturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
