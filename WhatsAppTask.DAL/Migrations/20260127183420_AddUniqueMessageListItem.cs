using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsAppTask.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueMessageListItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageListItems_MessageListId",
                table: "MessageListItems");

            migrationBuilder.CreateIndex(
                name: "IX_MessageListItems_MessageListId_ContactId",
                table: "MessageListItems",
                columns: new[] { "MessageListId", "ContactId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageListItems_MessageListId_ContactId",
                table: "MessageListItems");

            migrationBuilder.CreateIndex(
                name: "IX_MessageListItems_MessageListId",
                table: "MessageListItems",
                column: "MessageListId");
        }
    }
}
