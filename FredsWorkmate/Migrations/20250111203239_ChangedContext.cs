using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FredsWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class ChangedContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_InvoiceBuyer_BuyerId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_InvoiceSeller_SellerId",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceSeller",
                table: "InvoiceSeller");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceBuyer",
                table: "InvoiceBuyer");

            migrationBuilder.RenameTable(
                name: "InvoiceSeller",
                newName: "InvoiceSellers");

            migrationBuilder.RenameTable(
                name: "InvoiceBuyer",
                newName: "InvoiceBuyers");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceSellers",
                table: "InvoiceSellers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceBuyers",
                table: "InvoiceBuyers",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_InvoiceBuyers_BuyerId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_InvoiceSellers_SellerId",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceSellers",
                table: "InvoiceSellers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceBuyers",
                table: "InvoiceBuyers");

            migrationBuilder.RenameTable(
                name: "InvoiceSellers",
                newName: "InvoiceSeller");

            migrationBuilder.RenameTable(
                name: "InvoiceBuyers",
                newName: "InvoiceBuyer");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "Invoices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceSeller",
                table: "InvoiceSeller",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceBuyer",
                table: "InvoiceBuyer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_InvoiceBuyer_BuyerId",
                table: "Invoices",
                column: "BuyerId",
                principalTable: "InvoiceBuyer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_InvoiceSeller_SellerId",
                table: "Invoices",
                column: "SellerId",
                principalTable: "InvoiceSeller",
                principalColumn: "Id");
        }
    }
}
