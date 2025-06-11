using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyClientProject.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemsManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemOrder");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Items",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Items",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "ItemOrder",
                columns: table => new
                {
                    ItemsItemId = table.Column<int>(type: "int", nullable: false),
                    OrdersOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemOrder", x => new { x.ItemsItemId, x.OrdersOrderId });
                    table.ForeignKey(
                        name: "FK_ItemOrder_Items_ItemsItemId",
                        column: x => x.ItemsItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemOrder_Orders_OrdersOrderId",
                        column: x => x.OrdersOrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrder_OrdersOrderId",
                table: "ItemOrder",
                column: "OrdersOrderId");
        }
    }
}
