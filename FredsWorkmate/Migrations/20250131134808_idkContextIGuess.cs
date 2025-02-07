using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FredsWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class idkContextIGuess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoicePositions_Invoices_InvoiceId",
                table: "InvoicePositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_InvoiceBuyers_BuyerId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_InvoiceSellers_SellerId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_BuyerId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SellerId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Invoices");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "InvoicePositions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceBuyers_Invoices_Id",
                table: "InvoiceBuyers",
                column: "Id",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicePositions_Invoices_InvoiceId",
                table: "InvoicePositions",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceSellers_Invoices_Id",
                table: "InvoiceSellers",
                column: "Id",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceBuyers_Invoices_Id",
                table: "InvoiceBuyers");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicePositions_Invoices_InvoiceId",
                table: "InvoicePositions");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceSellers_Invoices_Id",
                table: "InvoiceSellers");

            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "Invoices",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "InvoicePositions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BuyerId",
                table: "Invoices",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SellerId",
                table: "Invoices",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicePositions_Invoices_InvoiceId",
                table: "InvoicePositions",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_InvoiceBuyers_BuyerId",
                table: "Invoices",
                column: "BuyerId",
                principalTable: "InvoiceBuyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_InvoiceSellers_SellerId",
                table: "Invoices",
                column: "SellerId",
                principalTable: "InvoiceSellers",
                principalColumn: "Id");
        }
    }
}
