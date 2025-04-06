using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShoppingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderProductRelationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductEntityId",
                table: "OrderProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductEntityId",
                table: "OrderProducts",
                column: "ProductEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Products_ProductEntityId",
                table: "OrderProducts",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Products_ProductEntityId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_ProductEntityId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ProductEntityId",
                table: "OrderProducts");
        }
    }
}
