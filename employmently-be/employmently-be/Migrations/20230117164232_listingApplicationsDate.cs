using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class listingApplicationsDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "applicationTime",
                table: "ListingApplications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "f5ccd785-4da2-4cc7-ba2b-1dc7147b898b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "28f8ca6f-5749-4824-b15f-e56ff0f63e56");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "88dce622-9ba8-4578-a6ff-4155f2c5dcab");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "982ade35-4933-4a74-9862-bf64da5c8110", "604bad2e-671c-485e-865b-6e55aa9f0b3a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "applicationTime",
                table: "ListingApplications");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "649717bd-94b2-497f-97d4-11a5528447f1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "7b337cdf-28e3-4be2-9b93-4ccb0cf694a7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "2688b9ed-33c8-425a-babb-c760584962f8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "dec06782-3b52-48fa-b52c-eee28552594c", "2c5eb321-c467-4093-81e4-170256e481aa" });
        }
    }
}
