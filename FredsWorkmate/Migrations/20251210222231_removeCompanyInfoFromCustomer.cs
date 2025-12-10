using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FredsWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class removeCompanyInfoFromCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CompanyInformations_CompanyId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CompanyInformations_CompanyId",
                table: "Customers",
                column: "CompanyId",
                principalTable: "CompanyInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
