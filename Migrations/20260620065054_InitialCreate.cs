using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace FitnessPP.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Patronymic = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Patronymic = table.Column<string>(type: "TEXT", nullable: true),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    TrainerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LockerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Client_Service",
                columns: table => new
                {
                    client_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    service_id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client_Service", x => new { x.client_id, x.service_id });
                    table.ForeignKey(
                        name: "FK_Client_Service_Clients_client_id",
                        column: x => x.client_id,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Client_Service_Services_service_id",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lockers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lockers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lockers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Lockers",
                columns: new[] { "Id", "ClientId", "Number" },
                values: new object[,]
                {
                    { new Guid("01c0f3ed-f25b-49f2-8dab-165dba955a9a"), null, 12 },
                    { new Guid("08af3429-7154-4a83-bcf8-833f1e7cdf4e"), null, 4 },
                    { new Guid("0c7af1e7-bd39-46e9-89c7-1e8d3e6df1b8"), null, 15 },
                    { new Guid("0e41e21b-4a8f-49a5-b398-f0bb0f58253d"), null, 16 },
                    { new Guid("109b6aed-7fb6-42d0-b0e5-c388a92da229"), null, 7 },
                    { new Guid("350ae3c0-b734-49be-a9df-fcc1e205f802"), null, 14 },
                    { new Guid("3637610d-ef7d-495d-956a-6b2680323bb1"), null, 1 },
                    { new Guid("4c55cf23-025c-4085-b6cb-ee97026c03dc"), null, 3 },
                    { new Guid("5b7ce47a-54b9-4b52-8a0f-a4c2ddbdf871"), null, 8 },
                    { new Guid("7973fdec-0a81-4f1d-b89d-1832caee562e"), null, 2 },
                    { new Guid("79b6996b-a629-46a7-b7eb-ca88909aa3ce"), null, 17 },
                    { new Guid("7ef36b9b-696a-4e4a-b050-ca5b10a8e54b"), null, 18 },
                    { new Guid("873c0306-3e0c-40ab-82d8-49ead0735636"), null, 9 },
                    { new Guid("882f32f7-9f4d-45ce-ba97-aa4dae118c50"), null, 13 },
                    { new Guid("89c4f198-0165-4f1f-8f87-0469626890ac"), null, 5 },
                    { new Guid("94e3b795-e521-4527-86af-8af7a038dfef"), null, 20 },
                    { new Guid("a816fe93-57a3-411a-9c71-8c10e87427a8"), null, 11 },
                    { new Guid("a8242e7b-b834-44f2-954a-481ac27edb3a"), null, 19 },
                    { new Guid("e3bb7fe0-0155-4f8a-826e-367bdd820b7b"), null, 10 },
                    { new Guid("faf1b537-4cfb-4e00-969a-2bde5043f323"), null, 6 }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { "CROSSFIT", "Кроссфит", 500 },
                    { "CRYOSAUNA", "Криосауна", 1000 },
                    { "POOL", "Бассейн", 200 },
                    { "SAUNA", "Сауна", 0 },
                    { "SOLARIUM", "Солярий", 400 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_Service_service_id",
                table: "Client_Service",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_TrainerId",
                table: "Clients",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lockers_ClientId",
                table: "Lockers",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lockers_Number",
                table: "Lockers",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client_Service");

            migrationBuilder.DropTable(
                name: "Lockers");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Trainers");
        }
    }
}
