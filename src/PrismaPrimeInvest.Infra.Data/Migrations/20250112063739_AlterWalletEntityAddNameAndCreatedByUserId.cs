using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterWalletEntityAddNameAndCreatedByUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Wallet",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Wallet",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Wallet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wallet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wallet");
        }
    }
}
