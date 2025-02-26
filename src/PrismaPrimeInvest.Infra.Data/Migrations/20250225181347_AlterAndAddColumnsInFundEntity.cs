using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterAndAddColumnsInFundEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalShares",
                table: "Fund",
                newName: "TotalShareholders");

            migrationBuilder.AlterColumn<int>(
                name: "TotalShareholders",
                table: "Fund",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "DividendYield",
                table: "Fund",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DividendYield",
                table: "Fund");

            migrationBuilder.AlterColumn<double>(
                name: "TotalShareholders",
                table: "Fund",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.RenameColumn(
                name: "TotalShareholders",
                table: "Fund",
                newName: "TotalShares");
        }
    }
}
