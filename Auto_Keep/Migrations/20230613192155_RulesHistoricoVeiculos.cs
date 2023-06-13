using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auto_Keep.Migrations
{
    /// <inheritdoc />
    public partial class RulesHistoricoVeiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Precos_Id_TipoVeiculo",
                table: "Precos");

            migrationBuilder.AddColumn<decimal>(
                name: "Valor_Total",
                table: "HistoricoVeiculos",
                type: "numeric(10,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Precos_Id_TipoVeiculo",
                table: "Precos",
                column: "Id_TipoVeiculo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Precos_Id_TipoVeiculo",
                table: "Precos");

            migrationBuilder.DropColumn(
                name: "Valor_Total",
                table: "HistoricoVeiculos");

            migrationBuilder.CreateIndex(
                name: "IX_Precos_Id_TipoVeiculo",
                table: "Precos",
                column: "Id_TipoVeiculo");
        }
    }
}
