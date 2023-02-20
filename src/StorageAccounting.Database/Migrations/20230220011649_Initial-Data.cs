using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StorageAccounting.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "Name", "RequiredArea" },
                values: new object[,]
                {
                    { 1, "Router", 1 },
                    { 2, "Switch", 2 },
                    { 3, "Server", 5 },
                    { 4, "Cluster", 50 },
                    { 5, "Miner", 4 }
                });

            migrationBuilder.InsertData(
                table: "StorageRooms",
                columns: new[] { "Id", "Name", "TotalArea" },
                values: new object[,]
                {
                    { 1, "Dorm: Lomonosova 35", 10 },
                    { 2, "Office: Kyiv, Polyva 21", 250 },
                    { 3, "Verkhovna Rada of Ukraine: Kyiv, Bankova 6-8", 1000 },
                    { 4, "Frex: Faculty", 800 },
                    { 5, "Storage: general", 50 },
                    { 6, "Shelter: it must be empty, because you should be there when air alarm", 100 }
                });

            migrationBuilder.InsertData(
                table: "RentingContracts",
                columns: new[] { "Id", "EquipmentCount", "EquipmentId", "RoomId" },
                values: new object[,]
                {
                    { 1, 3, 2, 1 },
                    { 2, 2, 1, 1 },
                    { 3, 20, 1, 2 },
                    { 4, 15, 2, 2 },
                    { 5, 10, 3, 2 },
                    { 6, 10, 2, 2 },
                    { 7, 15, 2, 2 },
                    { 8, 150, 1, 3 },
                    { 9, 50, 2, 3 },
                    { 10, 10, 3, 3 },
                    { 11, 6, 4, 4 },
                    { 12, 60, 1, 4 },
                    { 13, 40, 1, 4 },
                    { 14, 50, 2, 4 },
                    { 15, 35, 1, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RentingContracts",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "StorageRooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "StorageRooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StorageRooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StorageRooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StorageRooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "StorageRooms",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
