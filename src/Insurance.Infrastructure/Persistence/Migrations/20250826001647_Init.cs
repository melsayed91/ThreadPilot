using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Insurance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsurancePolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonalNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    MonthlyCostAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyCostCurrency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    VehicleRegNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePolicies", x => x.Id);
                    table.CheckConstraint("CK_InsurancePolicies_CarRequiresReg", "\"Type\" <> 3 OR \"VehicleRegNumber\" IS NOT NULL");
                });

            migrationBuilder.InsertData(
                table: "InsurancePolicies",
                columns: new[] { "Id", "MonthlyCostAmount", "MonthlyCostCurrency", "PersonalNumber", "Type", "VehicleRegNumber" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 10m, "USD", "19650101-1234", 1, null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 20m, "USD", "19650101-1234", 2, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 30m, "USD", "19650101-1234", 3, "ABC123" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 30m, "USD", "19650101-1234", 3, "XYZ999" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), 10m, "USD", "19700101-1111", 1, null },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), 20m, "USD", "19700101-1111", 2, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_PersonalNumber",
                table: "InsurancePolicies",
                column: "PersonalNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_PersonalNumber_VehicleRegNumber",
                table: "InsurancePolicies",
                columns: new[] { "PersonalNumber", "VehicleRegNumber" },
                unique: true,
                filter: "\"Type\" = 3 AND \"VehicleRegNumber\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsurancePolicies");
        }
    }
}
