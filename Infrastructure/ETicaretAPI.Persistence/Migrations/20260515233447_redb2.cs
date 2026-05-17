using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaretAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class redb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedOrder_Orders_OrderId",
                table: "CompletedOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompletedOrder",
                table: "CompletedOrder");

            migrationBuilder.RenameTable(
                name: "CompletedOrder",
                newName: "CompletedOrders");

            migrationBuilder.RenameIndex(
                name: "IX_CompletedOrder_OrderId",
                table: "CompletedOrders",
                newName: "IX_CompletedOrders_OrderId");

            migrationBuilder.AddColumn<Guid>(
                name: "EndpointId",
                table: "AspNetRoles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompletedOrders",
                table: "CompletedOrders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Endpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionType = table.Column<string>(type: "text", nullable: false),
                    HttpType = table.Column<string>(type: "text", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    MenuId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Endpoints_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderCode",
                table: "Orders",
                column: "OrderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_EndpointId",
                table: "AspNetRoles",
                column: "EndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_Endpoints_MenuId",
                table: "Endpoints",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Endpoints_EndpointId",
                table: "AspNetRoles",
                column: "EndpointId",
                principalTable: "Endpoints",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedOrders_Orders_OrderId",
                table: "CompletedOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Endpoints_EndpointId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_CompletedOrders_Orders_OrderId",
                table: "CompletedOrders");

            migrationBuilder.DropTable(
                name: "Endpoints");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderCode",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_EndpointId",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompletedOrders",
                table: "CompletedOrders");

            migrationBuilder.DropColumn(
                name: "EndpointId",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "CompletedOrders",
                newName: "CompletedOrder");

            migrationBuilder.RenameIndex(
                name: "IX_CompletedOrders_OrderId",
                table: "CompletedOrder",
                newName: "IX_CompletedOrder_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompletedOrder",
                table: "CompletedOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedOrder_Orders_OrderId",
                table: "CompletedOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
