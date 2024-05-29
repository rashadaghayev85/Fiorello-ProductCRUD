using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiorelloMVC.Migrations
{
    public partial class CreatedSocialMediaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedias", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 22, 0, 50, 11, 434, DateTimeKind.Local).AddTicks(1431));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 22, 0, 50, 11, 434, DateTimeKind.Local).AddTicks(1432));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 22, 0, 50, 11, 434, DateTimeKind.Local).AddTicks(1433));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 22, 0, 50, 11, 434, DateTimeKind.Local).AddTicks(1518));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 22, 0, 50, 11, 434, DateTimeKind.Local).AddTicks(1519));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 22, 0, 50, 11, 434, DateTimeKind.Local).AddTicks(1520));

            migrationBuilder.InsertData(
                table: "SocialMedias",
                columns: new[] { "Id", "CreatedDate", "Name", "SoftDeleted", "Url" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 22, 0, 50, 11, 434, DateTimeKind.Local).AddTicks(1567), "Instagram", false, "www.instagram.com" },
                    { 2, new DateTime(2024, 5, 22, 0, 50, 11, 434, DateTimeKind.Local).AddTicks(1568), "Twitter", false, "www.twitter.com" },
                    { 3, new DateTime(2024, 5, 22, 0, 50, 11, 434, DateTimeKind.Local).AddTicks(1569), "Facebook", false, "www.facebook.com" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialMedias");

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

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 21, 19, 21, 14, 168, DateTimeKind.Local).AddTicks(4707));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 21, 19, 21, 14, 168, DateTimeKind.Local).AddTicks(4709));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 5, 21, 19, 21, 14, 168, DateTimeKind.Local).AddTicks(4710));
        }
    }
}
