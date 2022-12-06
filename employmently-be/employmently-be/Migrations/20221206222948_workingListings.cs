using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class workingListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorID",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CompanyyId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AuthorID",
                table: "Listings",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_AuthorID",
                table: "Listings",
                newName: "IX_Listings_AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Listings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "43c1797a-6161-477a-a643-17b89676b710");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "b342a62f-1f09-4c46-9dcc-a2bcf76c715f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "0e733eb4-ed97-42d6-a0e0-7851e4a817ba");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "10929973-04db-4e64-8291-e3514095b2a5", "04184d23-0b85-4407-a216-9f7185a23248" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "20f351b6-a185-4454-b5d1-8950a17156b6", "de5e3dc7-615b-490a-b714-bc59bbe544f9" });

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorId",
                table: "Listings",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorId",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Listings",
                newName: "AuthorID");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_AuthorId",
                table: "Listings",
                newName: "IX_Listings_AuthorID");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorID",
                table: "Listings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyyId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "84c08bd1-ec3e-40f3-a806-a44d7ff9ff9a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "1f017ec5-e94d-4b77-8928-b21de343001c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "804aca74-d672-4d85-983b-4da308cf59df");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "CompanyyId", "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { null, "98433601-8c97-4346-8a33-c950860fd3ae", "60588e69-6583-49ce-b4a4-711d01d3ca78" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "CompanyyId", "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { null, "8a095b00-0ce0-4e03-bc80-7366696157a0", "3722386e-9b09-4ee8-884e-3e6f922b4d62" });

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorID",
                table: "Listings",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
