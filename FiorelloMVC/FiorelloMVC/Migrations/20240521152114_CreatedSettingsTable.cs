using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiorelloMVC.Migrations
{
    public partial class CreatedSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 21, 19, 21, 14, 168, DateTimeKind.Local).AddTicks(4631));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 21, 19, 21, 14, 168, DateTimeKind.Local).AddTicks(4633));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 21, 19, 21, 14, 168, DateTimeKind.Local).AddTicks(4634));

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "CreatedDate", "Key", "SoftDeleted", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 21, 19, 21, 14, 168, DateTimeKind.Local).AddTicks(4707), "HeaderLogo", false, "logo.png" },
                    { 2, new DateTime(2024, 5, 21, 19, 21, 14, 168, DateTimeKind.Local).AddTicks(4709), "Phone", false, "67891012" },
                    { 3, new DateTime(2024, 5, 21, 19, 21, 14, 168, DateTimeKind.Local).AddTicks(4710), "Address", false, "Ehmedli" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 15, 16, 19, 23, 732, DateTimeKind.Local).AddTicks(7385));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 15, 16, 19, 23, 732, DateTimeKind.Local).AddTicks(7396));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 15, 16, 19, 23, 732, DateTimeKind.Local).AddTicks(7397));
        }
    }
}
