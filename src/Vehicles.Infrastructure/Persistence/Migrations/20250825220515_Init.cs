using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vehicles.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Make = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Model = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Vin = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Make", "Model", "RegistrationNumber", "Vin", "Year" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Tesla", "Model 3", "ABC123", "VIN-A", 2020 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Volvo", "XC90", "XYZ999", "VIN-X", 2019 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Toyota", "Corolla", "KLM456", "VIN-K", 2018 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RegistrationNumber",
                table: "Vehicles",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Vin",
                table: "Vehicles",
                column: "Vin",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
