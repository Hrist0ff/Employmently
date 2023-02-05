using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationTImefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "appTime",
                table: "ListingApplications");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "0f8e7719-dac5-40d6-a928-58e073fcbe3e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "d05ab7bb-58ee-4c5d-a729-8ba1e0e57ee9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "11fbb5d8-155a-40e7-9770-b19dafc983a6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "647919b1-7adf-48f6-92a3-3e7158cc013d", "c9d5afc5-f550-4db3-9f58-1be165fd7ad8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "appTime",
                table: "ListingApplications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "23a9a905-ecac-43cf-8632-78d385edd50f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "85551e72-fa81-498a-a2fa-8c8c953fcd2e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "7547d648-47a7-4905-9c31-5d24aa5a1872");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4db12b2a-7649-46d5-b590-d133c426cd97", "01fa18ba-0273-4856-a146-53fa0b002d65" });
        }
    }
}
