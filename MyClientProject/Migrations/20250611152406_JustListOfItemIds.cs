using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyClientProject.Migrations
{
    /// <inheritdoc />
    public partial class JustListOfItemIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Items_ItemId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ItemId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ItemId",
                table: "Orders",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Items_ItemId",
                table: "Orders",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId");
        }
    }
}
