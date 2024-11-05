using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityPurchasePriceAndPurchaseDateInWalletFund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "WalletFund",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "PurchasePrice",
                table: "WalletFund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "WalletFund",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "WalletFund");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "WalletFund");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "WalletFund");
        }
    }
}
