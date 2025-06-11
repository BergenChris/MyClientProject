using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyClientProject.Migrations
{
    /// <inheritdoc />
    public partial class _1106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Items_ItemsItemId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrdersOrderId",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "ItemOrder");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrdersOrderId",
                table: "ItemOrder",
                newName: "IX_ItemOrder_OrdersOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemOrder",
                table: "ItemOrder",
                columns: new[] { "ItemsItemId", "OrdersOrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ItemOrder_Items_ItemsItemId",
                table: "ItemOrder",
                column: "ItemsItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemOrder_Orders_OrdersOrderId",
                table: "ItemOrder",
                column: "OrdersOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemOrder_Items_ItemsItemId",
                table: "ItemOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemOrder_Orders_OrdersOrderId",
                table: "ItemOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemOrder",
                table: "ItemOrder");

            migrationBuilder.RenameTable(
                name: "ItemOrder",
                newName: "OrderItems");

            migrationBuilder.RenameIndex(
                name: "IX_ItemOrder_OrdersOrderId",
                table: "OrderItems",
                newName: "IX_OrderItems_OrdersOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                columns: new[] { "ItemsItemId", "OrdersOrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Items_ItemsItemId",
                table: "OrderItems",
                column: "ItemsItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrdersOrderId",
                table: "OrderItems",
                column: "OrdersOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
