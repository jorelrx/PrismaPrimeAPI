using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateWalletAndUpdateFundAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletFund_User_UserId",
                table: "WalletFund");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "WalletFund",
                newName: "WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_WalletFund_UserId",
                table: "WalletFund",
                newName: "IX_WalletFund_WalletId");

            migrationBuilder.AddColumn<double>(
                name: "MaxPrice",
                table: "Fund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinPrice",
                table: "Fund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallet_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletFund_Wallet_WalletId",
                table: "WalletFund",
                column: "WalletId",
                principalTable: "Wallet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletFund_Wallet_WalletId",
                table: "WalletFund");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "Fund");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "WalletFund",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WalletFund_WalletId",
                table: "WalletFund",
                newName: "IX_WalletFund_UserId");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "WalletFund",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletFund_User_UserId",
                table: "WalletFund",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
