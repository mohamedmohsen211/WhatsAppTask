using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsAppTask.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFailureReasonToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "WhatsAppMessageId",
                table: "Messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppMessageId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
