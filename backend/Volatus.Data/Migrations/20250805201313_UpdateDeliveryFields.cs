using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volatus.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerPhone",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ScheduledDate",
                table: "Deliveries");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerAddress",
                table: "Deliveries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerAddress",
                table: "Deliveries",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhone",
                table: "Deliveries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledDate",
                table: "Deliveries",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
