using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyRegisterAndForgotPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailConfirmationToken",
                table: "AspNetUsers",
                newName: "UniqueIdentifierCompany");

            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentifier",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "fc434277-9a0c-4d4f-822b-76d40034e50c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "456e760b-662a-4ead-a21a-928e00bee42e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "2b2da478-5398-463d-91d1-90953cbfe6be");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a645587c-f4d1-4872-aa08-1ef4f8daf229", "63a31a63-958d-4b11-8c4e-0c3c56aef3df" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "dd73b7cf-5d73-4479-8075-38af6999cde0", "9bb6fefb-174c-486b-940f-5de078e91a53" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueIdentifier",
                table: "Company");

            migrationBuilder.RenameColumn(
                name: "UniqueIdentifierCompany",
                table: "AspNetUsers",
                newName: "EmailConfirmationToken");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "31708f7b-1e8a-4131-9e2b-5b2cf799fbd9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "ffe975c5-9dc0-41bb-8cd9-554cf60e7110");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "9de00e05-b0d1-4a48-a085-502137c085a8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "26ad33c9-c989-4dfd-b8f4-8dcfa2f202c0", "07567fab-3ab8-4fa5-82ce-458df53b0fce" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "95b2f835-e777-4e45-8cdf-726ba738dda1", "ca57e2c0-d71f-4f4e-b0aa-1b09fe92207b" });
        }
    }
}
