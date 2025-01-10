using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFundEntityAddCnpjAndOthersThreeAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Fund",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "NetAssetValue",
                table: "Fund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NetAssetValuePerShare",
                table: "Fund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "QtyQuotasIssued",
                table: "Fund",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalShares",
                table: "Fund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "NetAssetValue",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "NetAssetValuePerShare",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "QtyQuotasIssued",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "TotalShares",
                table: "Fund");
        }
    }
}
