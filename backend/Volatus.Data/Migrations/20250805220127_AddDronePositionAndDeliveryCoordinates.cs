using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volatus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDronePositionAndDeliveryCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentX",
                table: "Drones",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentY",
                table: "Drones",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCharging",
                table: "Drones",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMovementTime",
                table: "Drones",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Speed",
                table: "Drones",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TargetX",
                table: "Drones",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TargetY",
                table: "Drones",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "X",
                table: "Deliveries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Y",
                table: "Deliveries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentX",
                table: "Drones");

            migrationBuilder.DropColumn(
                name: "CurrentY",
                table: "Drones");

            migrationBuilder.DropColumn(
                name: "IsCharging",
                table: "Drones");

            migrationBuilder.DropColumn(
                name: "LastMovementTime",
                table: "Drones");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Drones");

            migrationBuilder.DropColumn(
                name: "TargetX",
                table: "Drones");

            migrationBuilder.DropColumn(
                name: "TargetY",
                table: "Drones");

            migrationBuilder.DropColumn(
                name: "X",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Deliveries");
        }
    }
}
