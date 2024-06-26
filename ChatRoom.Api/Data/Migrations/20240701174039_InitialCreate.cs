﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChatRoom.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    MessageRoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    MessageText = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomName = table.Column<string>(type: "TEXT", nullable: true),
                    RoomDescription = table.Column<string>(type: "TEXT", nullable: true),
                    RoomMembers = table.Column<string>(type: "TEXT", nullable: true),
                    RoomAdmin = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: true),
                    UserPassword = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "MessageId", "MessageRoomId", "MessageText", "MessageUserId" },
                values: new object[,]
                {
                    { 1, 1, "This message is from Admins", 1 },
                    { 2, 2, "This message is from BackEnd", 2 },
                    { 3, 3, "This message is from FrontEnd", 3 }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "RoomAdmin", "RoomDescription", "RoomMembers", "RoomName" },
                values: new object[,]
                {
                    { 1, 1, "Admin is Admin", "[1]", "Admin Room" },
                    { 2, 2, "BackEnd is Admin", "[2]", "BackEnd Room" },
                    { 3, 3, "FrontEnd is Admin", "[3]", "FrontEnd Room" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "UserEmail", "UserName", "UserPassword" },
                values: new object[,]
                {
                    { 1, "admin@test.com", "Admin", "admin" },
                    { 2, "backend@test.com", "BackEnd", "backend" },
                    { 3, "frontend@test.com", "FrontEnd", "frontend" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
