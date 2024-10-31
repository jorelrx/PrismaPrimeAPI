using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFundsDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundBestDay");

            migrationBuilder.DropColumn(
                name: "AverageValue",
                table: "FundPayment");

            migrationBuilder.RenameColumn(
                name: "AnalysisMonth",
                table: "FundPayment",
                newName: "PaymentDate");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "UserFund",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BestBuyDay",
                table: "Fund",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "UserFund");

            migrationBuilder.DropColumn(
                name: "BestBuyDay",
                table: "Fund");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "FundPayment",
                newName: "AnalysisMonth");

            migrationBuilder.AddColumn<double>(
                name: "AverageValue",
                table: "FundPayment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "FundBestDay",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BestDay = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundBestDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundBestDay_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundBestDay_FundId",
                table: "FundBestDay",
                column: "FundId");
        }
    }
}
