using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FredsWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class NewEnums : Migration
    {
        protected void UpCountry(MigrationBuilder migrationBuilder, string table)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: table,
                newName: "CountryOld"
            );

            migrationBuilder.AddColumn<int>(
                name: "Country",
                table: table,
                type: "integer",
                defaultValue: 0,
                nullable: false);

            migrationBuilder.Sql(@"
                UPDATE """ + table + @""" SET ""Country"" = CASE 
                    WHEN ""CountryOld"" = 'DE' THEN 276
                    WHEN ""CountryOld"" = 'US' THEN 840
                    ELSE 0
                END
            ");

            migrationBuilder.DropColumn(name: "CountryOld", table: table);

            migrationBuilder.AlterColumn<int>(
                name: "Country",
                table: table,
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0 // This removes the default
            );
        }

        protected void DownCountry(MigrationBuilder migrationBuilder, string table)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryOld",
                table: table,
                type: "text",
                nullable: false,
                defaultValue: "UNKNOWN");

            migrationBuilder.Sql(@"
                UPDATE """ + table + @""" SET ""CountryOld"" = CASE 
                    WHEN ""Country"" = 276 THEN 'DE'
                    WHEN ""Country"" = 840 THEN 'US'
                    ELSE 'UNKNOWN'
                END
            ");

            migrationBuilder.DropColumn(name: "Country", table: table);

            migrationBuilder.RenameColumn(
                name: "CountryOld",
                table: table,
                newName: "Country"
            );
        }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpCountry(migrationBuilder, "InvoiceSellers");
            UpCountry(migrationBuilder, "InvoiceBuyers");
            UpCountry(migrationBuilder, "Addresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            DownCountry(migrationBuilder, "InvoiceSellers");
            DownCountry(migrationBuilder, "InvoiceBuyers");
            DownCountry(migrationBuilder, "Addresses");
        }
    }
}
