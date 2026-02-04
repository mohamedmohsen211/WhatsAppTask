using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsAppTask.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadeDeleteFromContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Contacts_ContactId",
                table: "Conversations");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Contacts_ContactId",
                table: "Conversations",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Contacts_ContactId",
                table: "Conversations");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Contacts_ContactId",
                table: "Conversations",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id");
        }
    }
}
