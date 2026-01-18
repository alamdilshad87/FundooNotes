using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class OptSessionId123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtpSessionId",
                table: "Otps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpSessionId",
                table: "Otps");
        }
    }
}
