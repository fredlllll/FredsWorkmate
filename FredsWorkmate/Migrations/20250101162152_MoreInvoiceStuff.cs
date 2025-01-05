using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FredsWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class MoreInvoiceStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "InvoiceSeller",
                newName: "ContactName");

            migrationBuilder.RenameColumn(
                name: "Decription",
                table: "InvoicePositions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "InvoiceBuyer",
                newName: "ContactName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CompanyInformations",
                newName: "ContactName");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "InvoiceSeller",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "CompanyInformations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "InvoiceSeller");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "CompanyInformations");

            migrationBuilder.RenameColumn(
                name: "ContactName",
                table: "InvoiceSeller",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "InvoicePositions",
                newName: "Decription");

            migrationBuilder.RenameColumn(
                name: "ContactName",
                table: "InvoiceBuyer",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ContactName",
                table: "CompanyInformations",
                newName: "Name");
        }
    }
}
