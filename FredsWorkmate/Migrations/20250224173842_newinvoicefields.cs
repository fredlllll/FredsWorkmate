using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FredsWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class newinvoicefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FC",
                table: "InvoiceSellers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VA",
                table: "InvoiceSellers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "InvoiceBuyers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "InvoiceBuyers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "InvoiceBuyers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "InvoiceBuyers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "InvoiceBuyers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FC",
                table: "CompanyInformations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VA",
                table: "CompanyInformations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FC",
                table: "InvoiceSellers");

            migrationBuilder.DropColumn(
                name: "VA",
                table: "InvoiceSellers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "InvoiceBuyers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "InvoiceBuyers");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "InvoiceBuyers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "InvoiceBuyers");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "InvoiceBuyers");

            migrationBuilder.DropColumn(
                name: "FC",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "VA",
                table: "CompanyInformations");
        }
    }
}
