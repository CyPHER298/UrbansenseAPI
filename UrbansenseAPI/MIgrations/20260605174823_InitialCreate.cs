using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UrbansenseAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ALERTS",
                columns: table => new
                {
                    ID_ALERT = table.Column<decimal>(type: "NUMBER(20,0)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CITY = table.Column<string>(type: "VARCHAR2(255)", nullable: false),
                    TYPE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    SEVERITY = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    MESSAGE = table.Column<string>(type: "VARCHAR2(1000)", nullable: false),
                    REGION = table.Column<string>(type: "VARCHAR2(255)", nullable: false),
                    LATITUDE = table.Column<decimal>(type: "NUMBER(10,6)", nullable: false),
                    LONGITUDE = table.Column<decimal>(type: "NUMBER(10,6)", nullable: false),
                    VALID_FROM = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    VALID_UNTIL = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTS", x => x.ID_ALERT);
                });

            migrationBuilder.CreateTable(
                name: "TRANSIT_LINES",
                columns: table => new
                {
                    ID_LINE = table.Column<decimal>(type: "NUMBER(20,0)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "VARCHAR2(10)", nullable: false),
                    NAME = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    OPERATOR = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    COLOR = table.Column<string>(type: "VARCHAR2(10)", nullable: false),
                    RAIN_VULNERABILITY = table.Column<decimal>(type: "NUMBER(4,2)", nullable: false),
                    RAIN_THRESHOLD_MM = table.Column<decimal>(type: "NUMBER(5,2)", nullable: false),
                    VULNERABLE_SECTION = table.Column<string>(type: "VARCHAR2(255)", nullable: false),
                    AVG_DELAY_PCT = table.Column<byte>(type: "NUMBER(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSIT_LINES", x => x.ID_LINE);
                });

            migrationBuilder.CreateTable(
                name: "WEATHER_RECORDS",
                columns: table => new
                {
                    ID_WEATHER = table.Column<decimal>(type: "NUMBER(20,0)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CITY = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    LATITUDE = table.Column<decimal>(type: "NUMBER(10,6)", nullable: false),
                    LONGITUDE = table.Column<decimal>(type: "NUMBER(10,6)", nullable: false),
                    TEMPERATURE = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                    FEELS_LIKE = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                    HUMIDITY = table.Column<byte>(type: "NUMBER(3)", nullable: true),
                    WIND_SPEED = table.Column<decimal>(type: "NUMBER(6,2)", nullable: true),
                    RAIN_MM = table.Column<decimal>(type: "NUMBER(6,2)", nullable: true),
                    UV_INDEX = table.Column<byte>(type: "NUMBER(3)", nullable: true),
                    CONDITION = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR2(255)", nullable: false),
                    RECORDED_AT = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WEATHER_RECORDS", x => x.ID_WEATHER);
                });

            migrationBuilder.InsertData(
                table: "TRANSIT_LINES",
                columns: new[] { "ID_LINE", "AVG_DELAY_PCT", "CODE", "COLOR", "NAME", "RAIN_THRESHOLD_MM", "RAIN_VULNERABILITY", "OPERATOR", "VULNERABLE_SECTION" },
                values: new object[,]
                {
                    { 1m, (byte)25, "L1", "#0155A8", "Linha 1 - Azul", 15m, 0.7m, "Metrô SP", "Tucuruvi - Jabaquara" },
                    { 2m, (byte)15, "L2", "#007E5E", "Linha 2 - Verde", 20m, 0.5m, "Metrô SP", "Vila Madalena - Vila Prudente" },
                    { 3m, (byte)30, "L3", "#EE3124", "Linha 3 - Vermelha", 10m, 0.8m, "Metrô SP", "Itaquera - Barra Funda" },
                    { 4m, (byte)10, "L4", "#FFD400", "Linha 4 - Amarela", 25m, 0.3m, "ViaQuatro", "Luz - Butantã" },
                    { 5m, (byte)15, "L5", "#9B2990", "Linha 5 - Lilás", 20m, 0.5m, "ViaMobilidade", "Capão Redondo - Chácara Klabin" },
                    { 6m, (byte)40, "L7", "#DC241F", "Linha 7 - Rubi", 8m, 0.9m, "CPTM", "Luz - Francisco Morato" },
                    { 7m, (byte)35, "L8", "#97999B", "Linha 8 - Diamante", 10m, 0.85m, "ViaMobilidade", "Júlio Prestes - Amador Bueno" },
                    { 8m, (byte)50, "L9", "#01A651", "Linha 9 - Esmeralda", 5m, 0.95m, "ViaMobilidade", "Osasco - Grajaú" },
                    { 9m, (byte)30, "L10", "#009FC4", "Linha 10 - Turquesa", 12m, 0.8m, "CPTM", "Mauá - Brás" },
                    { 10m, (byte)20, "L11", "#F26522", "Linha 11 - Coral", 18m, 0.6m, "CPTM", "Luz - Guaianases" },
                    { 11m, (byte)18, "L12", "#133E8E", "Linha 12 - Safira", 20m, 0.55m, "CPTM", "Brás - Calmon Viana" },
                    { 12m, (byte)10, "L13", "#00B398", "Linha 13 - Jade", 30m, 0.3m, "CPTM", "Engenheiro Goulart - Guarulhos" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALERTS");

            migrationBuilder.DropTable(
                name: "TRANSIT_LINES");

            migrationBuilder.DropTable(
                name: "WEATHER_RECORDS");
        }
    }
}
