using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsAppTask.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixConversationMessageRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_PhoneNumber",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserId_PhoneNumber",
                table: "Contacts",
                columns: new[] { "UserId", "PhoneNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_UserId_PhoneNumber",
                table: "Contacts");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PhoneNumber",
                table: "Contacts",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts",
                column: "UserId");
        }
    }
}
