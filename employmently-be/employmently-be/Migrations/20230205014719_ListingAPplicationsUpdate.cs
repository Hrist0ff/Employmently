using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class ListingAPplicationsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "appTime",
                table: "ListingApplications");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "59e4c5a4-ec3a-4270-9d36-b9d758325d96");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "cbf20c1e-5f0a-4460-83ca-2ed903d07df5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "13105510-90c8-4019-ac5e-fc5c49568f98");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bdc0dd9a-63dd-4664-b3a6-a017367ca854", "376a7dea-0cb1-4f88-8732-a50c5f7c062d" });
        }
    }
}
