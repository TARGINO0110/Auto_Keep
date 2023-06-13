using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Auto_Keep.Migrations
{
    /// <inheritdoc />
    public partial class InitialAutoKeep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstoqueMonetarios",
                columns: table => new
                {
                    Id_Estoque = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nota = table.Column<bool>(type: "boolean", nullable: true),
                    Moeda = table.Column<bool>(type: "boolean", nullable: true),
                    DescValor = table.Column<string>(type: "text", nullable: true),
                    Qtd = table.Column<int>(type: "integer", nullable: true),
                    Valor_Nota_Moeda = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    Dt_Atualizado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoqueMonetarios", x => x.Id_Estoque);
                });

            migrationBuilder.CreateTable(
                name: "TiposVeiculos",
                columns: table => new
                {
                    Id_TipoVeiculo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sigla_Veiculo = table.Column<char>(type: "character(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposVeiculos", x => x.Id_TipoVeiculo);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoVeiculos",
                columns: table => new
                {
                    Id_HistVeiculo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlacaVeiculo = table.Column<string>(type: "text", nullable: true),
                    DataHora_Entrada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataHora_Saida = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Pago = table.Column<bool>(type: "boolean", nullable: true),
                    Valor_Pago = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    Valor_Troco = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    Id_TiposVeiculos = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoVeiculos", x => x.Id_HistVeiculo);
                    table.ForeignKey(
                        name: "FK_HistoricoVeiculos_TiposVeiculos_Id_TiposVeiculos",
                        column: x => x.Id_TiposVeiculos,
                        principalTable: "TiposVeiculos",
                        principalColumn: "Id_TipoVeiculo");
                });

            migrationBuilder.CreateTable(
                name: "Precos",
                columns: table => new
                {
                    Id_Preco = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Preco = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    Id_TipoVeiculo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Precos", x => x.Id_Preco);
                    table.ForeignKey(
                        name: "FK_Precos_TiposVeiculos_Id_TipoVeiculo",
                        column: x => x.Id_TipoVeiculo,
                        principalTable: "TiposVeiculos",
                        principalColumn: "Id_TipoVeiculo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoVeiculos_Id_TiposVeiculos",
                table: "HistoricoVeiculos",
                column: "Id_TiposVeiculos");

            migrationBuilder.CreateIndex(
                name: "IX_Precos_Id_TipoVeiculo",
                table: "Precos",
                column: "Id_TipoVeiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstoqueMonetarios");

            migrationBuilder.DropTable(
                name: "HistoricoVeiculos");

            migrationBuilder.DropTable(
                name: "Precos");

            migrationBuilder.DropTable(
                name: "TiposVeiculos");
        }
    }
}
