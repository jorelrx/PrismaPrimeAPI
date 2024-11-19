using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePriceAndCreateOpenPriceFundDailyPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "FundDailyPrice",
                newName: "OpenPrice");

            migrationBuilder.AddColumn<double>(
                name: "ClosePrice",
                table: "FundDailyPrice",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosePrice",
                table: "FundDailyPrice");

            migrationBuilder.RenameColumn(
                name: "OpenPrice",
                table: "FundDailyPrice",
                newName: "Price");
        }
    }
}
