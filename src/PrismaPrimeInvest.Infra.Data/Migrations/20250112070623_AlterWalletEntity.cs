using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterWalletEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_CreatedById",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_CreatedById",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Wallet");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Wallet",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Wallet",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_CreatedByUserId",
                table: "Wallet",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_CreatedByUserId",
                table: "Wallet",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_CreatedByUserId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_CreatedByUserId",
                table: "Wallet");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Wallet",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Wallet",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Wallet",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_CreatedById",
                table: "Wallet",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_CreatedById",
                table: "Wallet",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
