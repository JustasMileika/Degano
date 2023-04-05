using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Degano_API.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FuelPrice_priceLPG",
                table: "GasStations",
                newName: "FuelPriceHourly_priceLPG");

            migrationBuilder.RenameColumn(
                name: "FuelPrice_priceDiesel",
                table: "GasStations",
                newName: "FuelPriceHourly_priceDiesel");

            migrationBuilder.RenameColumn(
                name: "FuelPrice_price98",
                table: "GasStations",
                newName: "FuelPriceHourly_price98");

            migrationBuilder.RenameColumn(
                name: "FuelPrice_price95",
                table: "GasStations",
                newName: "FuelPriceHourly_price95");

            migrationBuilder.AddColumn<DateTime>(
                name: "DailyUpdateTimestamp",
                table: "GasStations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FuelPriceDaily_price95",
                table: "GasStations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FuelPriceDaily_price98",
                table: "GasStations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FuelPriceDaily_priceDiesel",
                table: "GasStations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FuelPriceDaily_priceLPG",
                table: "GasStations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HourlyUpdateTimestamp",
                table: "GasStations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyUpdateTimestamp",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "FuelPriceDaily_price95",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "FuelPriceDaily_price98",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "FuelPriceDaily_priceDiesel",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "FuelPriceDaily_priceLPG",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "HourlyUpdateTimestamp",
                table: "GasStations");

            migrationBuilder.RenameColumn(
                name: "FuelPriceHourly_priceLPG",
                table: "GasStations",
                newName: "FuelPrice_priceLPG");

            migrationBuilder.RenameColumn(
                name: "FuelPriceHourly_priceDiesel",
                table: "GasStations",
                newName: "FuelPrice_priceDiesel");

            migrationBuilder.RenameColumn(
                name: "FuelPriceHourly_price98",
                table: "GasStations",
                newName: "FuelPrice_price98");

            migrationBuilder.RenameColumn(
                name: "FuelPriceHourly_price95",
                table: "GasStations",
                newName: "FuelPrice_price95");
        }
    }
}
