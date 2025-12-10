using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FredsWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceDeliveryDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DeliveryDate",
                table: "Invoices",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Invoices");
        }
    }
}
