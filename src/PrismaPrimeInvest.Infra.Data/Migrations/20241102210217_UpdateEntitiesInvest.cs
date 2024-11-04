using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesInvest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundDailyValue");

            migrationBuilder.RenameColumn(
                name: "MinimumValueDate",
                table: "FundPayment",
                newName: "MinimumPriceDate");

            migrationBuilder.RenameColumn(
                name: "MinimumValue",
                table: "FundPayment",
                newName: "MinimumPrice");

            migrationBuilder.RenameColumn(
                name: "MaximumValueDate",
                table: "FundPayment",
                newName: "MaximumPriceDate");

            migrationBuilder.RenameColumn(
                name: "MaximumValue",
                table: "FundPayment",
                newName: "MaximumPrice");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Fund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "FundDailyPrice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinPrice = table.Column<double>(type: "float", nullable: false),
                    MaxPrice = table.Column<double>(type: "float", nullable: false),
                    FundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundDailyPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundDailyPrice_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundDailyPrice_FundId",
                table: "FundDailyPrice",
                column: "FundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundDailyPrice");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Fund");

            migrationBuilder.RenameColumn(
                name: "MinimumPriceDate",
                table: "FundPayment",
                newName: "MinimumValueDate");

            migrationBuilder.RenameColumn(
                name: "MinimumPrice",
                table: "FundPayment",
                newName: "MinimumValue");

            migrationBuilder.RenameColumn(
                name: "MaximumPriceDate",
                table: "FundPayment",
                newName: "MaximumValueDate");

            migrationBuilder.RenameColumn(
                name: "MaximumPrice",
                table: "FundPayment",
                newName: "MaximumValue");

            migrationBuilder.CreateTable(
                name: "FundDailyValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxValue = table.Column<double>(type: "float", nullable: false),
                    MinValue = table.Column<double>(type: "float", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundDailyValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundDailyValue_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundDailyValue_FundId",
                table: "FundDailyValue",
                column: "FundId");
        }
    }
}
