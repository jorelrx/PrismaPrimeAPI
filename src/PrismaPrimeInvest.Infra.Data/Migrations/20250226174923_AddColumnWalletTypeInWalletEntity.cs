using Microsoft.EntityFrameworkCore.Migrations;
using PrismaPrimeInvest.Domain.Enums;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnWalletTypeInWalletEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WalletType",
                table: "Wallet",
                type: "int",
                nullable: false,
                defaultValue: (int)WalletTypeEnum.Undefined);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletType",
                table: "Wallet");
        }
    }
}
