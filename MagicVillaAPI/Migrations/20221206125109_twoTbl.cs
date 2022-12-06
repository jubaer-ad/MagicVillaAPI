using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class twoTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VillaNumbers",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    SpecialDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaNumbers", x => x.VillaNo);
                });

            migrationBuilder.CreateTable(
                name: "Villas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Occupency = table.Column<int>(type: "int", nullable: false),
                    Sqft = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amenity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VillaNumbers",
                columns: new[] { "VillaNo", "CreatedAt", "SpecialDetails", "UpdatedAt" },
                values: new object[,]
                {
                    { 101, new DateTime(2022, 12, 6, 18, 51, 9, 134, DateTimeKind.Local).AddTicks(8804), "Exotic pool attached.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 102, new DateTime(2022, 12, 6, 18, 51, 9, 134, DateTimeKind.Local).AddTicks(8805), "360 Sky view.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreatedAt", "Details", "ImageUrl", "Name", "Occupency", "Rate", "Sqft", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Test Amenity", new DateTime(2022, 12, 6, 18, 51, 9, 134, DateTimeKind.Local).AddTicks(8693), "Test details", "https://images.pexels.com/photos/7583935/pexels-photo-7583935.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", "A Villa", 2, 5.0, 750, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Test Amenity", new DateTime(2022, 12, 6, 18, 51, 9, 134, DateTimeKind.Local).AddTicks(8709), "Test details", "https://images.pexels.com/photos/7583935/pexels-photo-7583935.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", "B Villa", 3, 6.0, 850, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillaNumbers");

            migrationBuilder.DropTable(
                name: "Villas");
        }
    }
}
